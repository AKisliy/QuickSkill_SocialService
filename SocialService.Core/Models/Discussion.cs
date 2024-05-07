namespace SocialService.Core.Models
{
    public class Discussion
    {
        public int Id { get; set; }

        public int AuthorId { get; set; }

        public string Title { get; set; } = null!;

        public string Body { get; set; } = null!;

        public int Likes { get; set; }

        public int AnswersCount { get; set; }

        public DateTime PublishedOn { get; set; }

        public User? Author { get; set; }

        public ICollection<Answer>? Answers { get; set; }
    }
}