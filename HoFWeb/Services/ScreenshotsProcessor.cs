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
                        updatedShot.ViewsVariation = updatedShot.UniqueViewsCount - dbShot.UniqueViewsCount;
                        updatedShot.FavoritesVariation = updatedShot.FavoritesCount - dbShot.FavoritesCount;
                    }
                    else
                    {
                        updatedShot.ViewsVariation = updatedShot.UniqueViewsCount;
                        updatedShot.FavoritesVariation = updatedShot.FavoritesCount;
                    }

                    // Correct API data
                    TimeSpan dateDifferencial = DateTime.UtcNow - updatedShot.CreatedAt;
                    double daysPast = dateDifferencial.TotalDays;

                    if(updatedShot.FavoritesCount == 0 || updatedShot.ViewsCount == 0 || updatedShot.UniqueViewsCount == 0)
                    {
                        updatedShot.FavoritesPerDay = 0;
                        updatedShot.FavoritingPercentage = 0;
                        updatedShot.ViewsPerDay = 0;
                        updatedShot.UniqueViewsCount = 0;
                    }
                    else
                    {
                        updatedShot.FavoritesPerDay = (double)updatedShot.FavoritesCount / daysPast;
                        updatedShot.FavoritingPercentage = (double)updatedShot.FavoritesCount / (double)updatedShot.UniqueViewsCount;
                        updatedShot.ViewsPerDay = (double)updatedShot.UniqueViewsCount / daysPast;
                    }

                    

                    // Insert or Update shot data in the database
                    await _repo.AddOrUpdateScreenshotAsync(updatedShot, scheduled);

                    ScreenshotDataPoint newDataPoint = new ScreenshotDataPoint()
                    {
                        ScreenshotScreenshotDataPointId = Guid.NewGuid(),
                        Id = updatedShot.Id,
                        Views = updatedShot.ViewsCount,
                        UniqueViews = updatedShot.UniqueViewsCount,
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
            List<CanvasJsDatapoint> uniqueViewsCanvasDPs = new List<CanvasJsDatapoint>(); ;
            List<CanvasJsDatapoint> favoritesCanvasDPs = new List<CanvasJsDatapoint>();

            if (dps != null)
            {
                foreach (var dp in dps.OrderBy(c => c.CreatedAt))
                {
                    viewsCanvasDPs.Add(new CanvasJsDatapoint() { x = dp.CreatedAt, y = dp.Views });
                    uniqueViewsCanvasDPs.Add(new CanvasJsDatapoint() { x = dp.CreatedAt, y = dp.UniqueViews });
                    favoritesCanvasDPs.Add(new CanvasJsDatapoint() { x = dp.CreatedAt, y = dp.Favorites });
                }
            }

            Dictionary<string, List<CanvasJsDatapoint>> dic = new Dictionary<string, List<CanvasJsDatapoint>>()
            {
                {"views", viewsCanvasDPs},
                {"uniqueViews", uniqueViewsCanvasDPs},
                {"favorites", favoritesCanvasDPs}
            };

            return dic;
        }
    }
}
