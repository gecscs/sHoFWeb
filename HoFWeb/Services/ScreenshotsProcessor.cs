using HoFWeb.Data;
using HoFWeb.Logging;
using HoFWeb.Models;
using HoFWeb.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;

namespace HoFWeb.Services
{
    public class ScreenshotsProcessor : IScreenshotsProcessor
    {
        private readonly ILogger<ScreenshotsProcessor> _logger;
        private readonly ICustomLogger _customLogger;
        private readonly ICreatorImagesService _imagesService;
        private readonly IScreenshotItemRepository _repo;
        public ScreenshotsProcessor(ILogger<ScreenshotsProcessor> logger, ICustomLogger customLogger, ICreatorImagesService imagesService, IScreenshotItemRepository repo)
        {
            _logger = logger;
            _customLogger = customLogger;
            _imagesService = imagesService;
            _repo = repo;
        }

        public async Task<List<ScreenshotItem>> GetAllScreenshotsFromCreatorAsync(bool scheduled)
        {
            List<ScreenshotItem> dbList = await _repo.GetAllScreenshotsAsync();
            List<ScreenshotItem> updatedList = await _imagesService.GetUpdatedImagesStatsAsync();

            if (dbList != null && updatedList != null)
            {
                foreach (var updatedShot in updatedList)
                {
                    var dbShot = dbList.FirstOrDefault(c => c.Id == updatedShot.Id);

                    if (dbShot != null && !scheduled)
                    {
                        updatedShot.ViewsVariation = updatedShot.ViewsCount - dbShot.ViewsCount;
                        updatedShot.FavoritesVariation = updatedShot.FavoritesCount - dbShot.FavoritesCount;
                    }
                    else
                    {
                        updatedShot.ViewsVariation = updatedShot.ViewsCount;
                        updatedShot.FavoritesVariation = updatedShot.FavoritesCount;
                    }

                    // Correct API data
                    TimeSpan dateDifferencial = updatedShot.CreatedAt - DateTime.UtcNow;
                    double daysPast = dateDifferencial.TotalDays;

                    updatedShot.FavoritesPerDay = updatedShot.FavoritesCount / daysPast;
                    updatedShot.FavoritingPercentage = updatedShot.FavoritesCount / updatedShot.ViewsCount;
                    updatedShot.ViewsPerDay = updatedShot.ViewsCount / daysPast;

                    // Insert or Update shot data in the database
                    await _repo.AddOrUpdateScreenshotAsync(updatedShot, scheduled);

                    ScreenshotDataPoint newDataPoint = new ScreenshotDataPoint()
                    {
                        ScreenshotScreenshotDataPointId = Guid.NewGuid(),
                        Id = updatedShot.Id,
                        Views = updatedShot.ViewsCount,
                        Favorites = updatedShot.FavoritesCount,
                        CreatedAt = DateTime.UtcNow
                    };

                    await _repo.AddScreenshotDataPointAsync(newDataPoint);

                }
            }

            return updatedList.OrderByDescending(x => x.CreatedAt).ToList(); ;
        }

        public async Task<Dictionary<string, List<CanvasJsDatapoint>>> GetScreenshotDataPointsForChartCanvasJs(string id)
        {
            List<ScreenshotDataPoint> dps = await _repo.GetScreenshotDataPointsAsync(id);
            List<CanvasJsDatapoint> viewsCanvasDPs = new List<CanvasJsDatapoint>(); ;
            List<CanvasJsDatapoint> favoritesCanvasDPs = new List<CanvasJsDatapoint>();

            if (dps != null)
            {
                foreach (var dp in dps.OrderBy(c => c.CreatedAt))
                {
                    viewsCanvasDPs.Add(new CanvasJsDatapoint() { x = dp.CreatedAt, y = dp.Views });
                    favoritesCanvasDPs.Add(new CanvasJsDatapoint() { x = dp.CreatedAt, y = dp.Favorites });
                }
            }

            Dictionary<string, List<CanvasJsDatapoint>> dic = new Dictionary<string, List<CanvasJsDatapoint>>()
            {
                {"views", viewsCanvasDPs},
                {"favorites", favoritesCanvasDPs}
            };

            return dic;
        }
    }
}
