using AutoMapper;
using SocialService.Core.Models;
using SocialService.DataAccess;
using SocialService.WebApi.Dtos.ResponseDtos;

namespace SocialService.WebApi.Profiles
{
    public class AnswerProfile: Profile
    {
        public AnswerProfile()
        {
            CreateMap<AnswerEntity, Answer>().ReverseMap();
            CreateMap<Answer, AnswerResponse>();
        }
    }
}