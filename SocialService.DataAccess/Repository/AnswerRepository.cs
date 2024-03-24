using Microsoft.EntityFrameworkCore;
using SocialService.Core;
using SocialService.Core.Enums;
using SocialService.Core.Interfaces.Repositories;
using SocialService.Core.Models;
using SocialService.DataAccess.Extensions;
using AutoMapper;

namespace SocialService.DataAccess.Repository
{
    public class AnswerRepository : IAnswerRepository
    {
        private readonly SocialServiceContext _context;
        private readonly IDiscussionRepository _discussionRepository;
        private readonly IMapper _mapper;

        public AnswerRepository(SocialServiceContext context, IDiscussionRepository discussionRepository, IMapper mapper)
        {
            _context = context;
            _discussionRepository = discussionRepository;
            _mapper = mapper;
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
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAnswer(int id)
        {
            if(!await HasAnswerWithId(id))
                throw new NotFoundException($"No answer with id: {id}");
            await _context.Answers.Where(a => a.Id == id).ExecuteDeleteAsync();
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Answer>> GetAllAnswersForDiscussion(int id, OrderByOptions orderBy = OrderByOptions.ByDate)
        {
            var hasDiscussion = await _discussionRepository.HasDiscussionWithId(id);
            if(!hasDiscussion)
                throw new NotFoundException($"No discussion with this id: {id}");
            return _context.Answers
                .AsNoTracking()
                .Where(a => a.Id == id)
                .OrderAnswersBy(orderBy)
                .Select(a => _mapper.Map<Answer>(a));
        }

        public async Task<IEnumerable<Answer>> GetAnswersPage(int discussionId, int page, int size, OrderByOptions orderBy = OrderByOptions.ByDate)
        {
            var hasDiscussion = await _discussionRepository.HasDiscussionWithId(discussionId);
            if(!hasDiscussion)
                throw new NotFoundException($"No discussion with this id: {discussionId}");
            return _context.Answers
                .AsNoTracking()
                .Where(a => a.DiscussionId == discussionId)
                .Page(page, size)
                .OrderAnswersBy(orderBy)
                .Select(a => _mapper.Map<Answer>(a));
        }

        public async Task IncreaseAnswerLikes(int answerId)
        {
            var answer = await GetAnswerById(answerId);
            answer.Likes += 1;
            await _context.SaveChangesAsync();
        }

        public async Task DecreaseAnswerLikes(int answerId)
        {
            var answer = await GetAnswerById(answerId);
            if(answer.Likes == 0)
                return;
            answer.Likes -= 1;
            await _context.SaveChangesAsync();
        }

        public async Task EditAnswer(int answerId, string newBody)
        {
            var answer = await GetAnswerById(answerId);
            answer.Body = newBody;
            await _context.SaveChangesAsync();
        }

        private async Task<AnswerEntity> GetAnswerById(int id)
        {
            return await _context.Answers.FirstOrDefaultAsync(a => a.Id == id) ?? throw new NotFoundException($"No answer with id {id}");
        }

        private async Task<bool> HasAnswerWithId(int id)
        {
            return await _context.Answers.AnyAsync(a => a.Id == id);
        }
    }
}