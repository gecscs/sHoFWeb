using System.Net.Http.Headers;
using System.Text.Json;
using HoFSimpleJSONReader.Logging;
using HoFSimpleJSONReader.Models;
using HoFSimpleJSONReader.Pages;
using Microsoft.Extensions.Options;

namespace HoFSimpleJSONReader.Services
{
    public class CreatorStatsService : ICreatorStatsService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ServiceSettings _settings;
        private readonly ILogger<CreatorStatsService> _logger;
        private readonly ICustomLogger _customLogger;

        public CreatorStatsService(IHttpClientFactory httpClientFactory, IOptions<ServiceSettings> options, ILogger<CreatorStatsService> logger, ICustomLogger customLogger)
        {
            _httpClientFactory = httpClientFactory;
            _settings = options.Value;
            _logger = logger;
            _customLogger = customLogger;
        }

        public async Task<CreatorStats?> GetCreatorStatsAsync()
        {
            var fullUrl = $"{_settings.BaseUrl.TrimEnd('/')}/{_settings.StatsEndpoint.TrimStart('/')}";
            var client = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, fullUrl);
            request.Headers.Add("Authorization", _settings.AuthorizationToken);
            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                _customLogger.CustomInfo("Error attempting to access the Creator Stats");
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<CreatorStats>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            
        }
    }
}
