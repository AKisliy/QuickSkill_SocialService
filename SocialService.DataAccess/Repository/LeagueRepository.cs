using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SocialService.Core;
using SocialService.Core.Exceptions;
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
            if(await HasLeagueWithName(league.LeagueName))
                throw new ConflictException($"League with name: {league.LeagueName} already exists");
            if(await HasLeagueWithPlace(league.HierarchyPlace))
                throw new ConflictException($"League with hierarchy place: {league.HierarchyPlace} already exists");
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

        public IEnumerable<League> GetLeagues()
        {
            return _context.Leagues
                            .AsNoTracking()
                            .Where(l => l.Id != 0)
                            .OrderBy(l => l.HierarchyPlace)
                            .Select(l => _mapper.Map<League>(l));
        }

        public async Task<List<League>> GetLeaguesAsync()
        {
            return await _context.Leagues
                            .AsNoTracking()
                            .Where(l => l.Id != 0)
                            .OrderBy(l => l.HierarchyPlace)
                            .Select(l => _mapper.Map<League>(l))
                            .ToListAsync();
        }

        public async Task Delete(string leagueName)
        {
            var league = await GetLeagueByName(leagueName);
            int leaguePlace = league.HierarchyPlace;
            var nextLeague = await _context.Leagues.FirstOrDefaultAsync(x => x.HierarchyPlace > leaguePlace);
            // we push all users to the next league
            if(nextLeague != null)
            {
                await _context.Users.Where(u => u.LeagueId == league.Id)
                              .ExecuteUpdateAsync(u => u
                                .SetProperty(u => u.LeagueId, nextLeague.Id));
                await _context.Leagues.Where(l => l.LeagueName == leagueName).ExecuteDeleteAsync();
                return;
            }
            // if no next league - drop to previous league
            var prevLeague = await _context.Leagues.FirstOrDefaultAsync(x => x.HierarchyPlace < leaguePlace);
            if(prevLeague != null)
            {
                await _context.Users.Where(u => u.LeagueId == league.Id)
                                .ExecuteUpdateAsync(u => u
                                  .SetProperty(u => u.LeagueId, prevLeague.Id));
                await _context.Leagues.Where(l => l.LeagueName == leagueName).ExecuteDeleteAsync();
            }
            else
            {
                throw new ConflictException($"{leagueName} is the only league. Don't know how to delete it");
            }
        }

        public async Task Update(League league)
        {
            if(!await HasLeagueWithId(league.Id))
                throw new NotFoundException("League wasn't found");
            bool hasLeagueWithPlace = await _context.Leagues.Where(l => l.Id != league.Id).AnyAsync(l => l.HierarchyPlace == league.HierarchyPlace);
            if(hasLeagueWithPlace)
                throw new ConflictException($"Place {league.HierarchyPlace} is taken");
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

        public async Task<bool> HasLeagueWithName(string name)
        {
            return await _context.Leagues.AnyAsync(l => l.LeagueName == name);
        }

        public async Task<bool> HasLeagueWithPlace(int place)
        {
            return await _context.Leagues.AnyAsync(l => l.HierarchyPlace == place);
        }

        private async Task<LeagueEntity> GetTrackedLeagueByName(string name)
        {
            return await _context.Leagues.FirstOrDefaultAsync(l => l.LeagueName == name) ?? throw new NotFoundException($"No league with name {name}");
        }
    }
}