using MassTransit;
using Shared;
using SocialService.Core.Interfaces.Repositories;

namespace SocialService.Infrastructure.Consumers
{
    public class UserDeletedConsumer : IConsumer<UserDeletedEvent>
    {
        private IUserRepository _repository;

        public UserDeletedConsumer(IUserRepository repository)
        {
            _repository = repository;
        }
        public async Task Consume(ConsumeContext<UserDeletedEvent> context)
        {
            var id = context.Message.Id;
            await _repository.DeleteUser(id);
            await Task.CompletedTask;
        }
    }
}