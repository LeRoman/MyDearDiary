using Diary.DAL.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Diary.WebAPI.Filters
{
    public class ValidateSessionFilter : IAsyncActionFilter
    {
        private readonly DiaryContext _dbContext;

        public ValidateSessionFilter(DiaryContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var user = context.HttpContext.User;
            if (user.Identity?.IsAuthenticated == true)
            {
                var sessionId = user.FindFirst("SessionId")?.Value;
                if (!string.IsNullOrEmpty(sessionId))
                {
                    var session = await _dbContext.Sessions.FindAsync(Guid.Parse(sessionId));
                    if (session == null || session.IsRevoked || session.ExpiryAt <= DateTime.Now)
                    {
                        context.Result = new UnauthorizedResult();
                        return;
                    }
                }
            }

            await next();
        }
    }
}
