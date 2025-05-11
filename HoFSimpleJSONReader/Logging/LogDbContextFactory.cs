using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace HoFSimpleJSONReader.Logging
{
    public class LogDbContextFactory : IDesignTimeDbContextFactory<LogDbContext>
    {
        public LogDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // importante para localizar o appsettings.json
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<LogDbContext>();
            var connectionString = configuration.GetConnectionString("LogDbConnection");

            optionsBuilder.UseSqlServer(connectionString);

            return new LogDbContext(optionsBuilder.Options);
        }
    }
}
