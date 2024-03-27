using SocialService.Core;
using SocialService.Core.Exceptions;
using SocialService.Core.Interfaces.Repositories;
using SocialService.Core.Interfaces.Services;
using SocialService.Core.Models;

namespace SocialService.Application.Services
{
    public class LeagueService: ILeagueService
    {
        private readonly ILeagueRepository _repository;

        public LeagueService(ILeagueRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> CreateLeague(string name, string photo, int hierarchyPlace)
        {
            League league = new() { LeagueName = name, Photo = photo, HierarchyPlace = hierarchyPlace};
            return await _repository.Create(league);
        }

        public IEnumerable<League> GetLeagues()
        {
            return _repository.GetLeagues();
        }

        public async Task UpdateLeague(int id, string name, string photo, int hierarchyPlace)
        {
            League league = await _repository.GetLeagueByName(name);
            if(league.Id != id)
                throw new ConflictException($"Name {name} is already taken");
            League l = new() { Id = id, LeagueName = name, Photo = photo, HierarchyPlace = hierarchyPlace};
            await _repository.Update(l);
        }

        public async Task DeleteLeague(string name)
        {
            await _repository.Delete(name);
        }

        public async Task SwapLeaguePlaces(string l1, string l2)
        {
            if(!await _repository.HasLeagueWithName(l1) || !await _repository.HasLeagueWithName(l2))
                throw new NotFoundException("Leagues with these name not found");
            await _repository.SwapLeaguesPlaces(l1, l2);
        }
    }
}