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
                .Property(r => r.Content)
                .HasMaxLength(500)
                .IsRequired();
        }
    }
}
