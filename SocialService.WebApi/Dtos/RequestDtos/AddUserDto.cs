namespace SocialService.WebApi.Dtos.RequestDtos
{
    public class AddUserDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Username { get; set; } = null!;
    }
}