using Diary.BLL.Services.Abstract;
using Diary.DAL.Context;
using Diary.DAL.Entities;
using Microsoft.Extensions.Configuration;

namespace Diary.BLL.Services
{
    public class SessionService : BaseService
    {
        private readonly IConfiguration _configuration;

        public SessionService(DiaryContext diaryContext, IConfiguration configuration) : base(diaryContext)
        {
            _configuration = configuration;
        }
        public async Task<Session> CreateSession(User user)
        {
            double sessionLifeTime = Convert.ToDouble(_configuration["Session:LifeTimeHours"]);
            var session = new Session()
            {
                UserId = user.Id,
                ExpiryAt = DateTime.Now.AddHours(sessionLifeTime)
            };

            await _context.Sessions.AddAsync(session);
            await _context.SaveChangesAsync();

            return session;
        }
    }
}

