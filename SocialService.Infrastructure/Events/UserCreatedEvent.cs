using EventBus.Base.Standard;

namespace SocialService.Infrastructure
{
    public class UserCreatedEvent: IntegrationEvent
    {
        public int UserId { get; set; }
        
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Status { get; set; } = null!;

        public string Username { get; set; } = null!;

        public UserCreatedEvent(int userId, string firstName, string lastName, string status, string username) 
        {
            this.UserId = userId;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Status = status;
            this.Username = username;
        }
    }
}