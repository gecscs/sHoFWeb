using HoFSimpleJSONReader.Models;
using Microsoft.Extensions.Options;

namespace HoFSimpleJSONReader.Services
{
    public interface ICreatorImagesService
    {
        Task<List<ScreenshotItem>?> GetUpdatedImagesStatsAsync();
    }
}
