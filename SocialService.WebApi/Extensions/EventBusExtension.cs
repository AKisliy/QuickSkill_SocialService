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

                    cfg.ReceiveEndpoint("user_created_queue", e =>
                    {
                        e.BindQueue = false;
                        e.ExchangeType = ExchangeType.Fanout;
                        e.Bind("UserCreatedExchange");
                        e.Consumer<UserCreatedConsumer>(context);
                    });
                });
            });
        }
    }
}