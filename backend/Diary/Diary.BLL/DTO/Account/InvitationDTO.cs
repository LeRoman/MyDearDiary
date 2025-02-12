using System.ComponentModel.DataAnnotations;

namespace Diary.BLL.DTO.Account
{
    public class InvitationDTO
    {
        [Required]
        public string Email { get; set; }

        public double ExpirationHours { get; set; } = 5;
    }
}
