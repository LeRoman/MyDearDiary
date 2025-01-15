namespace Diary.BLL.DTO
{
    public class InvitationDTO
    {
        public string Email { get; set; }

        public double ExpirationHours { get; set; } = 5;
    }
}
