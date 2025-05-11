using HoFSimpleJSONReader.Models;

namespace HoFSimpleJSONReader.Repositories
{
    public interface IScreenshotItemRepository
    {
        Task <List<ScreenshotItem>> GetAllScreenshotsAsync();
        Task AddOrUpdateScreenshotAsync(ScreenshotItem shot);
        Task AddScreenshotDataPointAsync(ScreenshotDataPoint dataPoint);
    }
}
