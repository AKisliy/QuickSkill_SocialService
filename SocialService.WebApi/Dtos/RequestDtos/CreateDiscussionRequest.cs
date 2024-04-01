using System.ComponentModel.DataAnnotations;

namespace SocialService.WebApi.Dtos.RequestDtos
{
    public class CreateDiscussionRequest
    {
        [Required]
        public required string Topic { get; set; }

        [Required]
        public required string Body { get; set; }

        /// <summary>
        /// It's not required
        /// </summary>
        public int UserId { get; set; }
    }
}