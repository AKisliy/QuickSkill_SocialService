namespace SocialService.WebApi.Dtos.ResponseDtos
{
    public class UserSearchResponseDto
    {
        public int Id { get; set; }

        public string Username { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public bool Subscribed { get; set; }

        public string? Photo { get; set; }
    }
}