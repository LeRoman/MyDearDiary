using System.ComponentModel.DataAnnotations;

namespace Diary.BLL.DTO.Account
{
    public class AccountDeletionDTO
    {
        [Required]
        public string? Password { get; set; }
    }
}
