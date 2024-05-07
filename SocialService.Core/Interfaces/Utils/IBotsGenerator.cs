using SocialService.Core.Models;

namespace SocialService.Core.Interfaces.Utils
{
    public interface IBotsGenerator
    {
        public User GenerateBot();

        public List<User> GenerateBots(int cnt);
    }
}