using System;
using System.Collections.Generic;

namespace SocialService.DataAccess;

public class AnswerEntity
{
    public int Id { get; set; }

    public int DiscussionId { get; set; }

    public int UserId { get; set; }

    public string Body { get; set; } = null!;

    public int Likes { get; set; }

    public DateOnly PublishedOn { get; set; }

    public DateOnly? EditedOn { get; set; }

    public virtual DiscussionEntity Discussion { get; set; } = null!;

    public virtual UserEntity User { get; set; } = null!;
}
