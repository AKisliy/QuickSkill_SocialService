using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SocialService.Core.Interfaces.Services;
using SocialService.WebApi.Dtos.RequestDtos;
using SocialService.WebApi.Dtos.ResponseDtos;

namespace SocialService.WebApi.Controllers
{
    [ApiController]
    [Route("api/answer")]
    public class AnswerController : ControllerBase
    {
        private readonly IAnswerService _answerService;
        private readonly IMapper _mapper;

        public AnswerController(IAnswerService answerService, IMapper mapper)
        {
            _answerService = answerService;
            _mapper = mapper;
        }

        [HttpPost(Name = "Create new answer")]
        public async Task<IActionResult> CreateNewAnswer([FromBody] CreateAnswerRequest request)
        {
            var id = await _answerService.CreateAnswer(request.UserId, request.DiscussionId, request.Body);
            return Created($"api/answer/{id}", id);
        }

        [HttpGet("{id}", Name = "Get answer by id")]
        public async Task<IActionResult> GetAnswer(int id)
        {
            var answer = await _answerService.GetAnswerById(id);
            return Ok(_mapper.Map<AnswerResponse>(answer));
        }

        [HttpDelete("{id}", Name = "Delete answer with id")]
        public async Task<IActionResult> DeleteAnswer(int id)
        {
            await _answerService.DeleteAnswer(id);
            return Ok();
        }

        [HttpPatch("{id}", Name = "Edit answer")]
        public async Task<IActionResult> EditAnswer(int id, [Required]string newBody)
        {
            await _answerService.EditAnswer(id, newBody);
            return Ok();
        }
    }
}