using HoFSimpleJSONReader.Data;
using HoFSimpleJSONReader.Logging;
using HoFSimpleJSONReader.Models;
using HoFSimpleJSONReader.Services;
using Microsoft.EntityFrameworkCore;

namespace HoFSimpleJSONReader.Repositories
{
    public class ScreenshotItemRepository : IScreenshotItemRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ScreenshotItemRepository> _logger;
        private readonly ICustomLogger _customLogger;
        public ScreenshotItemRepository(AppDbContext context, ILogger<ScreenshotItemRepository> logger, ICustomLogger customLogger) 
        {
            _context = context;
            _logger = logger;
            _customLogger = customLogger;
        }

        public async Task <List<ScreenshotItem>> GetAllScreenshotsAsync()
        {
            return await _context.Screenshots.ToListAsync();
        }

        public async Task AddOrUpdateScreenshotAsync(ScreenshotItem shot, bool scheduled)
        {
            var s = await _context.Screenshots.FirstOrDefaultAsync(c => c.Id == shot.Id);

            if (!scheduled)
            {
                if (s != null)
                {
                    s.Favorited = shot.Favorited;
                    s.FavoritesCount = shot.FavoritesCount;
                    s.FavoritesPerDay = shot.FavoritesPerDay;
                    s.FavoritingPercentage = shot.FavoritingPercentage;
                    s.ViewsCount = shot.ViewsCount;
                    s.ViewsPerDay = shot.ViewsPerDay;
                    s.IsReported = shot.IsReported;
                    s.IsApproved = shot.IsApproved;
                }
                else
                {
                    // Handle potential duplicate Creator
                    if (shot.Creator != null)
                    {
                        var existingCreator = await _context.Creators.FindAsync(shot.Creator.Id);
                        if (existingCreator != null)
                        {
                            // Attach existing Creator (reuse tracked one)
                            shot.Creator = existingCreator;
                        }
                        else
                        {
                            // Insert new Creator
                            _context.Creators.Add(shot.Creator);
                        }
                    }

                    _context.Screenshots.Add(shot);
                }

                await _context.SaveChangesAsync();
            }
            else
            {
                if (s == null)
                {
                    // Handle potential duplicate Creator
                    if (shot.Creator != null)
                    {
                        var existingCreator = await _context.Creators.FindAsync(shot.Creator.Id);
                        if (existingCreator != null)
                        {
                            // Attach existing Creator (reuse tracked one)
                            shot.Creator = existingCreator;
                        }
                        else
                        {
                            // Insert new Creator
                            _context.Creators.Add(shot.Creator);
                        }
                    }

                    _context.Screenshots.Add(shot);
                }
            }

            
        }

        public async Task AddScreenshotDataPointAsync(ScreenshotDataPoint dataPoint)
        {
            _context.ScreenshotDataPoints.Add(dataPoint);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ScreenshotDataPoint>> GetScreenshotDataPointsAsync(string id)
        {
            return await _context.ScreenshotDataPoints.Where(c => c.Id == id).OrderBy(c => c.CreatedAt).ToListAsync();
        }

    }
}
