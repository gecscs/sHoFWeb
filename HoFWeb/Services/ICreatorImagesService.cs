using HoFWeb.Models;
using Microsoft.Extensions.Options;

namespace HoFWeb.Services
{
    public interface ICreatorImagesService
    {
        Task<List<ScreenshotItem>?> GetUpdatedImagesStatsAsync();
    }
}
