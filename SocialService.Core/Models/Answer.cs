using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialService.Core.Models
{
    public class Answer
    {
        public int Id { get; set; }

        public int DiscussionId { get; set; }

        public int UserId { get; set; }

        public string Body { get; set; } = null!;

        public int? Likes { get; set; }

        public DateOnly PublishedOn { get; set; }

    }
}