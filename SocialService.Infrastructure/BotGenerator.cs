using SocialService.Core.Interfaces.Utils;
using SocialService.Core.Models;

namespace SocialService.Infrastructure
{
    public class BotsGenerator : IBotsGenerator
    {
        public User GenerateBot()
        {
            var firstname = Faker.Name.First();
            var lastname = Faker.Name.Last();
            var username = Faker.Internet.UserName();
            return new User{ FirstName = firstname, LastName = lastname, Username = username};
        }

        public List<User> GenerateBots(int cnt)
        {
            List<User> result = [];
            for(int i = 0; i < cnt; ++i)
                result.Add(GenerateBot());
            return result;
        }
    }
}