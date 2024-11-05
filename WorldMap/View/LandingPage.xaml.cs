using WorldMap.ViewModel;
using WorldMap.Services;
using System.Xml.Linq;
namespace WorldMap.View;

public partial class LandingPage : ContentPage
{

    public LandingPage()
    {
        InitializeComponent();    
        
        var vm = new CountryViewModel(new DataServices());
        BindingContext = vm;
    }
  
    
}
