using System.ComponentModel.DataAnnotations;

namespace Diary.BLL.DTO
{
    public class RecordDTO
    {
        [Required]
        [MaxLength(3, ErrorMessage = "Cannot exceed 500 characters")]
        public string? Content { get; set; }
    }
}
