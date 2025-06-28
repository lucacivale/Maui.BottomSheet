namespace Plugin.Maui.BottomSheet.Tests.App;

public partial class MainPage : ContentPage
{
    int _count = 0;

    public MainPage()
    {
        InitializeComponent();
    }

    private void OnCounterClicked(object? sender, EventArgs e)
    {
        _count++;

        if (_count == 1)
            CounterBtn.Text = $"Clicked {_count} time";
        else
            CounterBtn.Text = $"Clicked {_count} times";

        SemanticScreenReader.Announce(CounterBtn.Text);
    }
}