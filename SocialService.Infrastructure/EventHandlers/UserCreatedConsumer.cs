using MassTransit;

namespace SocialService.Infrastructure.EventHandlers
{
    public class UserCreatedConsumer: IConsumer<UserCreatedEvent>
    {
        public async Task Consume(ConsumeContext<UserCreatedEvent> context)
        {
            // Обработка сообщения
            Console.WriteLine($"User Created: {context.Message.Username}");
            await Task.CompletedTask;
        }
    }
}