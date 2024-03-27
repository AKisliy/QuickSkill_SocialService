using SocialService.Core.Models;

namespace SocialService.Core.Interfaces.Services
{
    public interface ILeagueService
    {
        public Task<int> CreateLeague(string name, string photo, int hierarchyPlace);

        public IEnumerable<League> GetLeagues();

        public Task UpdateLeague(int id, string name, string photo, int hierarchyPlace);

        public Task DeleteLeague(string name);

        public Task SwapLeaguePlaces(string l1, string l2);
    }
}