namespace SocialService.WebApi.Dtos.ResponseDtos
{
    public class DiscussionOnPageResponse
    {
        public required DiscussionDto Discussion { get; set; }

        public required UserCardDto Author { get; set; }
    }
}