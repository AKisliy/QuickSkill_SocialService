using SocialService.Core.Models;

namespace SocialService.Core.Interfaces.Services
{
    public interface IUserService
    {
        public Task<List<User>> SearchUserByUsername(int id, string username, int page, int pageSize);

        public Task<List<int>> GetUserSubscriptionsId(int userId);

        public Task AddUser(int id, string firstName, string lastName, string userName);
    }
}