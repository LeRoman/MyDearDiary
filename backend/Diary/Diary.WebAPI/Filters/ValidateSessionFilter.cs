using Diary.BLL.Helper;
using Diary.DAL.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

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
            var endpoint = context.HttpContext.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<AllowAnonymousAttribute>() != null)
            {
                await next();
                return;
            }

            var user = context.HttpContext.User;
            var userStatus = user.FindFirst("Status")?.Value;
            var expTokenDate = SecurityHelper.GetJwtExpirationDate(user);



            if (user.Identity?.IsAuthenticated == true)
            {
                if (expTokenDate < DateTime.Now)
                {
                    context.Result = new ObjectResult(
                        new
                        {
                            message = "token-expired"
                        })
                    {
                        StatusCode = StatusCodes.Status401Unauthorized
                    };
                    return;
                }


                var sessionId = user.FindFirst("SessionId")?.Value;
                if (!string.IsNullOrEmpty(sessionId))
                {
                    var session = await _dbContext.Sessions.FirstOrDefaultAsync(s => s.Id == Guid.Parse(sessionId));
                    if (session == null || session.IsRevoked || session.ExpiryAt <= DateTime.Now)
                    {
                        context.Result = new ObjectResult(
                            new
                            {
                                message = "session-expired"
                            })
                        {
                            StatusCode = StatusCodes.Status401Unauthorized
                        };
                        return;
                    }
                }
                else
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }

            }

            await next();
        }
    }
}
