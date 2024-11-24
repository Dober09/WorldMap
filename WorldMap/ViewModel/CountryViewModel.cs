using CommunityToolkit.Mvvm.ComponentModel;
using WorldMap.Models;
using CommunityToolkit.Mvvm.Input;
using WorldMap.Services;
using System.Windows.Input;
using System.Collections.ObjectModel;


namespace WorldMap.ViewModel
{
    public partial class CountryViewModel : ObservableObject
    {
        private readonly IDataService  _dataServices;

        [ObservableProperty]
        private ObservableCollection<CountryModel> _countries;

        [ObservableProperty]
        private CountryModel _countryModel;

        [ObservableProperty]
        private bool _isLoading;
        
        public CountryViewModel(IDataService dataServices)
        {
            _dataServices = dataServices;
            
            Countries = new ObservableCollection<CountryModel>();

        }

    

        [RelayCommand]
        private async Task LoadDataAsync()
        {
            try
            {
               IsLoading = true;
               var countries = await _dataServices.LoadJsonDataAsync();
               

           
               Countries = new ObservableCollection<CountryModel>(countries); 
        
                if (Countries.Any())
                {
                    var firstCountry = Countries.First();
                    System.Diagnostics.Debug.WriteLine($"First country: {firstCountry.Name.Common}");
                    System.Diagnostics.Debug.WriteLine($"Languages: {string.Join(", ", firstCountry.Languages?.Select(l => $"{l.Key}: {l.Value}") ?? Array.Empty<string>())}");
                    System.Diagnostics.Debug.WriteLine($"Population: {firstCountry.Population}");
                    System.Diagnostics.Debug.WriteLine($"Region: {firstCountry.Region}");
                }
                

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading data : {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
