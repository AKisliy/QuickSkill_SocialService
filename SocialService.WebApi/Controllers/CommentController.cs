using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SocialService.Core.Enums;
using SocialService.Core.Interfaces.Services;
using SocialService.WebApi.Dtos.ResponseDtos;
using SocialService.WebApi.Extensions;

namespace SocialService.WebApi.Controllers
{
    [ApiController]
    [Route("api/social/comment")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly IMapper _mapper;

        public CommentController(ICommentService commentService, IMapper mapper)
        {
            _commentService = commentService;
            _mapper = mapper;
        }

        [HttpPost("lecture/{lectureId}", Name = "Create new comment")]
        public async Task<IActionResult> CreateComment(int lectureId, string body) // user id just for now
        {
            int userId = HttpContext.GetUserIdFromHeader();
            int commentId = await _commentService.CreateComment(userId, lectureId, body);
            return Created($"api/comment/{commentId}", commentId);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            await _commentService.DeleteComment(id);
            return Ok();
        }

        [HttpGet("lecture/{lectureId}", Name = "Get page of comments for lecture")]
        public async Task<IActionResult> GetPageOfComments(int lectureId, [Required]int page, int pageSize = 5, OrderByOptions options = OrderByOptions.SimpleOrder)
        {
            var res = await _commentService.GetPageOfComments(lectureId, page, pageSize, options);
            return Ok(res.Select(a => _mapper.Map<CommentResponse>(a)));
        }

        [HttpPatch("{id}", Name = "Edit comment")]
        public async Task<IActionResult> EditComment(int id, [Required]string newBody)
        {
            await _commentService.EditComment(id, newBody);
            return Ok();
        }
    }
}