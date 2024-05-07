namespace SocialService.WebApi.Dtos.ResponseDtos
{
    public class DiscussionResponse
    {
        public required DiscussionDto Discussion { get; set; }

        public required UserCardDto Author { get; set; }
        
    }
}