using System.ComponentModel.DataAnnotations;

namespace Diary.BLL.DTO.Account
{
    public class UserLoginDTO
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
