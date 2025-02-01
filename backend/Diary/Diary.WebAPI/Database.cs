using Diary.DAL.Context;
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
            }
        }
    }
}
