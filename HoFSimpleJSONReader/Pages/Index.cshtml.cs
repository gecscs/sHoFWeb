using HoFSimpleJSONReader.Models;
using HoFSimpleJSONReader.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;
using System.Text.Json;

namespace HoFSimpleJSONReader.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        private readonly CreatorStatsService _statsService;
        private readonly SingleImageService _imageService;
        private readonly CreatorImagesService _creatorImagesService;

        public CreatorStats? Stats { get; set; } = new CreatorStats();
        public List<ConnObj> ConnObjs { get; set; }
        public List<ScreenshotItem> Shots { get; set; }

        public String OldJsonList { get; set; } = "";
        public String NewJsonList { get; set; } = "";

        [BindProperty]
        public string OrderType { get; set; } = "mostRecent";

        public IndexModel(CreatorStatsService statsService, SingleImageService imageService, CreatorImagesService creatorImagesService, ILogger<IndexModel> logger)
        {
            _statsService = statsService;
            _imageService = imageService;
            _creatorImagesService = creatorImagesService;
            _logger = logger;
        }

        public async Task OnGetAsync()
        {
            Stats = new CreatorStats();
            Stats = await _statsService.GetCreatorStatsAsync();

            // Caminho para o ficheiro JSON
            //string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/data/data.json");
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/data/shots.json");

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true // To ignore case sensitivity
            };

            // Ler o conteúdo do ficheiro JSON
            string jsonContent = System.IO.File.ReadAllText(filePath);
            OldJsonList = jsonContent;

            // Desserializar o JSON para uma lista de ScreenshotItem
            Shots = new List<ScreenshotItem>();
            Shots = JsonSerializer.Deserialize<List<ScreenshotItem>>(jsonContent, options);

            if (Shots != null && Shots.Count > 0)
            {
                Shots.RemoveAll(x => x.Id == null);
                Shots = Shots.OrderByDescending(x => x.CreatedAt).ToList();
            }

            Shots = await _creatorImagesService.GetUpdatedImagesStatsAsync(Shots);
            Shots = Shots.OrderByDescending(x => x.CreatedAt).ToList();

            var options2 = new JsonSerializerOptions
            {
                WriteIndented = true, // Makes the JSON output formatted
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase // Converts PascalCase to camelCase
            };

            NewJsonList = JsonSerializer.Serialize(Shots, options2);

            string backupFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/data/shots_" + DateTime.UtcNow.ToString("yyyy-MM-dd_HHmmss") + ".json");

            BackupJsonAndUpdate(filePath, backupFilePath, NewJsonList);


            //// Desserializar o JSON para uma lista de RootObject
            //ConnObjs = new List<ConnObj>();
            //ConnObjs = JsonSerializer.Deserialize<List<ConnObj>>(jsonContent, options);

            //if (ConnObjs != null && ConnObjs.Count > 0)
            //{
            //    ConnObjs.RemoveAll(x => x.Body == null);
            //    //ConnObjs = ConnObjs.OrderByDescending(x => x.Body.FavoritesCount).ToList();
            //    ConnObjs = ConnObjs.OrderByDescending(x => x.Body.CreatedAt).ToList();
            //}

            //ConnObjs = await _imageService.GetUpdatedImagesStatsAsync(ConnObjs);
            //ConnObjs = ConnObjs.OrderByDescending(x => x.Body.CreatedAt).ToList();

            //var options2 = new JsonSerializerOptions
            //{
            //    WriteIndented = true, // Makes the JSON output formatted
            //    PropertyNamingPolicy = JsonNamingPolicy.CamelCase // Converts PascalCase to camelCase
            //};

            //NewJsonList = JsonSerializer.Serialize(ConnObjs, options2);

            //string backupFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/data/data_" + DateTime.UtcNow.ToString("yyyy-MM-dd_HHmmss") + ".json");

            //BackupJsonAndUpdate(filePath, backupFilePath, NewJsonList);

        }

        public async Task OnPostAsync()
        {
            Stats = new CreatorStats();
            Stats = await _statsService.GetCreatorStatsAsync();

            // Caminho para o ficheiro JSON
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/data/data.json");

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true // To ignore case sensitivity
            };

            // Ler o conteúdo do ficheiro JSON
            string jsonContent = System.IO.File.ReadAllText(filePath);
            OldJsonList = jsonContent;

            // Desserializar o JSON para uma lista de RootObject
            ConnObjs = new List<ConnObj>();
            ConnObjs = JsonSerializer.Deserialize<List<ConnObj>>(jsonContent, options);

            if (ConnObjs != null && ConnObjs.Count > 0)
            {
                ConnObjs.RemoveAll(x => x.Body == null);

                ConnObjs = await _imageService.GetUpdatedImagesStatsAsync(ConnObjs);

                switch (OrderType)
                {

                    case "mostLiked":
                        ConnObjs = ConnObjs.OrderByDescending(x => x.Body.FavoritesCount).ToList();
                        break;
                    case "mostRecent":
                        ConnObjs = ConnObjs.OrderByDescending(x => x.Body.CreatedAt).ToList();
                        break;
                    case "likesDay":
                        ConnObjs = ConnObjs.OrderByDescending(x => x.Body.FavoritesPerDay).ToList();
                        break;
                    case "likesPercent":
                        ConnObjs = ConnObjs.OrderByDescending(x => x.Body.FavoritingPercentage).ToList();
                        break;
                    case "viewsDay":
                        ConnObjs = ConnObjs.OrderByDescending(x => x.Body.ViewsPerDay).ToList();
                        break;
                    case "mostViews":
                        ConnObjs = ConnObjs.OrderByDescending(x => x.Body.ViewsCount).ToList();
                        break;
                    case "largestVariation":
                        ConnObjs = ConnObjs.OrderByDescending(x => x.Body.FavoritesVariation).ToList();
                        break;
                }

                var options2 = new JsonSerializerOptions
                {
                    WriteIndented = true, // Makes the JSON output formatted
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase // Converts PascalCase to camelCase
                };

                NewJsonList = JsonSerializer.Serialize(ConnObjs, options2);

            }

        }

        private static void BackupJsonAndUpdate(string originalFilePath, string backupFilePath, string updatedContent)
        {
            try
            {
                if (!System.IO.File.Exists(originalFilePath))
                    throw new FileNotFoundException("Original file not found.", originalFilePath);

                System.IO.File.Copy(originalFilePath, backupFilePath, overwrite: true);

                System.IO.File.WriteAllText(originalFilePath, updatedContent);
            }
            catch(Exception ex)
            {
                throw new Exception("Error while backing up and updating the JSON file: " + ex.Message);
            }

            
        }
    }
}
