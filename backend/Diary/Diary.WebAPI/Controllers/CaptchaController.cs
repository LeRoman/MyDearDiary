using Diary.BLL.DTO;
using Diary.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Diary.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class CaptchaController : Controller
    {
        private readonly ICapthaService _capthaService;

        public CaptchaController(ICapthaService capthaService)
        {
            _capthaService = capthaService;
        }
        /// <summary>
        /// Generate captcha
        /// </summary>
        [HttpGet("generate")]
        public IActionResult GenerateCaptcha()
        {
            string captchaText = _capthaService.GenerateRandomText(5);

            var token = _capthaService.GenerateToken(captchaText);

            return Ok(new
            {
                CaptchaImage = _capthaService.GenerateCaptchaImageAsBase64(captchaText),
                Token = token
            });
        }

        /// <summary>
        /// Verify captcha
        /// </summary>
        [HttpPost("verify")]
        public IActionResult VerifyCaptcha([FromBody] CaptchaVerificationRequest request)
        {
            var captchaText = _capthaService.ValidateToken(request.Token);
            if (captchaText == null || !captchaText.Equals(request.UserInput, StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("Captcha verification failed");
            }

            return Ok();
        }
    }
}
