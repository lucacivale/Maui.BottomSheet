namespace Plugin.BottomSheet.Tests.Maui.Ui.Application.Views.Pages;

public partial class ModalPageBottomSheetTestsPage : ContentPage
{
    public ModalPageBottomSheetTestsPage()
    {
        InitializeComponent();
    }

    private async void Button_OnClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}