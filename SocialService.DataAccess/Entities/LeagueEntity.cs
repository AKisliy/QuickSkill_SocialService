namespace SocialService.DataAccess;

public class LeagueEntity
{
    public int Id { get; set; }

    public string LeagueName { get; set; } = null!;

    public string Photo { get; set; } = null!;

    public int HierarchyPlace { get; set; }

    public virtual ICollection<UserEntity> Users { get; set; } = new List<UserEntity>();
}
