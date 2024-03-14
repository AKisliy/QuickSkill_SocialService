using System;
using System.Collections.Generic;

namespace SocialService.DataAccess;

public partial class Comment
{
    public int Id { get; set; }

    public int? LectureId { get; set; }

    public int? UserId { get; set; }

    public int? Likes { get; set; }

    public DateOnly PublishedOn { get; set; }

    public virtual Lecture? Lecture { get; set; }

    public virtual User? User { get; set; }
}
