using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SocialService.Core;
using SocialService.Core.Interfaces;
using SocialService.Core.Models;
using SocialService.DataAccess.Extensions;

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

        public async Task Subscribe(int userId, int subscritptionId)
        {
            var user = await GetTrackedUserById(userId);
            var sub = await GetTrackedUserById(subscritptionId);
            await _context.Subscribers.AddAsync(
                new Subscriber{
                    UserId = userId,
                    SubscriptionId = subscritptionId
                });
            ++user.Subscriptions;
            ++sub.Subscribers;
            await _context.SaveChangesAsync();
        }

        public async Task Unsubscribe(int userId, int subscriptionId)
        {
            var user = await GetTrackedUserById(userId);
            var sub = await GetTrackedUserById(subscriptionId);
            await _context.Subscribers
                    .Where(s => s.UserId == userId && s.SubscriptionId == subscriptionId)
                    .ExecuteDeleteAsync();
            --user.Subscriptions;
            --sub.Subscribers;
            await _context.SaveChangesAsync();
        }

        public async Task GiveUserXp(int userId, int xpCnt)
        {
            var user = await GetTrackedUserById(userId);
            user.Xp += xpCnt;
            user.WeeklyXp += xpCnt;
            await _context.SaveChangesAsync();
        }

        public IEnumerable<User> GetUsersByUsername(string userName, int page, int pageSize = 10)
        {
            return _context.Users
                .AsNoTracking()
                .Where(u => u.Username.StartsWith(userName))
                .OrderByDescending(u => u.Username == userName)
                .ThenBy(u => u.Username)
                .Page(page, pageSize)
                .Select(u => _mapper.Map<User>(u));
        }

        public async Task<User[]> GetRecommendation(int userId, int cnt = 10)
        {
            var user = await GetUserById(userId);
            int leaderboardId = user.LeaderboardId;
            var suggestion = await _context.Users
                                .AsNoTracking()
                                .Where(u => u.LeaderboardId == leaderboardId)
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

        private async Task<UserEntity> GetTrackedUserById(int userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId) ?? throw new NotFoundException($"No user with id: {userId}");
        }
    }
}