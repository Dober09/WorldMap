
using System.Text.Json;
using WorldMap.Models;

namespace WorldMap.Services
{
    public class DataServices : IDataService
    {
        private readonly string _jsonFilePath;

        public DataServices(string jsonFilePath = "") { 
             _jsonFilePath = jsonFilePath;
        }
        public async Task<List<CountryModel>> LoadJsonDataAsync()
        {
            try
            {
                // Get the path to Data folder
                string basePath = AppDomain.CurrentDomain.BaseDirectory;
                string dataFolderPath = Path.Combine(basePath, "Data");
                string jsonFilePath = Path.Combine(dataFolderPath, "data.json");

                //Read the JSON file
                string jsonContent = await File.ReadAllTextAsync(jsonFilePath);

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };

                return JsonSerializer.Deserialize<List<CountryModel>>(jsonContent,options);

            }catch (Exception ex)
            {
                //Handle or log the error oppropriately
                System.Diagnostics.Debug.WriteLine($"Error loading JSON: {ex.Message}");
                return new List<CountryModel>();
            }
        }
    }
}
