using System.ComponentModel.DataAnnotations;
using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SocialService.Core.Interfaces.Services;
using SocialService.Core.Models;
using SocialService.WebApi.Dtos;
using SocialService.WebApi.Dtos.RequestDtos;
using SocialService.WebApi.Dtos.ResponseDtos;
using SocialService.WebApi.Extensions;

namespace SocialService.WebApi.Controllers
{
    [ApiController]
    [Route("api/social/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        /// <summary>
        /// It's for testing, don't use it.
        /// </summary>
        [HttpPost("add")]
        public async Task<IActionResult> CreateUser(AddUserDto user)
        {
            await _userService.AddUser(user.Id, user.FirstName, user.LastName, user.Username);
            return Ok();
        }

        /// <summary>
        /// Search user by username (for testing, provide ID of user, who makes the request)
        /// </summary>
        /// <param name="id">ID of user, who makes the request</param>
        /// <param name="userName">Username to search </param>
        /// <param name="page">Number of page to get(0 - indexed)</param>
        /// <param name="pageSize">Size of the page to get</param>
        /// <response code="200">Success</response>
        /// <response code="404">User with ID not found</response>
        /// <response code="500">Server problems :(</response>
        [HttpGet("{id}/search")]
        [ProducesResponseType(typeof(IEnumerable<UserSearchResponseDto>),(int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> SearchUserByUsername(int id,[Required] string userName,[Required] int page, int pageSize = 5)
        {
            var result = await _userService.SearchUserByUsername(id, userName, page, pageSize);
            var subs = await _userService.GetUserSubscriptionsId(id);
            return Ok(result.Select(u => _mapper.Map<UserSearchResponseDto>(u, opts =>
                opts.AfterMap((src, dest) => dest.Subscribed = subs.Contains(((User)src).Id)
            ))));
        }

        /// <summary>
        /// Search user by username (Use when NGINX is running. You just need to have token in cookies)
        /// </summary>
        /// <param name="userName">Username to search </param>
        /// <param name="page">Number of page to get(0 - indexed)</param>
        /// <param name="pageSize">Size of the page to get</param>
        /// <response code="200">Success</response>
        /// <response code="404">User with ID not found</response>
        /// <response code="500">Server problems :(</response>
        [HttpGet("search")]
        public async Task<IActionResult> SearchUserByUsernameByHeader([Required] string userName,[Required] int page, int pageSize = 5)
        {
            int id = HttpContext.GetUserIdFromHeader();
            var result = await _userService.SearchUserByUsername(id, userName, page, pageSize);
            var subs = await _userService.GetUserSubscriptionsId(id);
            return Ok(result.Select(u => _mapper.Map<UserSearchResponseDto>(u, opts =>
                opts.AfterMap((src, dest) => dest.Subscribed = subs.Contains(((User)src).Id)
            ))));
        }

        /// <summary>
        /// Get recommended subscriptions for user(based on leaderboard)(for testing. Provide ID of user, who makes the request)
        /// </summary>
        /// <param name="id">ID of user, who makes the request</param>
        /// <param name="pageSize">Count of users to get</param>
        /// <returns>Leaderboard for user with ID</returns>
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

        /// <summary>
        /// Get recommended subscriptions for user(based on leaderboard)(Use when NGINX is running. You just need to have token in cookies)
        /// </summary>
        /// <param name="pageSize">Count of users to get</param>
        /// <returns>Recommendation for user</returns>
        /// <response code="200">Success</response>
        /// <response code="400">User not found</response>
        [HttpGet("reccomendation")]
        public async Task<IActionResult> GetRecommendationByHeader(int pageSize = 10)
        {
            int id = HttpContext.GetUserIdFromHeader();
            var result = await _userService.GetRecommendationForUser(id, pageSize);
            var subs = await _userService.GetUserSubscriptionsId(id);
            return Ok(result.Select(u => _mapper.Map<UserSearchResponseDto>(u, opts =>
                opts.AfterMap((src, dest) => dest.Subscribed = subs.Contains(((User)src).Id)))));
        }

        /// <summary>
        /// Get user's leaderboard.
        /// </summary>
        /// <param name="id">Id of user</param>
        /// <returns>Leaderboard for user</returns>
        /// <response code="200">Success</response>
        /// <response code="400">User not found</response>
        [HttpGet("{id}/leaderboard")]
        public async Task<IActionResult> GetUserLeaderboard(int id)
        {
            var result = await _userService.GetUserLeaderboard(id);
            var leagueplace = result.First().League.HierarchyPlace;
            return Ok(new LeaderboardResponse{ LeaguePlace = leagueplace, Users = result.Select(u => _mapper.Map<UserOnLeaderboardReponse>(u))});
        }

        /// <summary>
        /// Get user's leaderboard. (Use when NGINX is running. You just need to have token in cookies)
        /// </summary>
        /// <returns>Leaderboard for user</returns>
        /// <response code="200">Success</response>
        /// <response code="404">User not found</response>
        [HttpGet("leaderboard")]
        public async Task<IActionResult> GetUserLeaderboardByHeader()
        {
            int id = HttpContext.GetUserIdFromHeader();
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

        [HttpPatch("subscribe/{id}")]
        public async Task<IActionResult> SubscribeOnUserByHeader(int id)
        {
            int userId = HttpContext.GetUserIdFromHeader();
            await _userService.SubcribeOnUser(userId, id);
            return Ok();
        }

        [HttpPatch("{userId}/unsubscribe/{id}")]
        public async Task<IActionResult> Unsubscribe(int userId, int id)
        {
            await _userService.Unsubscribe(userId, id);
            return Ok();
        }

        [HttpPatch("unsubscribe/{id}")]
        public async Task<IActionResult> UnsubscribeByHeader(int id)
        {
            int userId = HttpContext.GetUserIdFromHeader();
            await _userService.Unsubscribe(userId, id);
            return Ok();
        }
    }
}