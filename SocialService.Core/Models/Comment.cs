namespace SocialService.Core.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public int LectureId { get; set; }

        public int UserId { get; set; }

        public string Body { get; set; } = null!;

        public int Likes { get; set; }

        public DateOnly PublishedOn { get; set; }

        public DateOnly? EditedOn { get; set; }
    }
}