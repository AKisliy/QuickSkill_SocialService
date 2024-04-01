using SocialService.Core.Enums;
using SocialService.Core.Exceptions;
using SocialService.Core.Interfaces.Repositories;
using SocialService.Core.Interfaces.Services;
using SocialService.Core.Models;

namespace SocialService.Application.Services
{
    public class CommentService: ICommentService
    {
        private readonly ICommentRepository _commentRepository;

        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<int> CreateComment(int userId, int lectureId, string body)
        {
            return await _commentRepository.Create(userId, lectureId, body);
        }

        public async Task DeleteComment(int commentId)
        {
            await _commentRepository.Delete(commentId);
        }

        public async Task<IEnumerable<Comment>> GetPageOfComments(int lectureId, int page, int pageSize, OrderByOptions options)
        {
            if(page < 0 || pageSize <= 0)
                throw new BadRequestException("Page number should be >= 0/page size should be > 0");
            return await _commentRepository.GetCommentsPage(lectureId, page, pageSize, options);
        }

        public async Task EditComment(int id, string newBody)
        {
            var comment = await _commentRepository.GetAsync(id);
            if(comment.Body == newBody)
                return;
            await _commentRepository.Edit(id, newBody);
        }
    }
}