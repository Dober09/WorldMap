using WorldMap.ViewModel;
using WorldMap.Services;
namespace WorldMap.View;

public partial class LandingPage : ContentPage
{

    public LandingPage(CountryViewModel viewModels)
    {
        InitializeComponent();    
        
        BindingContext = viewModels;
    }

   protected override async void OnAppearing()
   {
       base.OnAppearing(); 
       await ((CountryViewModel)BindingContext).LoadDataCommand.ExecuteAsync(null);
    }
  
    
}
