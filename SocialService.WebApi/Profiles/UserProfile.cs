using AutoMapper;
using SocialService.Core.Models;
using SocialService.Core.Models.UserModels;
using SocialService.DataAccess;
using Shared;
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
            CreateMap<UserEntity, User>()
                .ForMember(u => u.League, opt => opt.MapFrom(u => u.League));
            CreateMap<User, UserOnLeaderboardReponse>();
            CreateMap<UserEntity, UserLeaderboardUpdate>();
            CreateMap<User, UserLeaderboardUpdate>();
            CreateMap<User, Bot>();
            CreateMap<User, UserCardDto>();
        }
    }
}