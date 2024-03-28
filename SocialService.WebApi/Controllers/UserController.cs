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

        // just for test purposes
        [HttpPost("add")]
        public async Task<IActionResult> CreateUser(AddUserDto user)
        {
            await _userService.AddUser(user.Id, user.FirstName, user.LastName, user.Username);
            return Ok();
        }

        [HttpGet("{id}/search")]
        public async Task<IActionResult> SearchUserByUsername(int id,[Required] string userName,[Required] int page, int pageSize = 5)
        {
            // here should probably be getting id from token
            var result = await _userService.SearchUserByUsername(id, userName, page, pageSize);
            var subs = await _userService.GetUserSubscriptionsId(id);
            return Ok(result.Select(u => _mapper.Map<UserSearchResponseDto>(u, opts =>
                opts.AfterMap((src, dest) => dest.Subscribed = subs.Contains(((User)src).Id)
            ))));
        }

        /// <summary>
        /// Get recommended subscriptions for user(based on leaderboard)
        /// </summary>
        /// <param name="pageSize">Count of users to get</param>
        /// <returns>ID of badge + uri</returns>
        /// <response code="200">Success</response>
        /// <response code="400">User not found</response>
        [HttpGet("{id}/reccomendation")]
        public async Task<IActionResult> GetRecommendation(int id, int pageSize = 10)
        {
            var result = await _userService.GetRecommendationForUser(id, pageSize);
            var subs = await _userService.GetUserSubscriptionsId(id);
            return Ok(result.Select(u => _mapper.Map<UserSearchResponseDto>(u, opts =>
                opts.AfterMap((src, dest) => dest.Subscribed = subs.Contains(((User)src).Id)))));
        }

        [HttpGet("{id}/leaderboard")]
        public async Task<IActionResult> GetUserLeaderboard(int id)
        {
            var result = await _userService.GetUserLeaderboard(id);
            var leagueplace = result.First().League.HierarchyPlace;
            return Ok(new LeaderboardResponse{ LeaguePlace = leagueplace, Users = result.Select(u => _mapper.Map<UserOnLeaderboardReponse>(u))});
        }

        [HttpPatch("{userId}/subscribe/{id}")]
        public async Task<IActionResult> SubscribeOnUser(int userId, int id)
        {
            await _userService.SubcribeOnUser(userId, id);
            return Ok();
        }

        [HttpPatch("{userId}/unsubscribe/{id}")]
        public async Task<IActionResult> Unsubscribe(int userId, int id)
        {
            await _userService.Unsubscribe(userId, id);
            return Ok();
        }
    }
}