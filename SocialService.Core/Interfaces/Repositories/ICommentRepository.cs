using SocialService.Core.Enums;
using SocialService.Core.Models;

namespace SocialService.Core.Interfaces.Repositories
{
    public interface ICommentRepository
    {
        public Task<int> Create(int userId, int lectureId, string body);

        public Task<Comment> GetAsync(int id);

        public Task Delete(int commentId);

        public Task IncreaseLikes(int id);

        public Task DecreaseLikes(int id);

        public Task Edit(int id, string newBody);

        public Task<IEnumerable<Comment>> GetCommentsPage(int lectureId, int page, int size, OrderByOptions options = OrderByOptions.ByDate);
    }
}