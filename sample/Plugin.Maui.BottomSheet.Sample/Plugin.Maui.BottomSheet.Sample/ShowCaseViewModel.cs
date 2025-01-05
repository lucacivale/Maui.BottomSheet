namespace Plugin.Maui.BottomSheet.Sample;

using AsyncAwaitBestPractices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

public sealed partial class ShowCaseViewModel : ObservableObject
{
    [ObservableProperty]
    private BottomSheetHeaderButtonAppearanceMode _headerButtonAppearanceMode = BottomSheetHeaderButtonAppearanceMode.LeftAndRightButton;
    
    [ObservableProperty]
    private string _title = "My Title";
    
    [RelayCommand]
    private static void TopLefButton()
    {
        Shell.Current.DisplayAlert("Notification", "Top left clicked", "Cancel").SafeFireAndForget(continueOnCapturedContext: false);
    }
    
    [RelayCommand]
    private static void TopRightButton()
    {
        Shell.Current.DisplayAlert("Notification", "Top right clicked", "Cancel").SafeFireAndForget(continueOnCapturedContext: false);
    }
    
    [RelayCommand]
    private void HeaderButtonAppearanceModeNone()
    {
        HeaderButtonAppearanceMode = BottomSheetHeaderButtonAppearanceMode.None;
    }
    
    [RelayCommand]
    private void HeaderButtonAppearanceModeLeft()
    {
        HeaderButtonAppearanceMode = BottomSheetHeaderButtonAppearanceMode.LeftButton;
    }
    
    [RelayCommand]
    private void HeaderButtonAppearanceModeRight()
    {
        HeaderButtonAppearanceMode = BottomSheetHeaderButtonAppearanceMode.RightButton;
    }
    
    [RelayCommand]
    private void HeaderButtonAppearanceModeLeftAndRight()
    {
        HeaderButtonAppearanceMode = BottomSheetHeaderButtonAppearanceMode.LeftAndRightButton;
    }

    [ObservableProperty]
    private bool _isOpen;
    
    [ObservableProperty]
    private bool _hasHandle = true;
    
    [ObservableProperty]
    private bool _isCancelable = true;
    
    [ObservableProperty]
    private bool _showHeader = true;

    [ObservableProperty]
    private bool _isDraggable = true;

    [RelayCommand]
    private void OpenShowcase()
    {
        IsOpen = true;
    }
}