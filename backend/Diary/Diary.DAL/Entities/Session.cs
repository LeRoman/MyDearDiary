using Diary.DAL.Entities.Abstract;

namespace Diary.DAL.Entities
{
    public class Session : BaseEntity
    {
        public Guid UserId { get; set; }
        public DateTime? ExpiryAt { get; set; }
        public bool IsRevoked { get; set; }
    }
}
