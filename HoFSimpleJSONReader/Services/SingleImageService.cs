using System.Net.Http.Headers;
using System.Text.Json;
using HoFSimpleJSONReader.Models;
using Microsoft.Extensions.Options;

namespace HoFSimpleJSONReader.Services
{
    public class SingleImageService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ServiceSettings _settings;

        public SingleImageService(IHttpClientFactory httpClientFactory, IOptions<ServiceSettings> options)
        {
            _httpClientFactory = httpClientFactory;
            _settings = options.Value;
        }

        public async Task<List<ConnObj>?> GetUpdatedImagesStatsAsync(List<ConnObj> baseList)
        {
            List<ConnObj> refreshedList = baseList.ToList();

            if (baseList.Count > 0)
            {
                foreach (ConnObj connObj in baseList) 
                {
                    
                    var fullUrl = $"{_settings.BaseUrl.TrimEnd('/')}/{_settings.SingleImageEndPoint.TrimStart('/')}";
                    var client = _httpClientFactory.CreateClient();
                    var request = new HttpRequestMessage(HttpMethod.Get, fullUrl + "/" + connObj.Body.Id);
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
                            shot.FavoritesVariation = shot.FavoritesCount - connObj.Body.FavoritesCount;
                            shot.ViewsVariation = shot.ViewsCount - connObj.Body.ViewsCount;
                            ConnObj oldItem = refreshedList.First(c => c.Id == connObj.Body.Id);
                            ConnObj updatedItem = oldItem;
                            updatedItem.Body = shot;
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
