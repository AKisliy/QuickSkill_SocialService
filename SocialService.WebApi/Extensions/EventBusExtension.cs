using EventBus.Base.Standard;
using EventBus.RabbitMQ.Standard;
using MassTransit;
using RabbitMQ.Client;
using SocialService.Infrastructure;
using SocialService.Infrastructure.EventHandlers;

namespace SocialService.WebApi.Extensions
{
    public static class EventBusExtension
    {
        public static void AddMassTransitWithRabbitMQ(this IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<UserCreatedConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("rabbitmq://localhost");

                    cfg.ReceiveEndpoint("UserCreatedQueue", e => e.Consumer<UserCreatedConsumer>(context));
                });
            });
        }
    }
}