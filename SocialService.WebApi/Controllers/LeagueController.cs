using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SocialService.Core.Interfaces.Services;

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
            return Ok(_leagueService.GetLeagues());
        }
    }
}