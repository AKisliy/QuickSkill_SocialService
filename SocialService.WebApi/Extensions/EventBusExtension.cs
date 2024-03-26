using EventBus.Base.Standard;
using EventBus.RabbitMQ.Standard;
using SocialService.Infrastructure;
using SocialService.Infrastructure.EventHandlers;

namespace SocialService.WebApi.Extensions
{
    public static class EventBusExtension
    {
        public static void AddEventBus(this IServiceCollection services)
        {
            services.AddScoped<IEventBus, EventBusRabbitMq>();
            services.AddTransient<IIntegrationEventHandler<UserCreatedEvent>, UserCreatedEventHandler>();
        }

        public static IApplicationBuilder ConfigureEventBus(this IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subscribe<UserCreatedEvent, UserCreatedEventHandler>();
            return app;
        }
    }
}