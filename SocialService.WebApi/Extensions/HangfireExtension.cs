using Hangfire;
using Hangfire.PostgreSql;
using SocialService.Application.Services;
using SocialService.Core.Interfaces.Services;

namespace SocialService.WebApi.Extensions
{
    public static class HangfireExtension
    {
        public static void AddHangfireToApp(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHangfire(config => config.UsePostgreSqlStorage(configuration.GetConnectionString("DefaultConnection")));
            services.AddHangfireServer();
            services.AddSingleton<ILeaderboardService, LeaderboardService>();
        }

        public static IApplicationBuilder ConfigureHangfireTasks(this IApplicationBuilder app)
        {
            RecurringJob.AddOrUpdate<LeaderboardService>("create-leaderboards", x => x.CreateWeeklyLeaderboards(), "0 0 * * 1");
            RecurringJob.AddOrUpdate<LeaderboardService>("update-leagues", x => x.UpdateLeagues(), "59 23 * * 0");
            return app;
        }
    }
}