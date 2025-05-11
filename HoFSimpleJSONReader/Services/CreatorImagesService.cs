using System.Net.Http.Headers;
using System.Text.Json;
using HoFSimpleJSONReader.Models;
using Microsoft.Extensions.Options;

namespace HoFSimpleJSONReader.Services
{
    public class CreatorImagesService
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
                foreach (ScreenshotItem baseShot in baseList) 
                {
                    
                    var fullUrl = $"{_settings.BaseUrl.TrimEnd('/')}/{_settings.CreatorImagesEndPoint.TrimStart('/')}";
                    var client = _httpClientFactory.CreateClient();
                    var request = new HttpRequestMessage(HttpMethod.Get, fullUrl + "/" + baseShot.Id);
                    var response = await client.SendAsync(request);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        ScreenshotItem shot = new ScreenshotItem();
                        shot = JsonSerializer.Deserialize<ScreenshotItem>(content, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                        if (shot != null)
                        {
                            shot.FavoritesVariation = shot.FavoritesCount - baseShot.FavoritesCount;
                            shot.ViewsVariation = shot.ViewsCount - baseShot.ViewsCount;
                            ScreenshotItem oldItem = refreshedList.First(c => c.Id == baseShot.Id);
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
    }
}
