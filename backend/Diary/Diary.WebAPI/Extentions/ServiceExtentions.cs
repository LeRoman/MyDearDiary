﻿using Diary.BLL.Services;
using Diary.BLL.Services.Abstract;
using Diary.BLL.Services.Account;
using Diary.BLL.Services.BackgroundServices;
using Diary.DAL.Context;
using Diary.DAL.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

namespace Diary.WebAPI.Extentions
{
    public static class ServiceExtentions
    {
        public static void RegisterCustomServices(this IServiceCollection services)
        {
            services.AddSingleton<AesEncryptionService>();
            services.AddScoped<RecordsService>();
            services.AddScoped<InvitationService>();
            services.AddScoped<JwtService>();
            services.AddScoped<UserService>();
            services.AddScoped<SessionService>();
            services.AddTransient<ImageService>();
            services.AddTransient<FileStorageService>();
            services.AddHostedService<AccountDeletionService>();
            services.AddScoped<EmailService>();
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
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "MyDiary",
                    Description = "An ASP.NET Core Web API for personal diary"
                });
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });
        }

        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                builder => builder.WithOrigins("http://localhost:4200")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
            });
        }
    }
}
