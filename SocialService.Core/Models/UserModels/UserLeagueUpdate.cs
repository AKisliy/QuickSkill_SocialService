namespace SocialService.Core.Models.UserModels
{
    public class UserLeagueUpdate
    {
        public int Id { get; set; }

        public int? LeaderboardId { get; set; }

        public int WeeklyXp { get; set; }

        public int LeagueId { get; set; }

        public bool IsBot { get; set; }
    }
}