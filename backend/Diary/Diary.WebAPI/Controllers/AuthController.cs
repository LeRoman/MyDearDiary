using Azure.Core;
using Diary.BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Diary.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly InvitationService _inviteService;
        private readonly UserService _userService;

        public AuthController(InvitationService inviteService, UserService userService)
        {
            _inviteService = inviteService;
            _userService = userService;
        }

        [HttpPost("registration")]
        public async Task<IActionResult> RegisterUser(string token, string email, string name, string password)
        {
            var invitation = await _inviteService.ValidateTokenAsync(token);
            if (invitation == false)
            {
                return BadRequest("Invalid or expired invite token");
            }

            await _userService.CreateUser(email, name, password);
            await _inviteService.MarkTokenAsUsedAsync(token);
            return Ok("Registration successful");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(string email, string password)
        {
            var token = await _userService.Authenticate(email, password);
            if (token == null)
            {
                return Unauthorized("Invalid credentials.");
            }

            return Ok(token);
        }
    }
}
