using Diary.BLL.DTO;
using Diary.BLL.Services;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> RegisterUser(UserCreateDTO userCreateDTO)
        {
            var invitation = await _inviteService.ValidateTokenAsync(userCreateDTO.Token);
            if (invitation == false)
            {
                return BadRequest("Invalid or expired invite token");
            }

            await _userService.CreateUser(userCreateDTO);
            await _inviteService.MarkTokenAsUsedAsync(userCreateDTO.Token);
            return Ok("Registration successful");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDTO UserLoginDTO)
        {
            var token = await _userService.Authenticate(UserLoginDTO);
            if (token == null)
            {
                return Unauthorized("Invalid credentials.");
            }

            return Ok(token);
        }
    }
}
