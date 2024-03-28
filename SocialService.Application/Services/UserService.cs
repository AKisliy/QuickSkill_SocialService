using SocialService.Core.Exceptions;
using SocialService.Core.Interfaces;
using SocialService.Core.Interfaces.Services;
using SocialService.Core.Models;

namespace SocialService.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        // just for testing 
        public async Task AddUser(int id, string firstName, string lastName, string userName)
        {
            User user = new User{ FirstName = firstName, LastName = lastName, Username = userName};
            await _userRepository.AddUser(user);
        }

        public async Task<List<User>> SearchUserByUsername(int id, string username, int page, int pageSize)
        {
            if(string.IsNullOrEmpty(username))
                throw new BadRequestException("Incorrect string input");
            if(page < 0 || pageSize < 0)
                throw new BadRequestException("Page number and page size can't be negative");
            var list = await _userRepository.GetUsersByUsername(username, page, pageSize);
            var user = list.Find(u => u.Id == id);
            if(user != null)
                list.Remove(user);
            return list;
        }

        public async Task SubcribeOnUser(int id, int subId)
        {
            await _userRepository.Subscribe(id, subId);
        }

        public async Task Unsubscribe(int id, int subId)
        {
            await _userRepository.Unsubscribe(id, subId);
        }

        public async Task<User[]> GetRecommendationForUser(int id, int pageSize)
        {
            return await _userRepository.GetRecommendation(id, pageSize);
        }

        public async Task<List<int>> GetUserSubscriptionsId(int userId)
        {
            return await _userRepository.GetSubscriptionsIds(userId);
        }

        public async Task<IEnumerable<User>> GetUserLeaderboard(int userId)
        {
            return await _userRepository.GetUserLeaderboard(userId);
        }
    }
}