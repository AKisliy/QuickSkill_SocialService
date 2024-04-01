using AutoMapper;
using SocialService.Core.Models;
using SocialService.DataAccess;
using SocialService.WebApi.Dtos.ResponseDtos;

namespace SocialService.WebApi.Profiles
{
    public class DiscussionProfile : Profile
    {
        public DiscussionProfile()
        {
            CreateMap<DiscussionEntity, Discussion>().ReverseMap();
            CreateMap<Discussion, DiscussionDto>();
        }
    }
}