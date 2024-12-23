using Diary.DAL.Entities.Abstract;

namespace Diary.DAL.Entities
{
    public class Session : BaseEntity
    {
        public Guid UserId { get; private set; }
        public DateTime? ExpiryAt { get; private set; }
    }
}
