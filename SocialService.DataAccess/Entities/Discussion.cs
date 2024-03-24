using System;
using System.Collections.Generic;

namespace SocialService.DataAccess;

public partial class Discussion
{
    public int Id { get; set; }

    public int AuthorId { get; set; }

    public string Title { get; set; } = null!;

    public string Body { get; set; } = null!;

    public int? Likes { get; set; }

    public DateOnly PublishedOn { get; set; }

    public virtual ICollection<AnswerEntity> Answers { get; set; } = new List<AnswerEntity>();

    public virtual User Author { get; set; } = null!;
}
