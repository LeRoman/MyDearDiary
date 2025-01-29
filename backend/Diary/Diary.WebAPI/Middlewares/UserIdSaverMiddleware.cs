using Diary.BLL.Services.Abstract;

namespace Diary.WebAPI.Middlewares
{
    public class UserIdSaverMiddleware
    {
        private readonly RequestDelegate _next;

        public UserIdSaverMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IUserIdSetter userIdSetter)
        {
            var claimsUserId = context.User.Claims.FirstOrDefault(x => x.Type == "UserId")?.Value;
            if (claimsUserId != null)
            {
                userIdSetter.SetUserId(claimsUserId);
            }

            await _next.Invoke(context);
        }
    }
}
