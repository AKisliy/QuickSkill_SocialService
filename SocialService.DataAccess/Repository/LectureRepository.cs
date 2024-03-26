using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SocialService.Core.Interfaces.Repositories;

namespace SocialService.DataAccess.Repository
{
    public class LectureRepository: ILectureRepository
    {
        private readonly SocialServiceContext _context;

        public LectureRepository(SocialServiceContext context)
        {
            _context = context;
        }

        public async Task Add(int id)
        {
            if(!await HasLectureWithId(id))
            {
                await _context.Lectures.AddAsync(new LectureEntity{ Id = id });
                await _context.SaveChangesAsync();
            }
        }

        public async Task Delete(int id)
        {
            if(await HasLectureWithId(id))
            {
                await _context.Lectures.Where(l => l.Id == id).ExecuteDeleteAsync();
            }
        }

        public async Task<bool> HasLectureWithId(int id)
        {
            return await _context.Lectures.AnyAsync(l => l.Id == id);
        }
    }
}