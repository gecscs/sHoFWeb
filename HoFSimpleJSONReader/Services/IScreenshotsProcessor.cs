using HoFSimpleJSONReader.Models;

namespace HoFSimpleJSONReader.Services
{
    public interface IScreenshotsProcessor
    {
        Task<List<ScreenshotItem>> GetAllScreenshotsFromCreatorAsync();
        Task<Dictionary<string, List<CanvasJsDatapoint>>> GetScreenshotDataPointsForChartCanvasJs(string id);
    }
}
