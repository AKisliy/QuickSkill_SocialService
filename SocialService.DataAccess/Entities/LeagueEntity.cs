namespace SocialService.DataAccess;

public class LeagueEntity
{
    public int Id { get; set; }

    public string LeagueName { get; set; } = null!;

    public string Photo { get; set; } = null!;

    public int HierarchyPlace { get; set; }

    // public virtual ICollection<Leaderboard> Leaderboards { get; set; } = new List<Leaderboard>();
}
