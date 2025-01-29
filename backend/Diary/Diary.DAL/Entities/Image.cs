using Diary.DAL.Entities.Abstract;

namespace Diary.DAL.Entities
{
    public class Image : BaseEntity
    {
        public string? FileName { get; set; }
        public Guid RecordId { get; set; }
        public Record Record { get; set; }
        public string Path { get; set; }
        public long Size { get; set; }
        public DateTime UploadedAt { get; set; }

    }
}
