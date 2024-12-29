using Diary.BLL.Services;
using Diary.BLL.Services.Abstract;
using Diary.DAL.Context;
using Diary.DAL.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Diary.WebAPI.Extentions
{
    public static class ServiceExtentions
    {
        public static void RegisterCustomServices(this IServiceCollection services)
        {
            services.AddScoped<RecordsService>();
            services.AddScoped<InvitationService>();
            services.AddScoped<JwtService>();
            services.AddScoped<UserService>();
            services.AddScoped<SessionService>();
            services.AddScoped<UserIdStorage>();
            services.AddTransient<IUserIdSetter>(s => s.GetService<UserIdStorage>());
            services.AddTransient<IUserIdGetter>(s => s.GetService<UserIdStorage>());
        }

        public static void ConfigureDB(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DiaryContext>(options =>
               options.UseSqlServer(configuration.GetConnectionString("DiaryDBConnection")));
        }
        public static void ConfigureJwt(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminPolicy", policy =>
                    policy.RequireRole(UserRoles.Admin.ToString()));
            });
        }
    }
}
