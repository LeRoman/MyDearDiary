using Diary.DAL.Entities;

namespace Diary.BLL.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateJwtToken(User user, Session session);
    }
}