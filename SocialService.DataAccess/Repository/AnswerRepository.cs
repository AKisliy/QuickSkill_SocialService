using Microsoft.EntityFrameworkCore;
using SocialService.Core;
using SocialService.Core.Enums;
using SocialService.Core.Interfaces.Repositories;

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

        public async Task<IEnumerable<Answer>> GetAllAnswersForDiscussion(int id)
        {
            var hasDiscussion = await _discussionRepository.HasDiscussionWithId(id);
            if(!hasDiscussion)
                throw new NotFoundException($"No discussion with this id: {id}");
            return _context.Answers
                .AsNoTracking()
                .Where(a => a.Id == id);
        }

        public async Task<IEnumerable<Answer>> GetAnswersPage(int discussionId, int page, int size, OrderByOptions orderBy = OrderByOptions.SimpleOrder)
        {
            var hasDiscussion = await _discussionRepository.HasDiscussionWithId(discussionId);
            if(!hasDiscussion)
                throw new NotFoundException($"No discussion with this id: {discussionId}");
            return null;
        }
        
    }
}