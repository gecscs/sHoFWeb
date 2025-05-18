using HoFWeb.Models;

namespace HoFWeb.Services
{
    public interface ICreatorStatsService
    {
        Task<CreatorStats?> GetCreatorStatsAsync();
    }
}
