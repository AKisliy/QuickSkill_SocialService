using SocialService.Core.Models;

namespace SocialService.Core.Interfaces.Repositories
{
    public interface ILeagueRepository
    {
        public Task<int> Create(League league);
        
        public Task<League> GetLeagueByName(string name);

        public Task Delete(string leagueName);

        public Task Update(League league);

        public Task SwapLeaguesPlaces(string l1, string l2);
    }
}