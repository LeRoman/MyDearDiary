using Diary.DAL.Entities.Abstract;
using Diary.DAL.Enums;

namespace Diary.DAL.Entities
{
    public class User : BaseEntity
    {
        public string? Nickname { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public UserRoles Role { get; set; }
        public List<Record>? Records { get; set; }
        public AccountStatus Status { get; set; }
        public DateTime MarkedForDeletionAt { get; set; }
    }
}
