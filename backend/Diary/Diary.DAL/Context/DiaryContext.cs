using Diary.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Diary.DAL.Context
{
    public class DiaryContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Record> Records { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Invitation> Invitations { get; set; }

        public DiaryContext(DbContextOptions<DiaryContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Record>()
                .HasMany(e => e.Images)
                .WithOne(i => i.Record)
                .HasForeignKey(k => k.RecordId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Records)
                .WithOne(i => i.User)
                .HasForeignKey(k => k.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
