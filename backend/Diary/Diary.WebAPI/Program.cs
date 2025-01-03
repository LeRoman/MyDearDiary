using Diary.WebAPI.Extentions;
using Diary.WebAPI.Filters;
using Diary.WebAPI.Middlewares;

namespace Diary.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(
                new WebApplicationOptions { WebRootPath = "storage" });

            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<ValidateSessionFilter>();
            });
            builder.Services.AddSwaggerGen();
            builder.Services.RegisterCustomServices();
            builder.Services.ConfigureJwt(builder.Configuration);
            builder.Services.ConfigureDB(builder.Configuration);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseMiddleware<UserIdSaverMiddleware>();
            app.MapControllers();

            app.Run();
        }
    }
}
