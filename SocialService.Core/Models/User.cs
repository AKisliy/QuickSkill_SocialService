namespace SocialService.Core.Models
{
    public class User
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Status { get; set; }

        public string Username { get; set; } = null!;

        public int? LeaderboardId { get; set; }

        public int LeagueId { get; set; }

        public int Xp { get; set; }

        public int WeeklyXp { get; set; }

        public int Subscribers { get; set; }

        public int Subscriptions { get; set; }

        public string? Photo { get; set; }

        public List<int> SubscritptionIds { get; set; } = [];
    }
}
