namespace SocialService.Core.Models.UserModels
{
    public class UserLeaderboardUpdate
    {
        public int Id { get; set; }

        public int LeagueId { get; set; }

        public int? LeaderboardId { get; set; }

        public bool IsBot { get; set; }
    }
}