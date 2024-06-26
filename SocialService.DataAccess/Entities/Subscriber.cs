﻿using System;
using System.Collections.Generic;

namespace SocialService.DataAccess;

public partial class Subscriber
{
    public int UserId { get; set; }

    public int SubscriptionId { get; set; }

    public virtual UserEntity Subscription { get; set; } = null!;

    public virtual UserEntity User { get; set; } = null!;
}
