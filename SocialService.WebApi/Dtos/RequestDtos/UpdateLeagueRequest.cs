namespace SocialService.WebApi.Dtos.RequestDtos
{
    public class UpdateLeagueRequest
    {
        public string LeagueName { get; set; } = null!;

        public string Photo { get; set; } = null!;

        public int HierarchyPlace { get; set; }
    }
}