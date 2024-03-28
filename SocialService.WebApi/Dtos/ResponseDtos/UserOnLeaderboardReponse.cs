using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialService.WebApi.Dtos.ResponseDtos
{
    public class UserOnLeaderboardReponse
    {
        public string Username { get; set; } = null!;

        public int WeeklyXp { get; set; }

        public string? Photo { get; set; }
    }
}