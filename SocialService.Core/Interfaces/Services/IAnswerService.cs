using SocialService.Core.Enums;
using SocialService.Core.Models;

namespace SocialService.Core.Interfaces.Services
{
    public interface IAnswerService
    {
        public Task<int> CreateAnswer(int userId, int discussionId, string body);

        public Task<Answer> GetAnswerById(int id);

        public Task DeleteAnswer(int id);

        public Task EditAnswer(int id, string newBody);

        public Task<IEnumerable<Answer>> GetAnswersPage(int discussionId, int page, int pageSize, OrderByOptions orderBy);
    }
}