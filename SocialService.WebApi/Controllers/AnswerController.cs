using System.ComponentModel.DataAnnotations;
using System.Net;
using AutoMapper;
using MassTransit.Initializers;
using Microsoft.AspNetCore.Mvc;
using SocialService.Core.Enums;
using SocialService.Core.Exceptions;
using SocialService.Core.Interfaces.Services;
using SocialService.Core.Models;
using SocialService.WebApi.Dtos;
using SocialService.WebApi.Dtos.RequestDtos;
using SocialService.WebApi.Dtos.ResponseDtos;
using SocialService.WebApi.Extensions;

namespace SocialService.WebApi.Controllers
{
    [ApiController]
    [Route("api/social/answer")]
    public class AnswerController : ControllerBase
    {
        private readonly IAnswerService _answerService;
        private readonly IMapper _mapper;

        public AnswerController(IAnswerService answerService, IMapper mapper)
        {
            _answerService = answerService;
            _mapper = mapper;
        }

        /// <summary>
        /// Create new answer (pay attention that u don't have to provide userId)
        /// </summary>
        /// <param name="request">Id of answer to get</param>
        /// <returns>AnswerResponse object</returns>
        /// <response code="201">Answer was created</response>
        /// <response code="400">Bad request body</response>
        [HttpPost(Name = "Create new answer")]
        [ProducesResponseType(typeof(Answer), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateNewAnswer([FromBody] CreateAnswerRequest request)
        {
            if(string.IsNullOrEmpty(request.Body))
                throw new BadRequestException("Body should be not empty");
            var userId = request.UserId;
            if(userId == 0)
                userId = HttpContext.GetUserIdFromHeader();
            var id = await _answerService.CreateAnswer(userId, request.DiscussionId, request.Body);
            var answer = await _answerService.GetAnswerById(id);
            return Created($"api/social/answer/{id}", answer);
        }

        /// <summary>
        /// Get answer by ID
        /// </summary>
        /// <param name="id">Id of answer to get</param>
        /// <returns>AnswerResponse object</returns>
        /// <response code="200">Success</response>
        /// <response code="404">Answer with provided ID not found</response>
        [HttpGet("{id}", Name = "Get answer by id")]
        [ProducesResponseType(typeof(AnswerResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAnswer(int id)
        {
            var answer = await _answerService.GetAnswerById(id);
            return Ok(_mapper.Map<AnswerResponse>(answer));
        }

        /// <summary>
        /// Get answers page
        /// </summary>
        /// <param name="id">Id of answer to delete</param>
        /// <response code="200">Success</response>
        /// <response code="404">Answer with provided ID not found</response>
        [HttpDelete("{id}", Name = "Delete answer with id")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteAnswer(int id)
        {
            await _answerService.DeleteAnswer(id);
            return Ok();
        }

        /// <summary>
        /// Get answers page
        /// </summary>
        /// <param name="id">Id of answer to edit</param>
        /// <param name="newBody">New comment text</param>
        /// <response code="200">Success</response>
        /// <response code="404">Answer with provided ID not found</response>
        [HttpPatch("{id}", Name = "Edit answer")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> EditAnswer(int id, [Required]string newBody)
        {
            await _answerService.EditAnswer(id, newBody);
            return Ok();
        }

        /// <summary>
        /// Get answers page
        /// </summary>
        /// <param name="discussionId">Id of discussion, for which u need answers</param>
        /// <param name="page">Number of page to get(0-indexed)</param>
        /// <param name="pageSize">Size of the page(>=1)</param>
        /// <param name="options">Rule of sorting</param>
        /// <returns>List of AnswerResponse</returns>
        /// <response code="200">Success</response>
        /// <response code="404">Discussion with provided ID not found</response>
        [HttpGet("discussion/{discussionId}", Name = "Get answers for discussion")]
        [ProducesResponseType(typeof(IEnumerable<AnswerResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAnswersPageForDiscussion(int discussionId, int page, int pageSize, OrderByOptions options)
        {
            var result = await _answerService.GetAnswersPage(discussionId, page, pageSize, options);
            return Ok(result.Select(a => _mapper.Map<AnswerResponse>(a)));
        }
    }
}