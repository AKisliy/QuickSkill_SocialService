using SocialService.Core.Enums;
using SocialService.Core.Models;

namespace SocialService.Core.Interfaces.Repositories
{
    public interface IAnswerRepository
    {
        public Task CreateAnswer(int userId, int discussionId, string body);

        public Task DeleteAnswer(int id);

        public Task<IEnumerable<Answer>> GetAllAnswersForDiscussion(int id, OrderByOptions orderBy = OrderByOptions.ByDate);

        public Task<IEnumerable<Answer>> GetAnswersPage(int discussionId, int page, int size, OrderByOptions orderBy = OrderByOptions.ByDate);

        public Task IncreaseAnswerLikes(int answerId);

        public Task DecreaseAnswerLikes(int answerId);

        public Task EditAnswer(int answerId, string newBody);
    }
}