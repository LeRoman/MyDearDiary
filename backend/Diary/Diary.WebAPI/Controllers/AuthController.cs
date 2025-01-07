using Diary.BLL.DTO;
using Diary.BLL.Services;
using Diary.WebAPI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Diary.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AuthController : Controller
    {
        private readonly InvitationService _inviteService;
        private readonly UserService _userService;
        private readonly SessionService _sessionService;

        public AuthController(InvitationService inviteService, UserService userService, SessionService sessionService)
        {
            _inviteService = inviteService;
            _userService = userService;
            _sessionService = sessionService;
        }

        [AllowAnonymous]
        [HttpPost("registration")]
        public async Task<IActionResult> RegisterUser([FromBody]UserCreateDTO userCreateDTO)
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

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]UserLoginDTO UserLoginDTO)
        {
            var token = await _userService.Authenticate(UserLoginDTO);
            if (token == null)
            {
                return Unauthorized("Invalid credentials.");
            }

            return Ok(token);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _sessionService.RevokeSessionsAsync();
            return Ok();
        }

        [HttpPost("delete")]
        public async Task<IActionResult> DeleteAccount([FromBody] AccountDeletionDTO accountDeletionDTO)
        {
            await _userService.DeleteAccountAsync(accountDeletionDTO);
            return Ok();
        }

        [AllowRestore]
        [HttpPost("restore")]
        public async Task<IActionResult> RestoreAccount([FromBody] AccountDeletionDTO accountDeletionDTO)
        {
            await _userService.RestoreAccountAsync(accountDeletionDTO);
            return Ok();
        }
    }
}
