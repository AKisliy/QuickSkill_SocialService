using System.Net.Security;
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
            CreateLeaderboards(leagues);
            await _userRepository.UpdateUsersLeaderboards(leagues); // логика НЕ реализована
        }

        public async Task UpdateLeagues()
        {
            // Логика обновления лиг и обнуления weeklyXp(в конце недели)
        }

        // нужно добавить логику заполнения ботами
        private static void CreateLeaderboards(Dictionary<int, List<UserLeaderboardUpdate>> usersInLeagues)
        {
            Random rnd = new();
            int id = 1;
            foreach(int league in usersInLeagues.Keys)
            {
                int cnt = usersInLeagues[league].Count;
                int leaderboardsCnt =  (cnt / leaderboardSize)  + ((cnt % leaderboardSize) == 0 ? 0 : 1);
                int left = id;
                int right = id + leaderboardsCnt - 1;
                foreach(var u in usersInLeagues[league])
                {
                    u.LeaderboardId = rnd.Next(left, right + 1);
                }
                id = right + 1;
            }
        }
    }
}