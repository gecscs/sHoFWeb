﻿using HoFSimpleJSONReader.Data;
using HoFSimpleJSONReader.Logging;
using HoFSimpleJSONReader.Models;
using HoFSimpleJSONReader.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;

namespace HoFSimpleJSONReader.Services
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

        public async Task<List<ScreenshotItem>> GetAllScreenshotsFromCreatorAsync()
        {
            List<ScreenshotItem> dbList = await _repo.GetAllScreenshotsAsync();
            List<ScreenshotItem> updatedList = await _imagesService.GetUpdatedImagesStats2Async();

            if (dbList != null && updatedList != null)
            {
                foreach (var updatedShot in updatedList)
                {
                    var dbShot = dbList.FirstOrDefault(c => c.Id == updatedShot.Id);

                    if (dbShot != null)
                    {
                        updatedShot.ViewsVariation = updatedShot.ViewsCount - dbShot.ViewsCount;
                        updatedShot.FavoritesVariation = updatedShot.FavoritesCount - dbShot.FavoritesCount;
                    }

                    await _repo.AddOrUpdateScreenshotAsync(updatedShot);

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
                foreach (var dp in dps)
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
