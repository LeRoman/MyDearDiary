﻿using Diary.BLL.DTO.Account;
using Diary.BLL.DTO.Output;
using Diary.BLL.Exceptions;
using Diary.BLL.Services.Account;
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

        /// <summary>
        /// Register new user
        /// </summary>
        [AllowAnonymous]
        [HttpPost("registration")]
        public async Task<IActionResult> RegisterUser([FromBody]UserCreateDTO userCreateDTO)
        {
            var invitation = await _inviteService.ValidateInviteTokenAsync(userCreateDTO.Token);

            if (!invitation) throw new InvalidTokenException();

            await _userService.CreateUser(userCreateDTO);
            await _inviteService.MarkTokenAsUsedAsync(userCreateDTO.Token);

            return Ok("Registration successful");
        }

        /// <summary>
        /// Log in into system
        /// </summary>
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]UserLoginDTO UserLoginDTO)
        {
            var token = await _userService.Authenticate(UserLoginDTO)
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
