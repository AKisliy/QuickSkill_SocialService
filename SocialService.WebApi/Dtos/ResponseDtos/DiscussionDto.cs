namespace SocialService.WebApi.Dtos.ResponseDtos
{
    public class DiscussionDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string Body { get; set; } = null!;

        public int Likes { get; set; }

        public DateTime PublishedOn { get; set; }
    }
}