using Diary.DAL.Entities.Abstract;

namespace Diary.DAL.Entities
{
    public class Image : BaseEntity
    {
        public string? FileName { get; private set; }
        public Guid RecordId { get; private set; }
    }
}
