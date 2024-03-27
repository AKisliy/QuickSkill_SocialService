using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SocialService.Core.Interfaces.Services;
using SocialService.WebApi.Dtos.RequestDtos;

namespace SocialService.WebApi.Controllers
{
    [ApiController]
    [Route("api/league")]
    public class LeagueController : ControllerBase
    {
        private readonly ILeagueService _leagueService;
        private readonly IMapper _mapper;

        public LeagueController(ILeagueService leagueService, IMapper mapper)
        {
            _leagueService = leagueService;
            _mapper = mapper;
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetLeagueByName(string name)
        {
            var league = await _leagueService.GetLeagueByName(name);
            return Ok(league);
        }

        [HttpGet("all", Name = "Get all leagues(sorted by hierarchy)")]
        public IActionResult GetLeagues()
        {
            var leagues = _leagueService.GetLeagues();
            return Ok(leagues);
        }

        [HttpPost(Name = "Create new League")]
        public async Task<IActionResult> CreateNewLeague(CreateLeagueRequest request)
        {
            await _leagueService.CreateLeague(request.LeagueName, request.Photo, request.HierarchyPlace);
            var league = await _leagueService.GetLeagueByName(request.LeagueName);
            return Created($"api/league/{request.LeagueName}", league);
        }

        [HttpDelete("{name}", Name = "Delete league by name")]
        public async Task<IActionResult> DeleteLeagueWithName(string name)
        {
            await _leagueService.DeleteLeague(name);
            return Ok();
        }

        [HttpPatch("{id}", Name = "Update league information")]
        public async Task<IActionResult> UpdateLeagueWithId(int id, UpdateLeagueRequest request)
        {
            await _leagueService.UpdateLeague(id, request.LeagueName, request.Photo, request.HierarchyPlace);
            return Ok();
        }

        [HttpPatch("swap", Name = "Swap two leagues places in hierarchy")]
        public async Task<IActionResult> SwapLeaguePlaces(string league1, string league2)
        {
            await _leagueService.SwapLeaguePlaces(league1, league2);
            return Ok();
        }
    }
}