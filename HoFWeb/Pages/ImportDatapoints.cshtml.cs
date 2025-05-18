using HoFWeb.Logging;
using HoFWeb.Models;
using HoFWeb.Pages.Screenshot;
using HoFWeb.Repositories;
using HoFWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.Threading.Tasks;

namespace HoFWeb.Pages
{
    public class ConnObj
    {
        public string Id { get; set; }
        public int Status { get; set; }
        public ScreenshotItem Body { get; set; }
    }

    public class ImportDatapointsModel : PageModel
    {

        private readonly ICreatorImagesService _creatorImagesService;
        private readonly ILogger<ImportDatapointsModel> _logger;
        private readonly ICustomLogger _customLogger;
        private readonly IScreenshotsProcessor _screenshotsProcessor;
        private readonly IScreenshotItemRepository _repo;

        public string Views { get; set; } = "";
        public string Favorites { get; set; } = "";

        public ImportDatapointsModel(ICreatorImagesService creatorImagesService, ILogger<ImportDatapointsModel> logger, ICustomLogger customLogger, IScreenshotsProcessor screenshotsProcessor, IScreenshotItemRepository repo)
        {
            _creatorImagesService = creatorImagesService;
            _logger = logger;
            _customLogger = customLogger;
            _screenshotsProcessor = screenshotsProcessor;
            _repo = repo;
        }

        public async Task OnGet()
        {
            

            List<string> dataFiles = new List<string>();
            List<string> shotsFiles = new List<string>();

            //dataFiles.Add("data.json");
            dataFiles.Add("data_2025-05-01_220113.json");
            dataFiles.Add("data_2025-05-01_223530.json");
            dataFiles.Add("data_2025-05-01_223902.json");
            dataFiles.Add("data_2025-05-01_224126.json");
            dataFiles.Add("data_2025-05-01_224353.json");
            dataFiles.Add("data_2025-05-01_224717.json");
            dataFiles.Add("data_2025-05-01_224807.json");
            dataFiles.Add("data_2025-05-03_090500.json");

            shotsFiles.Add("shots.json");
            shotsFiles.Add("shots_2025-05-10_234416.json");
            shotsFiles.Add("shots_2025-05-11_072053.json");

            // Caminho para o ficheiro JSON
            string baseFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/data/");

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true // To ignore case sensitivity
            };

            foreach (string file in dataFiles)
            {
                try
                {
                    // Ler o conteúdo do ficheiro JSON
                    string jsonContent = System.IO.File.ReadAllText(baseFilePath + file);

                    string date = file.Replace("data_", "");

                    DateTime datapointDate = new DateTime();

                    if (file == "data.json")
                    {
                        datapointDate = new DateTime(2025, 5, 3, 10, 5, 0);
                    }
                    else
                    {
                        datapointDate = new DateTime(2025, 5, int.Parse(date.Substring(8, 2)), int.Parse(date.Substring(11, 2)), int.Parse(date.Substring(13, 2)), int.Parse(date.Substring(15, 2)));
                    }

                    List<ConnObj> connObjs = new List<ConnObj>();
                    connObjs = JsonSerializer.Deserialize<List<ConnObj>>(jsonContent, options);

                    if (connObjs != null && connObjs.Count > 0)
                    {
                        connObjs.RemoveAll(x => x.Body == null);                      

                        foreach (var obj in connObjs)
                        {
                            await _repo.AddScreenshotDataPointAsync(new ScreenshotDataPoint()
                            {
                                ScreenshotScreenshotDataPointId = Guid.NewGuid(),
                                Id = obj.Body.Id,
                                Views = obj.Body.ViewsCount,
                                Favorites = obj.Body.FavoritesCount,
                                CreatedAt = datapointDate
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while importing from file " + file);
                    throw;
                }


                
            }

            foreach (string file in shotsFiles)
            {
                try
                {
                    // Ler o conteúdo do ficheiro JSON
                    string jsonContent = System.IO.File.ReadAllText(baseFilePath + file);

                    string date = file.Replace("shots_", "");

                    DateTime datapointDate = new DateTime();

                    if(file == "shots.json")
                    {
                        datapointDate = new DateTime(2025, 5, 11, 8, 20, 0);
                    }
                    else
                    {
                        datapointDate = new DateTime(2025, 5, int.Parse(date.Substring(8, 2)), int.Parse(date.Substring(11, 2)), int.Parse(date.Substring(13, 2)), int.Parse(date.Substring(15, 2)));
                    }

                    List<ScreenshotItem> shots = JsonSerializer.Deserialize<List<ScreenshotItem>>(jsonContent, options);

                    if (shots != null && shots.Count > 0)
                    {
                        shots.RemoveAll(x => x.Id == null);

                        foreach (var shot in shots)
                        {
                            await _repo.AddScreenshotDataPointAsync(new ScreenshotDataPoint()
                            {
                                ScreenshotScreenshotDataPointId = Guid.NewGuid(),
                                Id = shot.Id,
                                Views = shot.ViewsCount,
                                Favorites = shot.FavoritesCount,
                                CreatedAt = datapointDate
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while importing from file " + file);
                    throw;
                }
                
            }

        }
    }
}
