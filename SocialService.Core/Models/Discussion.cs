namespace SocialService.Core.Models
{
    public class Discussion
    {
        public int Id { get; set; }

        public int AuthorId { get; set; }

        public string Title { get; set; } = null!;

        public string Body { get; set; } = null!;

        public int Likes { get; set; }

        public DateOnly PublishedOn { get; set; }

        // public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();
        // нужны ли нам здесь ответы?
    }
}