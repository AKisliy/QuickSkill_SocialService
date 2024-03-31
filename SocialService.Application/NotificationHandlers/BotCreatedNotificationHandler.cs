using AutoMapper;
using MassTransit;
using MediatR;
using SocialService.Core.Notifications;
using Shared;

namespace SocialService.Application.NotificationHandlers
{
    public class BotCreatedNotificationHandler : INotificationHandler<BotCreatedNotification>
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IMapper _mapper;

        public BotCreatedNotificationHandler(IPublishEndpoint publishEndpoint, IMapper mapper)
        {
            _publishEndpoint = publishEndpoint;
            _mapper = mapper;
        }

        public async Task Handle(BotCreatedNotification notification, CancellationToken cancellationToken)
        {
            var newBots = notification.bots.Select(b => _mapper.Map<Bot>(b)).ToList();
            await _publishEndpoint.Publish(new BotsCreatedEvent{ Bots = newBots }, cancellationToken);
            await Task.CompletedTask;
        }
    }
}