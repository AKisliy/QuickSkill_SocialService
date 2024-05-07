using System.Diagnostics.CodeAnalysis;
using Hangfire;
using Hangfire.Dashboard;
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
            services.AddScoped<ILeaderboardService, LeaderboardService>();
        }

        public static IApplicationBuilder ConfigureHangfireTasks(this IApplicationBuilder app)
        {
            RecurringJob.AddOrUpdate<LeaderboardService>("create-leaderboards", x => x.CreateWeeklyLeaderboards(), "01 3 * * 1");
            RecurringJob.AddOrUpdate<LeaderboardService>("update-leagues", x => x.UpdateLeagues(), "00 3 * * 1");
            return app;
        }
    }
}