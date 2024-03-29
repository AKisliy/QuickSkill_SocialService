using MassTransit;
using Shared;
using SocialService.Core.Interfaces;
using SocialService.Core.Models;

namespace SocialService.Infrastructure.EventHandlers
{
    public class UserCreatedConsumer: IConsumer<UserCreatedEvent>
    {
        private readonly IUserRepository _userRepository;

        public UserCreatedConsumer(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task Consume(ConsumeContext<UserCreatedEvent> context)
        {
            //System.Console.WriteLine("Wow i got it");
            var createdUser = context.Message;
            await _userRepository.AddUser(new User{
                Id = createdUser.UserId,
                FirstName = createdUser.FirstName,
                LastName = createdUser.LastName,
                Username = createdUser.Username,
                Photo = createdUser.Photo,
                Status = createdUser.Status});
            await Task.CompletedTask;
        }
    }
}