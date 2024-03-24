using Microsoft.AspNetCore.Mvc;

namespace SocialService.WebApi.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            return Ok(id);
        }
    }
}