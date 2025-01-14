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
                options.Filters.Add<CustomExceptionFilterAttribute>();

            });
            builder.Services.AddSwaggerGen();
            builder.Services.RegisterCustomServices();
            builder.Services.ConfigureJwt(builder.Configuration);
            builder.Services.ConfigureDB(builder.Configuration);
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                builder => builder.WithOrigins("http://localhost:4200")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors("AllowSpecificOrigin");

            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseMiddleware<UserIdSaverMiddleware>();
            app.MapControllers();

            app.Run();
        }
    }
}
