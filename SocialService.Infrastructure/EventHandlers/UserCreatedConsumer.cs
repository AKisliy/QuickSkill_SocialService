using MassTransit;
using Shared;
using SocialService.Core.Interfaces;

namespace SocialService.Infrastructure.EventHandlers
{
    public class UserCreatedConsumer: IConsumer<UserCreatedEvent>
    {
        private IUserRepository _userRepository;

        public UserCreatedConsumer(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task Consume(ConsumeContext<UserCreatedEvent> context)
        {
            // Обработка сообщения
            Console.WriteLine($"User Created: {context.Message.Username}");
            await Task.CompletedTask;
        }
    }
}