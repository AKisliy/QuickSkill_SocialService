using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SocialService.Core;
using SocialService.Core.Exceptions;
using SocialService.Core.Interfaces;
using SocialService.Core.Models;
using SocialService.Core.Models.UserModels;
using SocialService.DataAccess.Extensions;
using EFCore.BulkExtensions;
using System.Runtime.CompilerServices;
using System.Reflection;

namespace SocialService.DataAccess.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly SocialServiceContext _context;
        private readonly IMapper _mapper;

        public UserRepository(SocialServiceContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task AddUser(User user)
        {
            var userEntity = _mapper.Map<UserEntity>(user);
            await _context.AddAsync(userEntity);
            await _context.SaveChangesAsync();
        }

        public async Task AddBot(User bot)
        {
            var botEntity = _mapper.Map<UserEntity>(bot);
            botEntity.IsBot = true;
            await _context.AddAsync(botEntity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<UserLeaderboardUpdate>> AddBots(List<User> bots)
        {
            var botsEntityList = bots.Select(b =>
            {
                var enity = _mapper.Map<UserEntity>(b);
                enity.IsBot = true;
                return enity;
            });
            await _context.AddRangeAsync(botsEntityList);
            await _context.SaveChangesAsync();
            return botsEntityList.Select(b => _mapper.Map<UserLeaderboardUpdate>(b)).ToList();
        }

        public async Task DeleteUser(int id)
        {
            await _context.Users.Where(u => u.Id == id).ExecuteDeleteAsync();
            await _context.SaveChangesAsync();
        }

        public async Task Update(User user)
        {
            if(!await HasUserWithId(user.Id))
                throw new NotFoundException($"No user with id: {user.Id}");
            await _context.Users.Where(u => u.Id == user.Id).ExecuteUpdateAsync(s => s
                .SetProperty(u => u.FirstName, _ => user.FirstName)
                .SetProperty(u => u.LastName, _ => user.LastName)
                .SetProperty(u => u.Username, _ => user.Username)
                .SetProperty(u => u.Status, _ => user.Status));
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetUserById(int id)
        {
            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id) ??
                    throw new NotFoundException($"No user with id: {id}");
            return _mapper.Map<User>(user);
        }

        public async Task Subscribe(int userId, int subscriptionId)
        {
            if(await _context.Subscribers.AnyAsync(s => s.UserId == userId && s.SubscriptionId == subscriptionId))
                throw new BadRequestException($"User {userId} already subscribed on {subscriptionId}");
            var user = await GetTrackedUserById(userId);
            var sub = await GetTrackedUserById(subscriptionId);
            await _context.Subscribers.AddAsync(
                new Subscriber{
                    UserId = userId,
                    SubscriptionId = subscriptionId
                });
            ++user.Subscriptions;
            ++sub.Subscribers;
            await _context.SaveChangesAsync();
        }

        public async Task Unsubscribe(int userId, int subscriptionId)
        {
            if(!await _context.Subscribers.AnyAsync(s => s.UserId == userId && s.SubscriptionId == subscriptionId))
                throw new BadRequestException($"User {userId} not subscribed on {subscriptionId}");
            var user = await GetTrackedUserById(userId);
            var sub = await GetTrackedUserById(subscriptionId);
            await _context.Subscribers
                    .Where(s => s.UserId == userId && s.SubscriptionId == subscriptionId)
                    .ExecuteDeleteAsync();
            --user.Subscriptions;
            --sub.Subscribers;
            await _context.SaveChangesAsync();
        }

        public async Task<List<int>> GetSubscriptionsIds(int userId)
        {
            return await _context.Subscribers
                            .AsNoTracking()
                            .Where(s => s.UserId == userId)
                            .Select(s => s.SubscriptionId)
                            .ToListAsync();
        }

        public async Task GiveUserXp(int userId, int xpCnt)
        {
            var user = await GetTrackedUserById(userId);
            user.Xp += xpCnt;
            user.WeeklyXp += xpCnt;
            await _context.SaveChangesAsync();
        }

        public async Task<List<User>> GetUsersByUsername(string userName, int page, int pageSize = 10)
        {
            return await _context.Users
                .AsNoTracking()
                .Where(u => u.Username.StartsWith(userName))
                .OrderByDescending(u => u.Username == userName)
                .ThenBy(u => u.Username)
                .Page(page, pageSize)
                .Select(u => _mapper.Map<User>(u))
                .ToListAsync();
        }

        public async Task<User[]> GetRecommendation(int userId, int cnt = 10)
        {
            var user = await GetUserById(userId);
            int? leaderboardId = user.LeaderboardId;
            var suggestion = await _context.Users
                                .AsNoTracking()
                                .Where(u => u.LeaderboardId == leaderboardId && u.Id != userId && u.LeaderboardId != null)
                                .Take(cnt)
                                .Select(u => _mapper.Map<User>(u))
                                .ToArrayAsync();
            Random.Shared.Shuffle(suggestion);
            return suggestion;
        }

        public async Task<bool> HasUserWithId(int userId)
        {
            return await _context.Users.AnyAsync(u => u.Id == userId);
        }

        public async Task<IEnumerable<User>> GetUserLeaderboard(int userId)
        {
            var user = await GetUserById(userId);
            int? leaderboardId = user.LeaderboardId ?? throw new ConflictException($"User {userId} has no leaderboard");
            return  _context.Users
                        .AsNoTracking()
                        .Where(u => u.LeaderboardId == leaderboardId)
                        .Include(u => u.League)
                        .OrderByDescending(u => u.WeeklyXp)
                        .Select(u => _mapper.Map<User>(u));
        }

        public async Task<int> GetUserLeaguePlace(int userId)
        {
            if(!await HasUserWithId(userId))
                throw new NotFoundException($"User {userId} not found");
            return await _context.Users
                            .Where(u => u.Id == userId)
                            .Include(u => u.League)
                            .Select(u => u.League.HierarchyPlace).FirstAsync();
        }

        public async Task<Dictionary<int, List<UserLeaderboardUpdate>>> GetUserGroupedByLeagueId()
        {
            return await _context.Users
                            .AsNoTracking()
                            .Select(u => new UserLeaderboardUpdate{ Id = u.Id, LeaderboardId = u.LeaderboardId, LeagueId = u.LeagueId, IsBot = u.IsBot })
                            .GroupBy(u => u.LeagueId)
                            .ToDictionaryAsync(p => p.Key, p =>
                            {
                                var users = p.ToList();
                                users.Shuffle();
                                return users;
                            });
        }

        public async Task<Dictionary<int, List<UserLeagueUpdate>>> GetUserGroupedByLeaderboard()
        {
            return  await _context.Users
                            .AsNoTracking()
                            .Where(u => u.LeaderboardId != null)
                            .Select(u => new UserLeagueUpdate{ Id = u.Id, LeaderboardId = u.LeaderboardId, LeagueId = u.LeagueId, WeeklyXp = u.WeeklyXp})
                            .GroupBy(u => (int)u.LeaderboardId)
                            .ToDictionaryAsync(p => p.Key, p => p.OrderBy(p => p.Id).ToList());
        }

        public async Task<List<UserLeagueUpdate>> GetUsersWithoutLeaderboard()
        {
            return await _context.Users
                            .AsNoTracking()
                            .Where(u => u.LeaderboardId == null)
                            .Select(u => new UserLeagueUpdate{ Id = u.Id, LeaderboardId = u.LeaderboardId, LeagueId = u.LeagueId, WeeklyXp = u.WeeklyXp})
                            .ToListAsync();
        }

        public async Task UpdateUsersLeaderboards(Dictionary<int, List<UserLeaderboardUpdate>> usersByLeagues)
        {
            var usersToUpdate = usersByLeagues.SelectMany(pair => pair.Value)
                .Select(u => new UserEntity
                {
                    Id = u.Id,
                    LeaderboardId = u.LeaderboardId,
                    LeagueId = u.LeagueId
                }).ToList();
            await _context.BulkUpdateAsync(usersToUpdate, options => options.PropertiesToInclude = ["LeaderboardId", "LeagueId"]);
        }

        public async Task UpdateUsersLeagues(Dictionary<int, List<UserLeagueUpdate>> userByLeaderboards)
        {
            var usersToUpdate = userByLeaderboards.SelectMany(p => p.Value)
                .Select(u => new UserEntity
                {
                    Id = u.Id,
                    WeeklyXp = 0,
                    LeagueId = u.LeagueId
                }).ToList();
            await _context.BulkUpdateAsync(usersToUpdate, options => options.PropertiesToInclude = ["LeaderboardId", "LeagueId", "WeeklyXp"]);
        }

        private async Task<UserEntity> GetTrackedUserById(int userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId) ?? throw new NotFoundException($"No user with id: {userId}");
        }
    }
}