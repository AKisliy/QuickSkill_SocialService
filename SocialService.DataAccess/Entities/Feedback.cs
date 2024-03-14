using System;
using System.Collections.Generic;

namespace SocialService.DataAccess;

public partial class Feedback
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Body { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
