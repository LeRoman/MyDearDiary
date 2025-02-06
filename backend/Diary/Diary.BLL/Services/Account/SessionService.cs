using Diary.BLL.Services.Abstract;
using Diary.BLL.Services.Interfaces;
using Diary.DAL.Context;
using Diary.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Microsoft.Extensions.Configuration;

namespace Diary.BLL.Services.Account
{
    public class SessionService : BaseService, ISessionService
    {
        private readonly IConfiguration _configuration;
        private readonly UserIdStorage _userIdStorage;

        public SessionService(DiaryContext diaryContext, IConfiguration configuration, UserIdStorage userIdStorage) : base(diaryContext)
        {
            _configuration = configuration;
            _userIdStorage = userIdStorage;
        }


        public async Task<Session> CreateSessionAsync(User user)
        {
            double sessionLifeTimeHours = 120;
            if (double.TryParse(_configuration["Session:LifeTimeHours"], out double result))
                sessionLifeTimeHours = result;

            var session = new Session()
            {
                UserId = user.Id,
                ExpiryAt = DateTime.Now.AddHours(sessionLifeTimeHours)
            };

            await _context.Sessions.AddAsync(session);
            await _context.SaveChangesAsync();

            return session;
        }

        public async Task RevokeSessionsAsync()
        {
            var userId = Guid.Parse(_userIdStorage.CurrentUserId);
            var sessions = await _context.Sessions
                 .Where(s => s.UserId == userId && s.IsRevoked == false)
                 .ToListAsync();

            foreach (var session in sessions)
            {
                session.IsRevoked = true;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<Session?> GetActiveSession(User user)
        {
            var session = await _context.Sessions.FirstOrDefaultAsync(x => x.UserId == user.Id);
            if (session != null&!session!.IsRevoked && session.ExpiryAt > DateTime.Now)
            {
                return session;
            }
            return null;
        }
    }
}
