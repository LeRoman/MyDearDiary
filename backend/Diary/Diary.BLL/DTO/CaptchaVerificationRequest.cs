using System.ComponentModel.DataAnnotations;

namespace Diary.BLL.DTO
{
    public class CaptchaVerificationRequest
    {
        public string Token { get; set; } = "";
        [Required]
        public string UserInput { get; set; } = "";
    }
}
