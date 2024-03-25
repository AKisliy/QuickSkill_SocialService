namespace SocialService.Core.Models
{
    public class League
    {
        public int Id { get; set; }

        public string LeagueName { get; set; } = null!;

        public string Photo { get; set; } = null!;

        public int HierarchyPlace { get; set; }
    }
}