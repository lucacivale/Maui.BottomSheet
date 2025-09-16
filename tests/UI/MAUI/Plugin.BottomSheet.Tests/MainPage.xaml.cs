using AsyncAwaitBestPractices;
using Plugin.BottomSheet.Tests.Extensions;

namespace Plugin.BottomSheet.Tests;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private void Button_OnClicked(object? sender, EventArgs e)
    {
        Shell.Current.GoToBottomSheetHeaderTestsAsync().SafeFireAndForget();
    }

    private void OpenBottomSheetTestsPage(object? sender, EventArgs e)
    {
        Shell.Current.GoToBottomSheetTestsAsync().SafeFireAndForget();
    }
}