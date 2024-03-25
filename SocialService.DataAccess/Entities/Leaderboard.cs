namespace SocialService.DataAccess;

public class Leaderboard
{
    public int Id { get; set; }

    public int LeagueId { get; set; }

    public virtual LeagueEntity League { get; set; } = null!;

    public virtual ICollection<UserEntity> Users { get; set; } = new List<UserEntity>();
}
