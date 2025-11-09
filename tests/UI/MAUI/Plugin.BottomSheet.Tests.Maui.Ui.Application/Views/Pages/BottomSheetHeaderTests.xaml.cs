using Plugin.BottomSheet.Tests.Maui.Ui.Application.ViewModels;

namespace Plugin.BottomSheet.Tests.Maui.Ui.Application.Views.Pages;

public partial class BottomSheetHeaderTests
{
    public BottomSheetHeaderTests(BottomSheetHeaderTestsViewModel viewModel)
    {
        InitializeComponent();
        
        BindingContext = viewModel;
    }

    private async void OpenBottomSheet(object? sender, EventArgs e)
    {
        BottomSheet.IsOpen = true;
        
        await Task.Delay(TimeSpan.FromSeconds(1));

        RadioButtonGroup.SetSelectedValue(HeaderLeftButtonVersion, HeaderLeftButtonVersionOne.Value);
        RadioButtonGroup.SetSelectedValue(HeaderRightButtonVersion, HeaderRightButtonVersionOne.Value);
        IsCancelable.IsToggled = true;
    }

    private async void ChangeBindingContext(object? sender, EventArgs e)
    {
        bool isBottomSheetCustomHeaderOpen = BottomSheetCustomHeader.IsOpen;
        BindingContext = new BottomSheetHeaderTestsViewModel();
        await Task.Delay(TimeSpan.FromSeconds(1));

        if (isBottomSheetCustomHeaderOpen)
        {
            BottomSheetCustomHeader.IsOpen = true;
        }
        else
        {
            BottomSheet.IsOpen = true;
        }
    }

    private void CloseBottomSheet(object? sender, EventArgs e)
    {
        BottomSheet.IsOpen = false;
    }

    private void OpenBottomSheetCustomHeader(object? sender, EventArgs e)
    {
        BottomSheetCustomHeader.IsOpen = true;
    }
}