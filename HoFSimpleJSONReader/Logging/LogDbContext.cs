using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HoFSimpleJSONReader.Logging
{
    // DbContext
    public class LogDbContext : DbContext
    {
        public LogDbContext(DbContextOptions<LogDbContext> options) : base(options) { }

        public DbSet<ErrorLog> ErrorLogs { get; set; }
        public DbSet<CustomLog> CustomLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ErrorLog>().ToTable("ErrorLogs");
            modelBuilder.Entity<CustomLog>().ToTable("CustomLogs");
        }
    }

    // Logs separados, sem herança
    


}
