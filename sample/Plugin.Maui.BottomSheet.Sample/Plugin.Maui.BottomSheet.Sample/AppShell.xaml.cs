using Plugin.Maui.BottomSheet.Sample.Views;

namespace Plugin.Maui.BottomSheet.Sample;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(ShowCasePage), typeof(ShowCasePage));
        Routing.RegisterRoute(nameof(ShellPage), typeof(ShellPage));
    }
}