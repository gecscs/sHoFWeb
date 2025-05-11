using System.Net.Http.Headers;
using System.Text.Json;
using HoFSimpleJSONReader.Models;
using Microsoft.Extensions.Options;

namespace HoFSimpleJSONReader.Services
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

        public async Task<List<ScreenshotItem>?> GetUpdatedImagesStatsAsync(List<ScreenshotItem> baseList)
        {
            List<ScreenshotItem> refreshedList = baseList.ToList();

            if (baseList.Count > 0)
            {
                var fullUrl = $"{_settings.BaseUrl.TrimEnd('/')}/{_settings.CreatorImagesEndPoint.TrimStart('/')}";
                var client = _httpClientFactory.CreateClient();
                var request = new HttpRequestMessage(HttpMethod.Get, fullUrl);
                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();

                    List<ScreenshotItem> shots = new List<ScreenshotItem>();
                    shots = JsonSerializer.Deserialize<List<ScreenshotItem>>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (shots != null)
                    {
                        foreach (var shot in shots)
                        {
                            ScreenshotItem oldItem = refreshedList.First(c => c.Id == shot.Id);

                            shot.FavoritesVariation = shot.FavoritesCount - oldItem.FavoritesCount;
                            shot.ViewsVariation = shot.ViewsCount - oldItem.ViewsCount;

                            ScreenshotItem updatedItem = oldItem;

                            updatedItem = shot;
                            refreshedList.Remove(oldItem);
                            refreshedList.Add(updatedItem);
                        }
                    }
                }

            }

            return refreshedList;
        }

        public async Task<List<ScreenshotItem>?> GetUpdatedImagesStats2Async()
        {
            List<ScreenshotItem> shots = new List<ScreenshotItem> ();

            var fullUrl = $"{_settings.BaseUrl.TrimEnd('/')}/{_settings.CreatorImagesEndPoint.TrimStart('/')}";
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
