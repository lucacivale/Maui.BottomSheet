using Plugin.Maui.BottomSheet.Sample.ViewModels;

namespace Plugin.Maui.BottomSheet.Sample.Views;

public partial class ShellPage : ContentPage
{
	public ShellPage(ShellPageViewModel shellPageViewModel)
	{
		InitializeComponent();
        BindingContext = shellPageViewModel;
    }
}