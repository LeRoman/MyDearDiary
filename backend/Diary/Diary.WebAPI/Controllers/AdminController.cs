using Diary.BLL.DTO;
using Diary.BLL.Services;
using Diary.DAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Diary.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly InvitationService _inviteService;
        private readonly UserService _userService;

        public AdminController(InvitationService inviteService, UserService userService)
        {
            _inviteService = inviteService;
            _userService = userService;
        }
        [HttpGet]
        public async Task<Invitation> GetInvite(InvitationDTO invitationDTO)
        {
            return await _inviteService.CreateInviteAsync(invitationDTO);
        }


    }
}
