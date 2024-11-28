
using CommunityToolkit.Mvvm.ComponentModel;
using WorldMap.Models;

namespace WorldMap.ViewModel
{
    [QueryProperty(nameof(CountryModel), "countrymodel")]
    public partial class CountryDetailViewModel : ObservableObject
    {

        

        [ObservableProperty]
        CountryModel countryModel;

    }
}
