namespace SocialService.WebApi.Dtos.ResponseDtos
{
    public class LeaderboardResponse
    {
        public int LeaguePlace { get; set; }
        public IEnumerable<UserOnLeaderboardReponse> Users { get; set; } = [];
    }
}