using Diary.BLL.DTO.Account;
using Diary.BLL.Services.Account;
using Diary.BLL.Services.Interfaces;
using Diary.DAL.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Diary.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class TokenController : Controller
    {
        private readonly UserIdStorage _idStorage;
        private readonly ISessionService _sessionService;
        private readonly IJwtService _jwtService;
        private readonly DiaryContext _context;

        public TokenController(UserIdStorage idStorage,ISessionService sessionService,IJwtService jwtService, DiaryContext context )
        {
            this._idStorage = idStorage;
            this._sessionService = sessionService;
            this._jwtService = jwtService;
            this._context = context;
        }


        /// <summary>
        /// Get fresh token
        /// </summary>
        [AllowAnonymous]
        [HttpGet("refresh")]
        public async Task<IActionResult> Refresh()
        {
            var userGuidId = Guid.Parse(_idStorage.CurrentUserId);
            var user = await _context.Users.FirstOrDefaultAsync(x=>x.Id==userGuidId);

            if (user != null)
            {
                var session = await _sessionService.GetActiveSession(user);
                if (session!=null)
                {
                    var tokenDTO = new TokenDTO
                    {
                        Value = _jwtService.GenerateJwtToken(user, session),
                    };
                    return Ok(tokenDTO);
                }

            }
            
            return Unauthorized();
        }
    }

}
