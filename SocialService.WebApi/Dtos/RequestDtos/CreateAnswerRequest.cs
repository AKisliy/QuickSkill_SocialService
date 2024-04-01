using System.ComponentModel.DataAnnotations;

namespace SocialService.WebApi.Dtos.RequestDtos
{
    public class CreateAnswerRequest
    {
        [Required]
        public required int DiscussionId { get; set; }

        /// <summary>
        /// It's not required. 
        /// </summary>
        public int UserId { get; set; }

        [Required]
        public required string Body { get; set; }
    }
}