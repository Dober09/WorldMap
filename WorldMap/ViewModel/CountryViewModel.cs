using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using WorldMap.Models;
using CommunityToolkit.Mvvm.Input;
using WorldMap.Services;
using System.Windows.Input;


namespace WorldMap.ViewModel
{
    public partial class CountryViewModel : ObservableObject
    {
        private readonly IDataService  _dataServices;

        [ObservableProperty]
        private List<CountryModel> _countries;

        [ObservableProperty]
        private CountryModel _countryModel;

        [ObservableProperty]
        private bool _isLoading;
        
        public CountryViewModel(IDataService dataServices)
        {
            _dataServices = dataServices;
            LoadDataComand = new AsyncRelayCommand(LoadDataAsync); ;

        }

        public  ICommand LoadDataComand { get;  }

        private async Task LoadDataAsync()
        {
            try
            {
                IsLoading = true;
                Countries = await _dataServices.LoadJsonDataAsync();

            }catch (Exception ex)
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
