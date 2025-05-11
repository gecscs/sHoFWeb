using System.Text.Json;

namespace HoFSimpleJSONReader.Logging
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public ExceptionHandlingMiddleware(RequestDelegate next,
                                           ILogger<ExceptionHandlingMiddleware> logger,
                                           IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task Invoke(HttpContext context)
        {
            var traceId = Guid.NewGuid().ToString(); // opcional, mas útil

            try
            {
                context.Items["TraceId"] = traceId;
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception | TraceId: {TraceId}", traceId);

                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                object response;
                if (_env.IsDevelopment())
                {
                    response = (new
                    {
                        error = "Erro interno no servidor.",
                        traceId,
                        exception = ex.Message,
                        stackTrace = ex.StackTrace
                    });
                }
                else
                {
                    response = (new
                    {
                        error = "Ocorreu um erro inesperado.",
                        traceId
                    });
                }

                var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                await context.Response.WriteAsync(json);
            }
        }
    }
}
