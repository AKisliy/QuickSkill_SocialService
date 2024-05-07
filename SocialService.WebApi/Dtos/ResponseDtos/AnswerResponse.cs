namespace SocialService.WebApi.Dtos.ResponseDtos
{
    public class AnswerResponse
    {
        public int Id { get; set; }

        public string Body { get; set; } = null!;

        public int? Likes { get; set; }

        public DateTime PublishedOn { get; set; }

        public DateTime? EditedOn { get; set; }

        public required UserCardDto User { get; set; }
    }
}