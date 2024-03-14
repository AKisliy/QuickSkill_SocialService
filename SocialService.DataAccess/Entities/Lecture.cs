using System;
using System.Collections.Generic;

namespace SocialService.DataAccess;

public partial class Lecture
{
    public int Id { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
