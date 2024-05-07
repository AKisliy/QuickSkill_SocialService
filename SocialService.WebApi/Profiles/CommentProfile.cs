using AutoMapper;
using SocialService.Core.Models;
using SocialService.DataAccess;
using SocialService.WebApi.Dtos.ResponseDtos;

namespace SocialService.WebApi.Profiles
{
    public class CommentProfile: Profile
    {
        public CommentProfile()
        {
            CreateMap<Comment, CommentEntity>();
            CreateMap<CommentEntity, Comment>();
            CreateMap<Comment, CommentResponse>();
        }
    }
}