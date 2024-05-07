using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SocialService.Core;
using SocialService.Core.Enums;
using SocialService.Core.Interfaces;
using SocialService.Core.Interfaces.Repositories;
using SocialService.Core.Models;
using SocialService.DataAccess.Extensions;

namespace SocialService.DataAccess.Repository
{
    public class CommentRepository: ICommentRepository
    {
        private readonly SocialServiceContext _context;
        private readonly IMapper _mapper;
        private readonly ILectureRepository _lectureRepository;
        private readonly IUserRepository _userRepository;

        public CommentRepository(SocialServiceContext context, ILectureRepository lectureRepository, IUserRepository userRepository, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _lectureRepository = lectureRepository;
            _userRepository = userRepository;
        }

        public async Task<int> Create(int userId, int lectureId, string body)
        {
            if(!await _lectureRepository.HasLectureWithId(lectureId))
                throw new NotFoundException($"No lecture with id: {lectureId}");
            if(!await _userRepository.HasUserWithId(userId))
                throw new NotFoundException($"No user with id: {userId}");
            var comment = new CommentEntity
            {
                UserId = userId,
                LectureId = lectureId,
                Body = body
            };
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
            return comment.Id;
        }

        public async Task<Comment> GetAsync(int id)
        {
            var commentEntity = await _context.Comments.FirstOrDefaultAsync(c => c.Id == id) ?? throw new NotFoundException($"No comment with id: {id}");
            return _mapper.Map<Comment>(commentEntity);
        }

        public async Task Delete(int commentId)
        {
            if(!await HasCommentWithId(commentId))
                throw new NotFoundException($"No comment with id: {commentId}");
            await _context.Comments.Where(c => c.Id == commentId).ExecuteDeleteAsync();
            await _context.SaveChangesAsync();
        }

        public async Task IncreaseLikes(int id)
        {
            if(!await HasCommentWithId(id))
                throw new NotFoundException($"No comment with id: {id}");
            await _context.Comments
                .Where(c => c.Id == id)
                .ExecuteUpdateAsync(c => c
                    .SetProperty(c => c.Likes, c => c.Likes + 1));
        }

        public async Task DecreaseLikes(int id)
        {
            if(!await HasCommentWithId(id))
                throw new NotFoundException($"No comment with id: {id}");
            await _context.Comments
                .Where(c => c.Id == id)
                .ExecuteUpdateAsync(c => c  
                    .SetProperty(c => c.Likes, c => c.Likes - 1));
        }

        public async Task Edit(int id, string newBody)
        {
            if(!await HasCommentWithId(id))
                throw new NotFoundException($"No comment with id: {id}");
            await _context.Comments
                .Where(c => c.Id == id)
                .ExecuteUpdateAsync(c => c
                    .SetProperty(c => c.Body, newBody)
                    .SetProperty(c => c.EditedOn, DateTime.UtcNow));
        }

        public async Task<IEnumerable<Comment>> GetCommentsPage(int lectureId, int page, int size, OrderByOptions options = OrderByOptions.ByDate)
        {
            if(!await _lectureRepository.HasLectureWithId(lectureId))
                throw new NotFoundException($"No lecture with id: {lectureId}");
            return _context.Comments
                    .AsNoTracking()
                    .Where(c => c.LectureId == lectureId)
                    .OrderCommentsBy(options)
                    .Page(page, size)
                    .Include(c => c.User)
                    .Select(c => _mapper.Map<Comment>(c));
        }

        private async Task<bool> HasCommentWithId(int id)
        {
            return await _context.Comments.AnyAsync(c => c.Id == id);
        }
    }
}