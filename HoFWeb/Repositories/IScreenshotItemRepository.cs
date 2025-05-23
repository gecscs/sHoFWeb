using HoFWeb.Models;

namespace HoFWeb.Repositories
{
    public interface IScreenshotItemRepository
    {
        Task <List<ScreenshotItem>> GetAllScreenshotsAsync();
        Task AddOrUpdateScreenshotAsync(ScreenshotItem shot, bool scheduled);
        Task AddScreenshotDataPointAsync(ScreenshotDataPoint dataPoint);
        Task<List<ScreenshotDataPoint>> GetScreenshotDataPointsAsync(string id);
    }
}
