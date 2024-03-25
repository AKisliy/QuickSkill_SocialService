using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SocialService.Core;
using SocialService.Core.Interfaces.Repositories;
using SocialService.Core.Models;

namespace SocialService.DataAccess.Repository
{
    public class LeagueRepository: ILeagueRepository
    {
        private readonly SocialServiceContext _context;
        private readonly IMapper _mapper;

        public LeagueRepository(SocialServiceContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Create(League league)
        {
            var leagueEntity = _mapper.Map<LeagueEntity>(league);
            await _context.Leagues.AddAsync(leagueEntity);
            await _context.SaveChangesAsync();
            return leagueEntity.Id;
        }

        public async Task<League> GetLeagueByName(string name)
        {
            var league = await _context.Leagues
                                .AsNoTracking()
                                .FirstOrDefaultAsync(l => l.LeagueName == name) ?? throw new NotFoundException($"No league with name: {name}");
            return _mapper.Map<League>(league);
        }

        public async Task Delete(string leagueName)
        {
            await _context.Leagues.Where(l => l.LeagueName == leagueName).ExecuteDeleteAsync();
            await _context.SaveChangesAsync();
        }

        public async Task Update(League league)
        {
            if(!await HasLeagueWithId(league.Id))
                throw new NotFoundException("League wasn't found");
            await _context.Leagues.Where(l => l.Id == league.Id).ExecuteUpdateAsync(l => l
                .SetProperty(l => l.LeagueName, _ => league.LeagueName)
                .SetProperty(l => l.Photo, _ => league.Photo)
                .SetProperty(l => l.HierarchyPlace, _ => league.HierarchyPlace)
            );
            await _context.SaveChangesAsync();
        }

        public async Task SwapLeaguesPlaces(string l1, string l2)
        {
            var firstLeague = await GetTrackedLeagueByName(l1);
            var secondLeague = await GetTrackedLeagueByName(l2);
            // swap using XOR
            firstLeague.HierarchyPlace ^= secondLeague.HierarchyPlace;
            secondLeague.HierarchyPlace ^= firstLeague.HierarchyPlace;
            firstLeague.HierarchyPlace ^= secondLeague.HierarchyPlace;
            await _context.SaveChangesAsync();
        }

        private async Task<bool> HasLeagueWithId(int id)
        {
            return await _context.Leagues.AnyAsync(l => l.Id == id);
        }

        private async Task<bool> HasLeagueWithName(string name)
        {
            return await _context.Leagues.AnyAsync(l => l.LeagueName == name);
        }

        private async Task<LeagueEntity> GetTrackedLeagueByName(string name)
        {
            return await _context.Leagues.FirstOrDefaultAsync(l => l.LeagueName == name) ?? throw new NotFoundException($"No league with name {name}");
        }
    }
}