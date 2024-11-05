
using WorldMap.Models;

namespace WorldMap.Services
{
    public interface IDataService
    {
        Task<List<CountryModel>> LoadJsonDataAsync();
    }
}
