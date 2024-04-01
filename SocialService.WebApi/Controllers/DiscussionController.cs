using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SocialService.Core.Enums;
using SocialService.Core.Exceptions;
using SocialService.Core.Interfaces.Services;
using SocialService.WebApi.Dtos;
using SocialService.WebApi.Dtos.RequestDtos;
using SocialService.WebApi.Dtos.ResponseDtos;

namespace SocialService.WebApi.Controllers
{
    [ApiController]
    [Route("api/discussion")]
    public class DiscussionController : ControllerBase
    {
        private IDiscussionService _discussionService;
        private IMapper _mapper;

        public DiscussionController(IDiscussionService discussionService, IMapper mapper)
        {
            _discussionService = discussionService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateDiscussion([FromBody] CreateDiscussionRequest request)
        {
            if(string.IsNullOrEmpty(request.Topic) || string.IsNullOrEmpty(request.Body))
                throw new BadRequestException("Topic and body must be non-empty");
            int id = await _discussionService.CreateDiscussion(request.Topic, request.Body, request.UserId);
            return Created($"api/discussion/{id}", id); // here should be created discussion
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDiscussion(int id)
        {
            await _discussionService.DeleteDiscussion(id);
            return Ok();
        }

        /// <summary>
        /// Get discussions page
        /// </summary>
        /// <param name="page">Number of page to get(0-indexed)</param>
        /// <param name="pageSize">Size of the page(>=1)</param>
        /// <returns>List of UserResponse</returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad request</response>
        [HttpGet(Name = "Get discussions page")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetDiscussionsPage(int page, int pageSize, [FromQuery] OrderByOptions sortBy)
        {
            var res =  _discussionService.GetDiscussions(page, pageSize, OrderByOptions.ByDate);
            var response = res.Select(d => new DiscussionResponse 
            {
                Discussion = new DiscussionDto{Id = d.Id, Title = d.Title, Body = d.Body, Likes = d.Likes, PublishedOn = d.PublishedOn},
                Author = _mapper.Map<UserInDiscussionDto>(d.Author)
            });
            return Ok(response);
        }
    }
}