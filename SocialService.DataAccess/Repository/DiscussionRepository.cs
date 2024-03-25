using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SocialService.Core;
using SocialService.Core.Enums;
using SocialService.Core.Interfaces.Repositories;
using SocialService.Core.Models;
using SocialService.DataAccess.Extensions;

namespace SocialService.DataAccess.Repository
{
    public class DiscussionRepository : IDiscussionRepository
    {
        private readonly SocialServiceContext _context;
        private readonly IMapper _mapper;

        public DiscussionRepository(SocialServiceContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> CreateDiscussion(string title, string body, User author)
        {
            var discussion = new DiscussionEntity
            {
                Title = title,
                Body = body,
                AuthorId = author.Id
            };
            await _context.Discussions.AddAsync(discussion);
            await _context.SaveChangesAsync();
            return discussion.Id;
        }

        public async Task<Discussion> GetDiscussonById(int id)
        {
            var discussion = await _context.Discussions
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(d => d.Id == id) ?? throw new NotFoundException($"No discusson with id: {id}");
            return _mapper.Map<Discussion>(discussion);
        }

        public async Task DeleteDiscussion(int discussionId)
        {
            if(!await HasDiscussionWithId(discussionId))
                throw new NotFoundException($"No discussion with id: {discussionId}");
            await _context.Discussions.Where(d => d.Id == discussionId).ExecuteDeleteAsync();
            await _context.SaveChangesAsync();
        }

        public IEnumerable<Discussion> GetAllDiscussions(OrderByOptions orderBy = OrderByOptions.ByDate)
        {
            return _context.Discussions
                    .AsNoTracking()
                    .OrderDiscussionsBy(orderBy)
                    .Select(d => _mapper.Map<Discussion>(d));
        }

        public IEnumerable<Discussion> GetDiscussionsPage(int page, int size, OrderByOptions orderBy = OrderByOptions.ByDate)
        {
            return _context.Discussions
                    .AsNoTracking()
                    .Page(page, size)
                    .OrderDiscussionsBy(orderBy)
                    .Select(d => _mapper.Map<Discussion>(d));
        }

        public async Task IncreaseDiscussionLikes(int discussionId)
        {
            var discussion = await GetTrackedDisscussionById(discussionId);
            ++discussion.Likes;
            await _context.SaveChangesAsync();
        }

        public async Task DecreaseDiscussionLikes(int discussionId)
        {
            var discussion = await GetTrackedDisscussionById(discussionId);
            if(discussion.Likes == 0)
                return;
            --discussion.Likes;
            await _context.SaveChangesAsync();
        }

        private async Task<DiscussionEntity> GetTrackedDisscussionById(int id)
        {
            return await _context.Discussions.FirstOrDefaultAsync(d => d.Id == id) ??
                    throw new NotFoundException($"No discussion with id: {id}");
        }

        public async Task<bool> HasDiscussionWithId(int id)
        {
            return await _context.Discussions.AnyAsync(d => d.Id == id);
        }
    }
}