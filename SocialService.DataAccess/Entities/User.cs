using System;
using System.Collections.Generic;

namespace SocialService.DataAccess;

public partial class User
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? Status { get; set; }

    public string Username { get; set; } = null!;

    public int LeaderboardId { get; set; }

    public int? Xp { get; set; }

    public int? WeeklyXp { get; set; }

    public int? Subscribers { get; set; }

    public int? Subscriptions { get; set; }

    public virtual ICollection<AnswerEntity> Answers { get; set; } = new List<AnswerEntity>();

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Discussion> Discussions { get; set; } = new List<Discussion>();

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual Leaderboard Leaderboard { get; set; } = null!;
}
