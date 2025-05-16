using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace HoFSimpleJSONReader.Logging
{
    public class LogDbContext : DbContext
    {
        public LogDbContext(DbContextOptions<LogDbContext> options) : base(options) { }

        public DbSet<ErrorLog> ErrorLogs { get; set; }
        public DbSet<CustomLog> CustomLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define LogEntry como tipo base com chave
            modelBuilder.Entity<LogEntry>().HasKey(e => e.Id);

            // Define TPT (Table-per-Type) - cada derivada em tabela separada
            modelBuilder.Entity<ErrorLog>().ToTable("ErrorLogs");
            modelBuilder.Entity<CustomLog>().ToTable("CustomLogs");
        }
    }

    // Classe base
    public class LogEntry
    {
        [Key]
        public int Id { get; set; }
        public string Message { get; set; }
        public string MessageTemplate { get; set; }
        public string Level { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
        public string Exception { get; set; }
        public string Properties { get; set; }
        public string LogEvent { get; set; }
    }

    // Classes derivadas (entidades distintas com tabelas próprias)
    public class ErrorLog : LogEntry { }

    public class CustomLog : LogEntry { }

}
