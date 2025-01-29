using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diary.BLL.DTO
{
    public class CaptchaVerificationRequest
    {
        public string Token { get; set; } = "";
        public string UserInput { get; set; } = "";
    }
}
