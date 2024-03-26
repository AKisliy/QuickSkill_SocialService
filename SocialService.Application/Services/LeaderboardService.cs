using SocialService.Core.Interfaces;
using SocialService.Core.Interfaces.Services;

namespace SocialService.Application.Services
{
    public class LeaderboardService: ILeaderboardService
    {
        private readonly IUserRepository _userRepository;

        public LeaderboardService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task CreateWeeklyLeaderboards()
        {
            // Логика выбора пользователей и создания лидербордов(начало недели)
        }

        public async Task UpdateLeagues()
        {
            // Логика обновления лиг и обнуления weeklyXp(в конце недели)
        }
    }
}