using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Diary.BLL.DTO.Record
{
    public class CreateRecordDTO
    {
        [Required]
        [MaxLength(500, ErrorMessage = "Cannot exceed 500 characters")]
        public string? Content { get; set; }
        public IFormFileCollection? Images { get; set; }
    }
}
