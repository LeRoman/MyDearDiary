using Diary.BLL.Services.Abstract;
using Diary.DAL.Context;
using Diary.DAL.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var session = new Session()
            {
                UserId = user.Id,
                ExpiryAt = DateTime.Now.AddHours(Convert.ToDouble(_configuration["Session:Lifetime"]))
            };

            await _context.Sessions.AddAsync(session);
            await _context.SaveChangesAsync();

            return session;
        }
    }
}

