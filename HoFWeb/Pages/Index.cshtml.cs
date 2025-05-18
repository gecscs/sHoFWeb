using HoFWeb.Logging;
using HoFWeb.Models;
using HoFWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;
using System.Text.Json;

namespace HoFWeb.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ICreatorStatsService _statsService;
        private readonly ICreatorImagesService _creatorImagesService;
        private readonly ILogger<IndexModel> _logger;
        private readonly ICustomLogger _customLogger;
        private readonly IScreenshotsProcessor _screenshotsProcessor;

        public CreatorStats? Stats { get; set; } = new CreatorStats();
        public List<ScreenshotItem> Shots { get; set; }

        [BindProperty]
        public string OrderType { get; set; } = "mostRecent";

        public IndexModel(ICreatorStatsService statsService, ICreatorImagesService creatorImagesService, ILogger<IndexModel> logger, ICustomLogger customLogger, IScreenshotsProcessor screenshotsProcessor )
        {
            _statsService = statsService;
            _creatorImagesService = creatorImagesService;
            _logger = logger;
            _customLogger = customLogger;
            _screenshotsProcessor = screenshotsProcessor;
        }

        public async Task OnGetAsync(bool scheduled = false)
        {

            try
            {
                //_customLogger.CustomInfo("This is a test");

                Stats = new CreatorStats();
                
                Stats = await _statsService.GetCreatorStatsAsync();

            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Erro ao chamar os stats");
                throw;
            }

            try
            {
                Shots = await _screenshotsProcessor.GetAllScreenshotsFromCreatorAsync(scheduled);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Erro ao chamar os screenshots");
                throw;
            }

        }

        public async Task<JsonResult> OnGetGetShotDataPoints(string id)
        {
            Dictionary<string, List<CanvasJsDatapoint>> dic = await _screenshotsProcessor.GetScreenshotDataPointsForChartCanvasJs(id);
            return new JsonResult(dic);
        }
    }
}
