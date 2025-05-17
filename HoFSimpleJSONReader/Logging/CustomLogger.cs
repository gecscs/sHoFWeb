using Serilog;
using Serilog.Events;
using Serilog.Parsing;
using Serilog.Sinks.MSSqlServer;

namespace HoFSimpleJSONReader.Logging
{
    public class CustomLogger : ICustomLogger
    {
        private readonly Serilog.ILogger _logger;

        public CustomLogger(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("LogDbConnection");

            var sinkOptions = new MSSqlServerSinkOptions
            {
                TableName = "CustomLogs",
                AutoCreateSqlTable = true
            };

            _logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.MSSqlServer(
                    connectionString: connectionString,
                    sinkOptions: sinkOptions,
                    restrictedToMinimumLevel: LogEventLevel.Verbose
                )
                .CreateLogger();
        }

        public void CustomInfo(string message)
        {
            _logger.Information(message);
        }

        public void CustomInfo(string message, IDictionary<string, object> properties)
        {
            // Usar ForContext com chave-valor
            var contextLogger = _logger;
            foreach (var prop in properties)
            {
                contextLogger = contextLogger.ForContext(prop.Key, prop.Value);
            }

            contextLogger.Information(message);
        }
    }
}
