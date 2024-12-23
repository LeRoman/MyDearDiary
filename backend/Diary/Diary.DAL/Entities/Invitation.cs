using Diary.DAL.Entities.Abstract;

namespace Diary.DAL.Entities
{
    public class Invitation : BaseEntity
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public bool IsUsed { get; set; }
    }
}
