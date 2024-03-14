using System;
using System.Collections.Generic;

namespace SocialService.DataAccess;

public partial class Leaderboard
{
    public int Id { get; set; }

    public int LeagueId { get; set; }

    public virtual League League { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
