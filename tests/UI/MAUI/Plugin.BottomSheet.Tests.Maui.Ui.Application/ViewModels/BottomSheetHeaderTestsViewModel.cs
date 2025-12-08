using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Plugin.BottomSheet.Tests.Maui.Ui.Application.ViewModels;

public partial class BottomSheetHeaderTestsViewModel : ObservableObject
{
    [ObservableProperty]
    public partial bool IsOpen { get; set; }
    
    [ObservableProperty]
    public partial bool ShowHeader { get; set; }
    
    [ObservableProperty]
    public partial string? Title { get; set; }
    
    [ObservableProperty]
    public partial bool ShowCloseButton { get; set; }
    
    [ObservableProperty]
    public partial bool IsCancelable { get; set; } = true;
    
    [ObservableProperty]
    public partial BottomSheetHeaderCloseButtonPosition CloseButtonPosition { get; set; }
    
    [ObservableProperty]
    public partial BottomSheetHeaderButtonAppearanceMode ButtonAppearanceMode { get; set; }
    
    [ObservableProperty]
    public partial string TopLeftButtonOneText { get; set; } = "Left One";
    
    [ObservableProperty]
    public partial string TopLeftButtonTwoText { get; set; } = "Left Two";
    
    [ObservableProperty]
    public partial string TopRightButtonOneText { get; set; } = "Right One";
    
    [ObservableProperty]
    public partial string TopRightButtonTwoText { get; set; } = "Right Two";
    
    [ObservableProperty]
    public partial Button? TopLeftButton { get; set; }
    
    [ObservableProperty]
    public partial Button? TopRightButton { get; set; }
    
    [RelayCommand]
    private void OpenBottomSheet()
    {
        IsOpen = true;
    }
    
    [RelayCommand]
    private void TopLeftButtonOne()
    {
        TopLeftButtonOneText = "Clicked";
    }
    
    [RelayCommand]
    private void TopLeftButtonTwo()
    {
        TopLeftButtonTwoText = "Clicked";
    }
    
    [RelayCommand]
    private void TopRightButtonOne()
    {
        TopRightButtonOneText = "Clicked";
    }
    
    [RelayCommand]
    private void TopRightButtonTwo()
    {
        TopRightButtonTwoText = "Clicked";
    }

    [ObservableProperty]
    public partial bool IsOpenCustomHeader { get; set; }

    [ObservableProperty]
    public partial string? CustomHeaderButtonText { get; set; } = "Click Me";

    [RelayCommand]
    private void CustomHeaderButton()
    {
        CustomHeaderButtonText = "Clicked";
    }
}