using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Plugin.BottomSheet.Tests.Maui.Ui.Application.ViewModels;

public partial class BottomSheetHeaderTestsViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _isOpen;
    
    [ObservableProperty]
    private bool _showHeader;
    
    [ObservableProperty]
    private string? _title;
    
    [ObservableProperty]
    private bool _showCloseButton;
    
    [ObservableProperty]
    private bool _isCancelable = true;
    
    [ObservableProperty]
    private BottomSheetHeaderCloseButtonPosition _closeButtonPosition;
    
    [ObservableProperty]
    private BottomSheetHeaderButtonAppearanceMode _buttonAppearanceMode;
    
    [ObservableProperty]
    private string _topLeftButtonOneText = "Left One";
    
    [ObservableProperty]
    private string _topLeftButtonTwoText = "Left Two";
    
    [ObservableProperty]
    private string _topRightButtonOneText = "Right One";
    
    [ObservableProperty]
    private string _topRightButtonTwoText = "Right Two";
    
    [ObservableProperty]
    private Button? _topLeftButton;
    
    [ObservableProperty]
    private Button? _topRightButton;
    
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
    private bool _isOpenCustomHeader;

    [ObservableProperty]
    private string? _customHeaderButtonText = "Click Me";

    [RelayCommand]
    private void CustomHeaderButton()
    {
        CustomHeaderButtonText = "Clicked";
    }
}