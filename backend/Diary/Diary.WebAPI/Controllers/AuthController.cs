using Diary.BLL.DTO.Account;
using Diary.BLL.Exceptions;
using Diary.BLL.Services.Account;
using Diary.BLL.Services.Interfaces;
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
        private readonly IInvitationService _inviteService;
        private readonly IUserService _userService;
        private readonly ISessionService _sessionService;

        public AuthController(IInvitationService inviteService, IUserService userService, ISessionService sessionService)
        {
            _inviteService = inviteService;
            _userService = userService;
            _sessionService = sessionService;
        }

        /// <summary>
        /// Register new user
        /// </summary>
        [AllowAnonymous]
        [HttpPost("registration")]
        public async Task<IActionResult> RegisterUser([FromBody] UserCreateDTO userCreateDTO)
        {
            var invitation = await _inviteService.ValidateInviteTokenAsync(userCreateDTO.Token);

            if (!invitation) throw new InvalidTokenException();

            await _userService.CreateUser(userCreateDTO);
            await _inviteService.MarkTokenAsUsedAsync(userCreateDTO.Token);

            return Ok(new { message = "Registration successful" });
        }

        /// <summary>
        /// Log in into system
        /// </summary>
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO UserLoginDTO)
        {
            var token = await _userService.AuthenticateAsync(UserLoginDTO)
                ?? throw new InvalidCredentialsException();

            var tokenDTO = new TokenDTO
            {
                Value = token
            };

            return Ok(tokenDTO);
        }

        /// <summary>
        /// Terminate all sessions and logout
        /// </summary>
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _sessionService.RevokeSessionsAsync();
            return Ok();
        }

        /// <summary>
        /// Delete account
        /// </summary>
        [HttpPost("delete")]
        public async Task<IActionResult> DeleteAccount([FromBody] AccountDeletionDTO accountDeletionDTO)
        {
            await _userService.DeleteAccountAsync(accountDeletionDTO);
            return Ok();
        }

        /// <summary>
        /// Restore account
        /// </summary>
        [AllowRestore]
        [HttpPost("restore")]
        public async Task<IActionResult> RestoreAccount([FromBody] AccountDeletionDTO accountDeletionDTO)
        {
            await _userService.RestoreAccountAsync(accountDeletionDTO);
            return Ok();
        }
    }
}
