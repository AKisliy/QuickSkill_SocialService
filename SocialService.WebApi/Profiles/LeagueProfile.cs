using AutoMapper;
using SocialService.Core.Models;
using SocialService.DataAccess;

namespace SocialService.WebApi.Profiles
{
    public class LeagueProfile: Profile
    {
        public LeagueProfile()
        {
            CreateMap<LeagueEntity, League>().ReverseMap();
        }
    }
}