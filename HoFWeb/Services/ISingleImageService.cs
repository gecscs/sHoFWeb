using HoFWeb.Models;

namespace HoFWeb.Services
{
    public interface ISingleImageService
    {
        Task<ScreenshotItem?> GetImageStatsAsync(string id);
    }
}
