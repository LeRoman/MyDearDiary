
namespace Diary.DAL.Entities.Abstract
{
    public abstract class BaseEntity
    {
        private DateTime _createdAt;

        public BaseEntity()
        {
            CreatedAt = UpdatedAt = DateTime.Now;
            Id = Guid.NewGuid();
        }

        public Guid Id { get; private set; }
        public DateTime CreatedAt
        {
            get => _createdAt;
            set => _createdAt = (value == DateTime.MinValue) ? DateTime.Now : value;
        }
        public DateTime UpdatedAt { get; set; }
    }
}
