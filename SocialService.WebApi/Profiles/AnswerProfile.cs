using AutoMapper;
using SocialService.Core.Models;
using SocialService.DataAccess;

namespace SocialService.WebApi.Profiles
{
    public class AnswerProfile: Profile
    {
        public AnswerProfile()
        {
            CreateMap<AnswerEntity, Answer>();
        }
    }
}