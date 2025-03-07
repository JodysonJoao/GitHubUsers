using GitHubUsers.Server.Services;
using Microsoft.AspNetCore.Mvc;
using GitHubUsers.Server.Models;

namespace GitHubUsers.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GitHubController : ControllerBase
    {
        private readonly UserService _userService;

        public GitHubController(UserService gitHubUsers)
        {
            _userService = gitHubUsers;
        }

        [HttpPost]
        [Route("GetUser")]
        public async Task<IActionResult> GetUser([FromBody] GitHubUserRequest request)
        {
            if (string.IsNullOrEmpty(request?.Username))
            {
                return BadRequest("Username cannot be null or empty.");
            }

            var user = await _userService.GetGitHubUser(request.Username);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            return Ok(user);
        }

    }
}
