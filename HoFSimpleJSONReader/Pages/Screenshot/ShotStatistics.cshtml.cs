using HoFSimpleJSONReader.Logging;
using HoFSimpleJSONReader.Models;
using HoFSimpleJSONReader.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.Threading.Tasks;

namespace HoFSimpleJSONReader.Pages.Screenshot
{

    public class ShotStatisticsModel : PageModel
    {

        private readonly ICreatorImagesService _creatorImagesService;
        private readonly ILogger<ShotStatisticsModel> _logger;
        private readonly ICustomLogger _customLogger;
        private readonly IScreenshotsProcessor _screenshotsProcessor;

        public string Views { get; set; } = "";
        public string Favorites { get; set; } = "";

        public ShotStatisticsModel(ICreatorImagesService creatorImagesService, ILogger<ShotStatisticsModel> logger, ICustomLogger customLogger, IScreenshotsProcessor screenshotsProcessor)
        {
            _creatorImagesService = creatorImagesService;
            _logger = logger;
            _customLogger = customLogger;
            _screenshotsProcessor = screenshotsProcessor;
        }

        public async Task OnGetAsync(string ScreenshotId)
        {
            if (string.IsNullOrWhiteSpace(ScreenshotId)) 
            {
                _customLogger.CustomInfo("No screenshot id was provided");
                throw new ArgumentNullException(nameof(ScreenshotId));
            }

            Dictionary<string, List<CanvasJsDatapoint>> dic = await _screenshotsProcessor.GetScreenshotDataPointsForChartCanvasJs(ScreenshotId);

            if( dic.TryGetValue("views", out List<CanvasJsDatapoint> _views)){
                Views = JsonSerializer.Serialize(_views, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            if (dic.TryGetValue("favorites", out List<CanvasJsDatapoint> _favorites)){
                Favorites = JsonSerializer.Serialize(_favorites, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
        }
    }
}
