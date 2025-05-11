using Serilog;
using Serilog.Events;
using Serilog.Parsing;

namespace HoFSimpleJSONReader.Logging
{
    public class CustomLogger : ICustomLogger
    {
        private readonly Serilog.ILogger _logger;


        public CustomLogger(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("LogDbConnection");

            _logger = new LoggerConfiguration()
                .WriteTo.MSSqlServer(
                    connectionString: connectionString,
                    tableName: "CustomLogs",
                    autoCreateSqlTable: true,
                    restrictedToMinimumLevel: LogEventLevel.Information
                )
                .CreateLogger();
        }

        public void CustomInfo(string message)
        {
            _logger.Information(message);
        }

        public void CustomInfo(string message, IDictionary<string, object> properties)
        {
            var logEvent = new LogEvent(DateTimeOffset.Now, LogEventLevel.Information,
                null,
                new MessageTemplate(message, new List<MessageTemplateToken>()),
                properties.Select(p => new LogEventProperty(p.Key, new ScalarValue(p.Value)))
            );

            _logger.Write(logEvent);
        }

    }
}
