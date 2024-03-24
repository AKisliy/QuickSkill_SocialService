using Microsoft.EntityFrameworkCore;
using SocialService.Core;
using SocialService.Core.Enums;
using SocialService.Core.Interfaces.Repositories;
using SocialService.DataAccess.Extensions;

namespace SocialService.DataAccess.Repository
{
    public class AnswerRepository : IAnswerRepository
    {
        private readonly SocialServiceContext _context;
        private readonly IDiscussionRepository _discussionRepository;

        public AnswerRepository(SocialServiceContext context, IDiscussionRepository discussionRepository)
        {
            _context = context;
            _discussionRepository = discussionRepository;
        }

        public async Task CreateAnswer(int userId, int discussionId, string body)
        {
            var hasDiscussion = await _discussionRepository.HasDiscussionWithId(discussionId);
            if(!hasDiscussion)
                throw new NotFoundException($"No discussion with thid id: {discussionId}");
            await _context.Answers.AddAsync(
                new AnswerEntity
                {
                    UserId = userId,
                    DiscussionId = discussionId,
                    Body = body
                });
        }

        public async Task DeleteAnswer(int id)
        {
            await _context.Answers.Where(a => a.Id == id).ExecuteDeleteAsync();
        }

        public async Task<IEnumerable<AnswerEntity>> GetAllAnswersForDiscussion(int id, OrderByOptions orderBy = OrderByOptions.ByDate)
        {
            var hasDiscussion = await _discussionRepository.HasDiscussionWithId(id);
            if(!hasDiscussion)
                throw new NotFoundException($"No discussion with this id: {id}");
            return _context.Answers
                .AsNoTracking()
                .Where(a => a.Id == id)
                .OrderAnswersBy(orderBy);
        }

        public async Task<IEnumerable<AnswerEntity>> GetAnswersPage(int discussionId, int page, int size, OrderByOptions orderBy = OrderByOptions.ByDate)
        {
            var hasDiscussion = await _discussionRepository.HasDiscussionWithId(discussionId);
            if(!hasDiscussion)
                throw new NotFoundException($"No discussion with this id: {discussionId}");
            return _context.Answers
                .Where(a => a.DiscussionId == discussionId)
                .Page(page, size)
                .OrderAnswersBy(orderBy);
        }
    }
}