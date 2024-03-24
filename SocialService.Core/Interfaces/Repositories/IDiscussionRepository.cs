using SocialService.Core.Enums;
using SocialService.Core.Models;

namespace SocialService.Core.Interfaces.Repositories
{
    public interface IDiscussionRepository
    {
        public Task<int> CreateDiscussion(string title, string body, User author);

        public Task DeleteDiscussion(int discussionId);

        public IEnumerable<Discussion> GetAllDiscussions(OrderByOptions orderBy = OrderByOptions.ByDate);

        public IEnumerable<Discussion> GetDiscussionsPage(int page, int size, OrderByOptions orderBy = OrderByOptions.ByDate);

        public Task IncreaseDiscussionLikes(int discussionId);

        public Task DecreaseDiscussionLikes(int discussionId);

        public Task<bool> HasDiscussionWithId(int id);
    }
}