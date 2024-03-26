namespace SocialService.Core.Interfaces.Services
{
    public interface ILeaderboardService
    {
        public Task CreateWeeklyLeaderboards();

        public Task UpdateLeagues();
    }
}