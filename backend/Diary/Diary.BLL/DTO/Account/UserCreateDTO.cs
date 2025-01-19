using System.ComponentModel.DataAnnotations;

namespace Diary.BLL.DTO.Account
{
    public class UserCreateDTO
    {
        [Required]
        public string Token { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [MinLength(3)]
        public string Name { get; set; }

        [Required]
        [MinLength(8)]
        public string Password { get; set; }
    }
}
