using System.Text.Json;
using System.Text.Json.Serialization;

namespace HoFWeb.Logging
{
    

    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Serilog.ILogger _logger;
        private readonly IWebHostEnvironment _env;

        public ExceptionHandlingMiddleware(RequestDelegate next,
                                           Serilog.ILogger logger,
                                           IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger.ForContext<ExceptionHandlingMiddleware>();
            _env = env;
        }

        public async Task Invoke(HttpContext context)
        {
            var traceId = Guid.NewGuid().ToString();

            try
            {
                context.Items["TraceId"] = traceId;
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Unhandled exception | TraceId: {TraceId}", traceId);

                // Verifica se é uma chamada JSON/AJAX
                var isJson = IsJsonRequest(context.Request);

                if (isJson)
                {
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";

                    var response = _env.IsDevelopment()
                        ? new ErrorResponse
                        {
                            Error = "Erro interno no servidor.",
                            TraceId = traceId,
                            Exception = ex.Message,
                            StackTrace = ex.StackTrace
                        }
                        : new ErrorResponse
                        {
                            Error = "Ocorreu um erro inesperado.",
                            TraceId = traceId
                        };

                    var options = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                    };

                    var json = JsonSerializer.Serialize(response, options);

                    await context.Response.WriteAsync(json);
                }
                else
                {
                    // Redireciona para /Error (a página Razor)
                    context.Response.Redirect($"/Error?traceId={traceId}");
                }
            }
        }

        private bool IsJsonRequest(HttpRequest request)
        {
            // Deteta se a chamada foi via API/AJAX
            var acceptHeader = request.Headers["Accept"].ToString();
            var isAjax = request.Headers["X-Requested-With"] == "XMLHttpRequest";

            return acceptHeader.Contains("application/json", StringComparison.OrdinalIgnoreCase) || isAjax;
        }
    }

}
