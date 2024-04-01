namespace SocialService.WebApi.Dtos.ResponseDtos
{
    public class DiscussionResponse
    {
        public required DiscussionDto Discussion { get; set; }

        public required UserInDiscussionDto Author { get; set; }
    }
}