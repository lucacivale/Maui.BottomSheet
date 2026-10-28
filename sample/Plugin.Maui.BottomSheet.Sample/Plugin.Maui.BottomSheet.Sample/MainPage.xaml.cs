using Plugin.Maui.BottomSheet.Sample.Views;

namespace Plugin.Maui.BottomSheet.Sample;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private bool _themeTestStarted;

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (!_themeTestStarted && Environment.GetEnvironmentVariable("BOTTOMSHEET_THEME_TEST") == "1")
        {
            _themeTestStarted = true;
            await Task.Delay(1000);
            await ThemeAppearanceSample.OpenAsync(this, true);
        }
    }

    private async void OpenThemeSample(object sender, EventArgs e)
    {
        await ThemeAppearanceSample.OpenAsync(this, false);
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