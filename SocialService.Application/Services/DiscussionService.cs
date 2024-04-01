using SocialService.Core.Enums;
using SocialService.Core.Exceptions;
using SocialService.Core.Interfaces.Repositories;
using SocialService.Core.Interfaces.Services;
using SocialService.Core.Models;

namespace SocialService.Application.Services
{
    public class DiscussionService: IDiscussionService
    {
        private readonly IDiscussionRepository _discussionRepository;
        private readonly IUserRepository _userRepository;

        public DiscussionService(IDiscussionRepository discussionRepository, IUserRepository userRepository)
        {
            _discussionRepository = discussionRepository;
            _userRepository = userRepository;
        }

        public async Task<int> CreateDiscussion(string title, string body, int userId)
        {
            var user = await _userRepository.GetUserById(userId);
            return await _discussionRepository.CreateDiscussion(title, body, user);
        }

        public async Task DeleteDiscussion(int id)
        {
            await _discussionRepository.DeleteDiscussion(id);
        }

        public IEnumerable<Discussion> GetDiscussions(int page, int pageSize, OrderByOptions orderByOptions)
        {
            if(page < 0 || pageSize <= 0)
                throw new BadRequestException("Page should be greater or equal to 0. Page size should be positive");
            return _discussionRepository.GetDiscussionsPage(page, pageSize, orderByOptions);
        }
    }
}