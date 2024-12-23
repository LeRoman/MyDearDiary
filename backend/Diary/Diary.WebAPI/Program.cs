using Diary.DAL.Context;
using Diary.BLL;
using Microsoft.EntityFrameworkCore;
using Diary.BLL.Services;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Diary.DAL.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Diary.WebAPI.Extentions;

namespace Diary.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.RegisterCustomServices();
            builder.Services.ConfigureJwt(builder.Configuration);

            builder.Services.AddDbContext<DiaryContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DiaryDBConnection")));
           
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseAuthentication();
            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
