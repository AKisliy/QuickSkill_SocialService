using SocialService.Core.Enums;
using SocialService.Core.Models;

namespace SocialService.Core.Interfaces.Services
{
    public interface IDiscussionService
    {
        public Task<int> CreateDiscussion(string title, string body, int userId);

        public Task DeleteDiscussion(int id);

        public IEnumerable<Discussion> GetDiscussions(int page, int pageSize, OrderByOptions orderByOptions);

        public Task<Discussion> GetDiscussion(int id);
    }
}