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

        public async Task<int> CreateAnswer(int userId, int discussionId, string body)
        {
            var hasDiscussion = await _discussionRepository.HasDiscussionWithId(discussionId);
            if(!hasDiscussion)
                throw new NotFoundException($"No discussion with thid id: {discussionId}");
            var discussion = await _discussionRepository.GetDiscussonById(discussionId);
            var answer = new AnswerEntity
            {
                UserId = userId,
                DiscussionId = discussionId,
                Body = body
            };
            discussion.AnswersCount++;
            await _discussionRepository.Update(discussion);
            await _context.Answers.AddAsync(answer);
            await _context.SaveChangesAsync();
            return answer.Id;
        }

        public async Task DeleteAnswer(int id)
        {
            if(!await HasAnswerWithId(id))
                throw new NotFoundException($"No answer with id: {id}");
            var answer = _context.Answers.Where(a => a.Id == id).Single();
            var discussion = await _discussionRepository.GetDiscussonById(answer.DiscussionId);
            discussion.AnswersCount--;
            await _discussionRepository.Update(discussion);
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
                .OrderAnswersBy(orderBy)
                .Page(page, size)
                .Select(a => _mapper.Map<Answer>(a));
        }

        public async Task IncreaseAnswerLikes(int answerId)
        {
            await _context.Answers
                    .Where(a => a.Id == answerId)
                    .ExecuteUpdateAsync(a => a
                        .SetProperty(a => a.Likes, a => a.Likes + 1));
        }

        public async Task DecreaseAnswerLikes(int answerId)
        {
            await _context.Answers
                    .Where(a => a.Id == answerId)
                    .ExecuteUpdateAsync(a => a
                        .SetProperty(a => a.Likes, a => a.Likes == 0 ? 0 : a.Likes - 1));
        }

        public async Task EditAnswer(int answerId, string newBody)
        {
            var answer = await GetTrackedAnswerById(answerId);
            answer.Body = newBody;
            answer.EditedOn = DateTime.UtcNow.AddHours(3);
            await _context.SaveChangesAsync();
        }

        public async Task<Answer> GetAnswerById(int id)
        {
            var entity =  await _context.Answers
                            .AsNoTracking()
                            .FirstOrDefaultAsync(a => a.Id == id) ?? throw new NotFoundException($"No answer with id: {id}");
            return _mapper.Map<Answer>(entity);
        }

        private async Task<AnswerEntity> GetTrackedAnswerById(int id)
        {
            return await _context.Answers.FirstOrDefaultAsync(a => a.Id == id) ?? throw new NotFoundException($"No answer with id {id}");
        }

        private async Task<bool> HasAnswerWithId(int id)
        {
            return await _context.Answers.AnyAsync(a => a.Id == id);
        }
    }
}