using MassTransit;
using SocialService.Infrastructure.Consumers;
using SocialService.Infrastructure.EventHandlers;
using SocialService.Infrastructure.Options;

namespace SocialService.WebApi.Extensions
{
    public static class EventBusExtension
    {
        public static void AddMassTransitWithRabbitMQ(this IServiceCollection services, IConfiguration configuration)
        {
            var options = new RabbitMQOptions();
            configuration.GetSection(nameof(RabbitMQOptions)).Bind(options);
            services.AddMassTransit(x =>
            {
                x.AddConsumer<UserCreatedConsumer>();
                x.AddConsumer<UserChangedConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(options.Host,h =>
                    {
                        h.Username(options.Username);
                        h.Password(options.Password);
                    });

                    cfg.ReceiveEndpoint("UserCreatedQueue", e => e.Consumer<UserCreatedConsumer>(context));
                    cfg.ReceiveEndpoint("UserChangedQueue", e => e.Consumer<UserChangedConsumer>(context));
                });
            });
        }
    }
}