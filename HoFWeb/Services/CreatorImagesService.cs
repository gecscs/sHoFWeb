using System.Net.Http.Headers;
using System.Text.Json;
using HoFWeb.Models;
using Microsoft.Extensions.Options;

namespace HoFWeb.Services
{
    public class CreatorImagesService : ICreatorImagesService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ServiceSettings _settings;

        public CreatorImagesService(IHttpClientFactory httpClientFactory, IOptions<ServiceSettings> options)
        {
            _httpClientFactory = httpClientFactory;
            _settings = options.Value;
        }

        public async Task<List<ScreenshotItem>?> GetUpdatedImagesStatsAsync()
        {
            List<ScreenshotItem> shots = new List<ScreenshotItem> ();

            var fullUrl = $"{_settings.BaseUrl.TrimEnd('/')}/{_settings.CreatorImagesEndPoint.TrimStart('/')}{_settings.CreatorId}";
            var client = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, fullUrl);
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                shots = JsonSerializer.Deserialize<List<ScreenshotItem>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            }

            return shots;
        }
    }
}
