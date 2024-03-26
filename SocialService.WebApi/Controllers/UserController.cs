using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SocialService.Core.Interfaces.Services;
using SocialService.Core.Models;
using SocialService.WebApi.Dtos.RequestDtos;
using SocialService.WebApi.Dtos.ResponseDtos;

namespace SocialService.WebApi.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpPost("add")]
        public IActionResult CreateUser(AddUserDto user)
        {
            _userService.AddUser(user.Id, user.FirstName, user.LastName, user.Username);
            return Ok();
        }

        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            return Ok(id);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchUserByUsername([Required] string userName,[Required] int page, int pageSize = 5)
        {
            // here should probably be getting id from token
            const int id = 0;
            var result = await _userService.SearchUserByUsername(id, userName, page, pageSize);
            var subs = await _userService.GetUserSubscriptionsId(id);
            return Ok(result.Select(u => _mapper.Map<UserSearchResponseDto>(u, opts =>
                opts.AfterMap((src, dest) => dest.Subscribed = subs.Contains(((User)src).Id)
            ))));
        }
    }
}