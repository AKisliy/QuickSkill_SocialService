namespace SocialService.Core.Models
{
    public class Answer
    {
        public int Id { get; set; }

        public int DiscussionId { get; set; }

        public int UserId { get; set; }

        public string Body { get; set; } = null!;

        public int? Likes { get; set; }

        public DateTime PublishedOn { get; set; }

        public DateTime? EditedOn { get; set; }

        public User? User { get; set; }
    }
}