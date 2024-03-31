using SocialService.Core.Models;
using SocialService.Core.Models.UserModels;

namespace SocialService.Core.Interfaces
{
    public interface IUserRepository
    {
        public Task AddUser(User user);

        public Task AddBot(User bot);

        public Task DeleteUser(int id);

        public Task Update(User user);

        public Task<User> GetUserById(int id);

        public Task Subscribe(int userId, int subscritptionId);

        public Task Unsubscribe(int userId, int subscriptionId);

        public Task<List<int>> GetSubscriptionsIds(int userId);

        public Task GiveUserXp(int userId, int xpCnt);

        public Task<List<User>> GetUsersByUsername(string userName, int page, int pageSize = 10);

        public Task<User[]> GetRecommendation(int userId, int cnt = 10);

        public Task<bool> HasUserWithId(int userId);

        public Task<Dictionary<int, List<UserLeaderboardUpdate>>> GetUserGroupedByLeagueId();

        public Task UpdateUsersLeaderboards(Dictionary<int, List<UserLeaderboardUpdate>> usersByLeagues);

        public Task UpdateUsersLeagues(Dictionary<int, List<UserLeagueUpdate>> userByLeaderboards);

        public Task<IEnumerable<User>> GetUserLeaderboard(int userId);

        public Task<List<User>> AddBots(List<User> bots);

        public Task<Dictionary<int, List<UserLeagueUpdate>>> GetUserGroupedByLeaderboard();

        public Task<List<UserLeagueUpdate>> GetUsersWithoutLeaderboard();
    }
}