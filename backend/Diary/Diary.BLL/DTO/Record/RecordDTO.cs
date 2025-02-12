namespace Diary.BLL.DTO.Record
{
    public class RecordDTO
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public string Title { get; set; }
        public string CreatedAt { get; set; }
        public bool CanDelete { get; set; }
        public string? Image { get; set; }
    }
}
