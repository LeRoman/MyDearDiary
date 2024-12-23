using Diary.DAL.Entities.Abstract;

namespace Diary.DAL.Entities
{
    public class Record : BaseEntity
    {
        public string Content { get; private set; }
        public User User { get; private set; }
    }
}
