using HoFSimpleJSONReader.Models;

namespace HoFSimpleJSONReader.Services
{
    public interface IScreenshotsProcessor
    {
        Task<List<ScreenshotItem>> GetAllScreenshotsFromCreatorAsync(bool scheduled);
        Task<Dictionary<string, List<CanvasJsDatapoint>>> GetScreenshotDataPointsForChartCanvasJs(string id);
    }
}
