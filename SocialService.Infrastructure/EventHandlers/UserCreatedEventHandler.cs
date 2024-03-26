using EventBus.Base.Standard;
using SocialService.Core.Interfaces;
using SocialService.Core.Models;

namespace SocialService.Infrastructure.EventHandlers
{
    public class UserCreatedEventHandler : IIntegrationEventHandler<UserCreatedEvent>
    {
        private readonly IUserRepository _repository;

        public UserCreatedEventHandler(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(UserCreatedEvent @event)
        {
            var user = new User
            {
                Id = @event.UserId,
                FirstName = @event.FirstName,
                LastName = @event.LastName,
                Username = @event.Username,
                Status = @event.Status
            };
            await _repository.AddUser(user);
        }
    }
}