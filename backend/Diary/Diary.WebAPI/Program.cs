using Diary.WebAPI.Extentions;
using Diary.WebAPI.Filters;
using Diary.WebAPI.Middlewares;
namespace Diary.WebAPI
{
    public partial class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(
                new WebApplicationOptions { WebRootPath = "storage" });

            var env = builder.Environment;
            if (env.IsProduction())
            {
                builder.SetProductionConfig(env);
            }

            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<ValidateSessionFilter>();
                options.Filters.Add<CustomExceptionFilterAttribute>();

            });

            builder.Services.RegisterCustomServices();
            builder.Services.ConfigureCors();
            builder.Services.ConfigureSwagger();
            builder.Services.ConfigureJwt(builder.Configuration);
            builder.Services.ConfigureDB(builder.Configuration);
            builder.Services.AddEndpointsApiExplorer();

            var app = builder.Build();

            Database.Migrate(app);
            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI();

            app.Use(async (context, next) =>
            {
                Console.WriteLine($"Request: {context.Request.Method} {context.Request.Path}");
                await next();
                Console.WriteLine($"Response: {context.Response.StatusCode}");
            });

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors("AllowAll");
            app.UseFileServer();
            app.UseMiddleware<UserIdSaverMiddleware>();
            app.MapControllers();


            app.Run();
        }


    }

}

