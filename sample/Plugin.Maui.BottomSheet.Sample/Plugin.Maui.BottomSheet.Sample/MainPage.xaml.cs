using Plugin.Maui.BottomSheet.Sample.Views;

namespace Plugin.Maui.BottomSheet.Sample;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private async void OpenShowCasePage(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ShowCasePage));
    }

    private async void OpenShellPage(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ShellPage));
    }
}