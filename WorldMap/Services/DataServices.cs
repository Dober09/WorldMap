using System.Text.Json;
using WorldMap.Models;

namespace WorldMap.Services
{
    public class DataServices : IDataService
    {
        List<CountryModel> countries = new List<CountryModel>();
        public async Task<List<CountryModel>> LoadJsonDataAsync()
        {
            if (countries?.Count > 0) { return countries; }
            using var stream = await FileSystem.OpenAppPackageFileAsync("data.json");
            using var reader = new StreamReader(stream);
            var content = await reader.ReadToEndAsync();
            countries = JsonSerializer.Deserialize<List<CountryModel>>(content);

            return countries;
            //try
            //{
            //    // Get the path to Data folder
            // using var stream = await FileSystem.OpenAppPackageFileAsync("data.json");
            // using var reader = new StreamReader(stream);
            // var jsonContent = await reader.ReadToEndAsync();

            //    var options = new JsonSerializerOptions
            //    {
            //        PropertyNameCaseInsensitive = true,
            //    };


            //    var result = JsonSerializer.Deserialize<List<CountryModel>>(jsonContent,options);
            //    var jsonDoc = JsonDocument.Parse(jsonContent);
            //    var rootElement = jsonDoc.RootElement;

            //    if (rootElement.ValueKind == JsonValueKind.Array)
            //    {
            //        var countries = JsonSerializer.Deserialize<List<CountryModel>>(jsonContent,options);
            //        System.Diagnostics.Debug.WriteLine($"Deserialed {countries?.Count ?? 0} countries");
            //        Console.Write(countries);
            //        return countries ?? new List<CountryModel>();
            //    }
            //    else
            //    {
            //        System.Diagnostics.Debug.WriteLine("JSON root is not an array");
            //        return new List<CountryModel>();
            //    }

            //}
            //catch (Exception ex)
            //{
            //    //Handle or log the error oppropriately
            //    System.Diagnostics.Debug.WriteLine($"Error loading JSON: {ex.Message}");
            //    return new List<CountryModel>();
            //}
        }
    }
}
