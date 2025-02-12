using Diary.DAL.Context;
using Diary.DAL.Entities;
using Diary.DAL.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Diary.WebAPI
{
    public class Database
    {
        public static void Migrate(WebApplication app)
        {
            using (var container = app.Services.CreateScope())
            {
                var dbContext = container.ServiceProvider.GetService<DiaryContext>();
                var pendingMigration = dbContext!.Database.GetPendingMigrations();
                if (pendingMigration.Any())
                {
                    dbContext.Database.Migrate();
                }

                var existingAdmin = dbContext.Users.FirstOrDefault(x => x.Role == UserRoles.Admin);

                if (existingAdmin == null)
                {
                    var configuration = container.ServiceProvider.GetRequiredService<IConfiguration>();
                    var adminEmail = configuration["Admin:Email"];
                    var adminPassword = configuration["Admin:Password"];
                    var admin = new User()
                    {
                        Nickname = "Administrator",
                        Email = adminEmail,
                        Role = UserRoles.Admin,
                    };

                    var passHash = new PasswordHasher<User>().HashPassword(admin, adminPassword!);
                    admin.PasswordHash = passHash;
                    dbContext.Users.Add(admin);
                    dbContext.SaveChanges();
                }
            }
        }
    }
}