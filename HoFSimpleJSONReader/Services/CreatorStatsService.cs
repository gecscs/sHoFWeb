using System.Net.Http.Headers;
using System.Text.Json;
using HoFSimpleJSONReader.Models;
using Microsoft.Extensions.Options;

namespace HoFSimpleJSONReader.Services
{
    public class CreatorStatsService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ServiceSettings _settings;

        public CreatorStatsService(IHttpClientFactory httpClientFactory, IOptions<ServiceSettings> options)
        {
            _httpClientFactory = httpClientFactory;
            _settings = options.Value;
        }

        public async Task<CreatorStats?> GetCreatorStatsAsync()
        {
            var fullUrl = $"{_settings.BaseUrl.TrimEnd('/')}/{_settings.StatsEndpoint.TrimStart('/')}";
            var client = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, fullUrl);
            request.Headers.Add("Authorization", _settings.AuthorizationToken);

            //var request = new HttpRequestMessage(HttpMethod.Get, fullUrl);
            //request.Headers.Add("Authorization", _settings.AuthorizationToken);

            var response = await client.SendAsync(request);
            //var content = await response.Content.ReadAsStringAsync();


            //var client = _httpClientFactory.CreateClient();


            //var fullUrl = $"{_settings.BaseUrl.TrimEnd('/')}/{_settings.StatsEndpoint.TrimStart('/')}";
            //var response = await client.GetAsync(fullUrl);
            //client.BaseAddress = new Uri(_settings.BaseUrl);
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", _settings.AuthorizationToken);
            //client.DefaultRequestHeaders.Add("Authorization", _settings.AuthorizationToken);

            //var response = await client.GetAsync(_settings.StatsEndpoint);
            if (!response.IsSuccessStatusCode)
                return null;

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<CreatorStats>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            
        }
    }
}
