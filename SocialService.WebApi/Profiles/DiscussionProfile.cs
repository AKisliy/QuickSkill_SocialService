using AutoMapper;
using SocialService.Core.Models;
using SocialService.DataAccess;

namespace SocialService.WebApi.Profiles
{
    public class DiscussionProfile : Profile
    {
        public DiscussionProfile()
        {
            CreateMap<DiscussionEntity, Discussion>();
        }
    }
}