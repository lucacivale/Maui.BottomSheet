using Maui.BottomSheet.Samples.Services;

namespace Maui.BottomSheet.Samples;

public partial class MainPage : ContentPage
{
	public MainPage(MainViewModel mainViewModel)
	{
		InitializeComponent();

		BindingContext = mainViewModel;
	}
}


