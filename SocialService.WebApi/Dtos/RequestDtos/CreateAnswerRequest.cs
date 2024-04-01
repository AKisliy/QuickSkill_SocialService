using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialService.WebApi.Dtos.RequestDtos
{
    public class CreateAnswerRequest
    {
        public required int DiscussionId { get; set; }

        public required int UserId { get; set; }

        public required string Body { get; set; }
    }
}