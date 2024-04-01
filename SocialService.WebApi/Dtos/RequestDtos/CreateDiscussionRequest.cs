using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialService.WebApi.Dtos.RequestDtos
{
    public class CreateDiscussionRequest
    {
        public required string Topic { get; set; }

        public required string Body { get; set; }

        public int UserId { get; set; }
    }
}