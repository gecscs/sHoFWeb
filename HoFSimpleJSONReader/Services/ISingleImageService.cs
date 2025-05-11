using HoFSimpleJSONReader.Models;

namespace HoFSimpleJSONReader.Services
{
    public interface ISingleImageService
    {
        Task<ScreenshotItem?> GetImageStatsAsync(string id);
    }
}
