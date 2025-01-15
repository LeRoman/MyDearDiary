using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Diary.BLL.DTO
{
    public class CreateRecordDTO
    {
        [Required]
        [MaxLength(500, ErrorMessage = "Cannot exceed 500 characters")]
        public string? Content { get; set; }
        public IFormFileCollection? Images { get; set; }
    }
}
