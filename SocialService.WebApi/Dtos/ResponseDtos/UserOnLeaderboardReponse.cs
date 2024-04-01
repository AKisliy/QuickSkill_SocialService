namespace SocialService.WebApi.Dtos.ResponseDtos
{
    public class UserOnLeaderboardReponse
    {
        public string Username { get; set; } = null!;

        public int WeeklyXp { get; set; }

        public string? Photo { get; set; }
    }
}