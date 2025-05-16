using HoFSimpleJSONReader.Models;

namespace HoFSimpleJSONReader.Services
{
    public interface ICreatorStatsService
    {
        Task<CreatorStats?> GetCreatorStatsAsync();
    }
}
