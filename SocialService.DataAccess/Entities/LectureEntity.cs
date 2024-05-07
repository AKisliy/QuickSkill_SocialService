namespace SocialService.DataAccess;

public class LectureEntity
{
    public int Id { get; set; }

    public virtual ICollection<CommentEntity> Comments { get; set; } = new List<CommentEntity>();
}
