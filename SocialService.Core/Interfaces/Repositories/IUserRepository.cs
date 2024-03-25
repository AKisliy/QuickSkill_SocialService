using SocialService.Core.Models;

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

        public Task GiveUserXp(int userId, int xpCnt);

        public IEnumerable<User> GetUsersByUsername(string userName);

        public Task<User[]> GetRecommendation(int userId, int cnt = 10);
    }
}