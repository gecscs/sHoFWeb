using HoFWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace HoFWeb.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<ScreenshotItem> Screenshots { get; set; }
        public DbSet<ScreenshotDataPoint> ScreenshotDataPoints { get; set; }
        public DbSet<Creator> Creators { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ScreenshotItem>()
            .HasOne(si => si.Creator)
            .WithMany(c => c.ScreenshotItems)
            .HasForeignKey(si => si.CreatorId)
            .OnDelete(DeleteBehavior.Cascade);


            //base.OnModelCreating(modelBuilder);
            // configs opcionais
        }

    }
}
