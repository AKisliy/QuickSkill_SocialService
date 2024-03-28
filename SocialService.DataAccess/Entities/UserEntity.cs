using SocialService.Core.Models;

namespace SocialService.DataAccess;

public class UserEntity
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string Username { get; set; } = null!;

    public int? LeaderboardId { get; set; }

    public int LeagueId { get; set; }

    public int Xp { get; set; }

    public int WeeklyXp { get; set; }

    public int Subscribers { get; set; }

    public int Subscriptions { get; set; }

    public string? Photo { get; set; }

    public bool IsBot { get; set; }

    public virtual ICollection<AnswerEntity> Answers { get; set; } = new List<AnswerEntity>();

    public virtual ICollection<CommentEntity> Comments { get; set; } = new List<CommentEntity>();

    public virtual ICollection<DiscussionEntity> Discussions { get; set; } = new List<DiscussionEntity>();

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual LeagueEntity? League { get; set; }
}
