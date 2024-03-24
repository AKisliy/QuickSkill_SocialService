namespace SocialService.DataAccess;

public class CommentEntity
{
    public int Id { get; set; }

    public int LectureId { get; set; }

    public int UserId { get; set; }

    public int Likes { get; set; }

    public DateOnly PublishedOn { get; set; }

    public DateOnly? EditedOn { get; set; }

    public virtual Lecture Lecture { get; set; } = null!;

    public virtual UserEntity User { get; set; } = null!;
}
