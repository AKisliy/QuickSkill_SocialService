using SocialService.Core.Enums;
using SocialService.Core.Models;

namespace SocialService.Core.Interfaces.Services
{
    public interface ICommentService
    {
        public Task<int> CreateComment(int userId, int lectureId, string body);

        public Task DeleteComment(int commentId);

        public Task<IEnumerable<Comment>> GetPageOfComments(int lectureId, int page, int pageSize, OrderByOptions options);

        public Task EditComment(int id, string newBody);
    }
}