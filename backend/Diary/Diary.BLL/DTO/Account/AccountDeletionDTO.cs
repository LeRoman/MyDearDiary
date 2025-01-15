using System.ComponentModel.DataAnnotations;

namespace Diary.BLL.DTO
{
    public class AccountDeletionDTO
    {
        [Required]
        public string? Password { get; set; }
    }
}
