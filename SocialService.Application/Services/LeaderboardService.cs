using System.Runtime.CompilerServices;
using SocialService.Core.Interfaces;
using SocialService.Core.Interfaces.Services;
using SocialService.Core.Models;
using SocialService.Core.Models.UserModels;

namespace SocialService.Application.Services
{
    public class LeaderboardService: ILeaderboardService
    {
        private readonly IUserRepository _userRepository;

        private const int leaderboardSize = 20;
        private const int undefinedLeagueIndex = 0;

        public LeaderboardService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task CreateWeeklyLeaderboards()
        {
            // по факту нам не нужна вся информация о юзерах
            // нам нужны на вход только UserId и LeagueId, LeaderboardId
            // можно создать DTO 
            var leagues = await _userRepository.GetUserGroupedByLeague();
            await CreateLeaderboards(leagues);
            await _userRepository.UpdateUsersLeaderboards(leagues); // логика НЕ реализована
        }

        public async Task UpdateLeagues()
        {
            // Логика обновления лиг и обнуления weeklyXp(в конце недели)
        }

        // нужно добавить логику заполнения ботами
        private async Task CreateLeaderboards(Dictionary<int, List<UserLeaderboardUpdate>> usersInLeagues)
        {
            int id = 1;
            int nullBotsUsed = 0;
            List<int> leagues = usersInLeagues.Keys.Where(x => x != undefinedLeagueIndex).ToList();
            foreach(int league in leagues)
            {
                if(league == undefinedLeagueIndex)
                    continue;
                int cnt = usersInLeagues[league].Count;
                int leaderboardsCnt =  (cnt / leaderboardSize)  + ((cnt % leaderboardSize) == 0 ? 0 : 1);
                int[] boards = new int[leaderboardsCnt];
                int left = id;
                int delta = 0;
                foreach(var u in usersInLeagues[league])
                {
                    u.LeaderboardId = left + (delta % leaderboardsCnt);
                    ++delta;
                    ++boards[left - id + (delta % leaderboardsCnt)];
                }
                id = left + leaderboardsCnt;
                if((cnt % leaderboardSize) != 0)
                    nullBotsUsed = await FillWithBotsFromLeague(boards, league, left, nullBotsUsed, usersInLeagues);
            }
        }

        private async Task<int> FillWithBotsFromLeague(int[] boards, int league, int startIndex, int nullBotsUsed, Dictionary<int, List<UserLeaderboardUpdate>> usersInLeagues)
        {
            int boardsCnt = boards.Length;
            List<UserLeaderboardUpdate> users = usersInLeagues[league];
            int indexOfLastTakenBot = -1;
            for(int i = 0; i < boardsCnt; ++i)
            {
                if(boards[i] == leaderboardSize)
                    continue;
                int curLeaderboardIndex = startIndex + i;
                while(boards[i] < leaderboardSize && ++indexOfLastTakenBot < users.Count)
                {
                    if(users[indexOfLastTakenBot].IsBot)
                    {
                        users[indexOfLastTakenBot].LeaderboardId = curLeaderboardIndex;
                        ++boards[i];
                    }
                }
                if(boards[i] != leaderboardSize)
                {
                    int needBots = leaderboardSize - boards[i];
                    nullBotsUsed = await FillWithBotsWithNoLeague(curLeaderboardIndex, needBots, league, nullBotsUsed, usersInLeagues);
                }
            }
            return nullBotsUsed;
        }

        private async Task<int> FillWithBotsWithNoLeague(int boardId, int botsNeeded, int league, int nullBotsUsed, Dictionary<int, List<UserLeaderboardUpdate>> usersInLeagues)
        {
            if(!usersInLeagues.ContainsKey(undefinedLeagueIndex))
            {
                usersInLeagues[undefinedLeagueIndex] = [];
                await CreateBots(botsNeeded * 2, usersInLeagues[undefinedLeagueIndex]);
            } 
            List<UserLeaderboardUpdate> nullBots = usersInLeagues[undefinedLeagueIndex];
            int botsLeft = nullBots.Count - nullBotsUsed;
            int needToCreate = botsLeft - botsNeeded;
            if(needToCreate < 0)
                await CreateBots(-needToCreate * 2, nullBots);
            int i = 0;
            while(botsNeeded > 0)
            {
                if(!nullBots[nullBotsUsed + i].IsBot)
                {
                    ++i;
                    continue;
                }
                nullBots[nullBotsUsed + i].LeaderboardId = boardId;
                nullBots[nullBotsUsed + i].LeagueId = league;
                --botsNeeded;
                ++i;
            }
            return nullBotsUsed + i;
        }

        private async Task CreateBots(int cnt, List<UserLeaderboardUpdate> bots)
        {
            System.Console.WriteLine("called");
            List<User> newBots = [];
            while(cnt-- > 0)
            {
                newBots.Add(new User{ FirstName = "Bot", LastName = "Bot", Username = "Bot"});
            }
            bots.AddRange(await _userRepository.AddBots(newBots));
        }
    }
}