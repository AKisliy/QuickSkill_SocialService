using SocialService.Core.Models;
using SocialService.Core.Models.UserModels;

namespace SocialService.Core.Interfaces
{
    public interface IUserRepository
    {
        public Task AddUser(User user);

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

        public Task<Dictionary<int, List<UserLeaderboardUpdate>>> GetUserGroupedByLeague();

        public Task UpdateUsersLeaderboards(Dictionary<int, List<UserLeaderboardUpdate>> usersByLeagues);

        public Task<IEnumerable<User>> GetUserLeaderboard(int userId);
    }
}