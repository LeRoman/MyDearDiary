using Diary.BLL.DTO.Account;
using Diary.BLL.Services.Account;
using Microsoft.AspNetCore.Mvc;

namespace Diary.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly InvitationService _inviteService;

        public AdminController(InvitationService inviteService)
        {
            _inviteService = inviteService;
        }

        /// <summary>
        /// Send invitation to a new user
        /// </summary>
        [HttpPost("admin")]
        public async Task<IActionResult> SendInvite([FromBody] InvitationDTO invitationDTO)
        {
            var invitation = await _inviteService.CreateInviteAsync(invitationDTO);
            await _inviteService.SendInvitationAsync(invitation.Email, invitation.Token);
            return Ok();
        }


    }
}
