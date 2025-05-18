using System.Net.Http.Headers;
using System.Text.Json;
using HoFWeb.Models;
using Microsoft.Extensions.Options;

namespace HoFWeb.Services
{
    public class SingleImageService : ISingleImageService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ServiceSettings _settings;

        public SingleImageService(IHttpClientFactory httpClientFactory, IOptions<ServiceSettings> options)
        {
            _httpClientFactory = httpClientFactory;
            _settings = options.Value;
        }

        public async Task<ScreenshotItem?> GetImageStatsAsync(string id)
        {
            ScreenshotItem shot = new ScreenshotItem();

            var fullUrl = $"{_settings.BaseUrl.TrimEnd('/')}/{_settings.SingleImageEndPoint.TrimStart('/')}";
            var client = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, fullUrl + "/" + id);
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                
                shot = JsonSerializer.Deserialize<ScreenshotItem>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                                
            }

            return shot;
        }
    }
}
