namespace SocialService.DataAccess;

public class DiscussionEntity
{
    public int Id { get; set; }

    public int AuthorId { get; set; }

    public string Title { get; set; } = null!;

    public string Body { get; set; } = null!;

    public int Likes { get; set; }

    public DateTime PublishedOn { get; set; }

    public virtual ICollection<AnswerEntity> Answers { get; set; } = new List<AnswerEntity>();

    public virtual UserEntity Author { get; set; } = null!;
}
