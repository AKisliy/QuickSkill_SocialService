using AutoMapper;
using SocialService.Core.Models;
using SocialService.DataAccess;
using SocialService.WebApi.Dtos.ResponseDtos;

namespace SocialService.WebApi.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserSearchResponseDto>()
                .ForMember(u => u.Subscribed, opt => opt.Ignore());
            CreateMap<User, UserEntity>();
            CreateMap<UserEntity, User>();
        }
    }
}