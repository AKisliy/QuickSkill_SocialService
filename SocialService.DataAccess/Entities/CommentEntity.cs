namespace SocialService.DataAccess;

public class CommentEntity
{
    public int Id { get; set; }

    public int LectureId { get; set; }

    public int UserId { get; set; }

    public int Likes { get; set; }

    public string Body { get; set; } = null!;

    public DateTime PublishedOn { get; set; }

    public DateTime? EditedOn { get; set; }

    public virtual LectureEntity Lecture { get; set; } = null!;

    public virtual UserEntity User { get; set; } = null!;
}
