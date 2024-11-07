
using System.Text.Json;
using WorldMap.Models;

namespace WorldMap.Services
{
    public class DataServices : IDataService
    {
        
        public async Task<List<CountryModel>> LoadJsonDataAsync()
        {
            try
            {
                // Get the path to Data folder
             using var stream = await FileSystem.OpenAppPackageFileAsync("data.json");
               
             using var reader = new StreamReader(stream);
             var jsonContent = await reader.ReadToEndAsync();

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
