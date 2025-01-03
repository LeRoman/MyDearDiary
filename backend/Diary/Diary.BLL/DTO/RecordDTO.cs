using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Diary.BLL.DTO
{
    public class RecordDTO
    {
        [Required]
        [MaxLength(3, ErrorMessage = "Cannot exceed 500 characters")]
        public string? Content { get; set; }
        public IFormFileCollection? Images { get; set; }
    }
}
