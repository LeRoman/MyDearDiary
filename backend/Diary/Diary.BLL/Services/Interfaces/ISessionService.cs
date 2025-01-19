using Diary.DAL.Entities;

namespace Diary.BLL.Services.Interfaces
{
    public interface ISessionService
    {
        Task<Session> CreateSessionAsync(User user);
        Task RevokeSessionsAsync();
    }
}