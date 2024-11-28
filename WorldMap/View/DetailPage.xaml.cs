using WorldMap.ViewModel;

namespace WorldMap.View;

public partial class DetailPage : ContentPage
{
	public DetailPage(CountryDetailViewModel viewModel)
	{
		 InitializeComponent();
		BindingContext=viewModel;
	}
}