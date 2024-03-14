using Microsoft.EntityFrameworkCore;
using SocialService.Core.Interfaces.Repositories;

namespace SocialService.DataAccess.Repository
{
    public class DiscussionRepository : IDiscussionRepository
    {
        private SocialServiceContext _context;

        public DiscussionRepository(SocialServiceContext context)
        {
            _context = context;
        }

        public async Task<bool> HasDiscussionWithId(int id)
        {
            return await _context.Discussions.AnyAsync(d => d.Id == id);
        }
    }
}