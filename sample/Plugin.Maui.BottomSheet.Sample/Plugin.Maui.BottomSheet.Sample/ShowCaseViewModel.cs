namespace Plugin.Maui.BottomSheet.Sample;

using AsyncAwaitBestPractices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Navigation;

public sealed partial class ShowCaseViewModel : ObservableObject
{
    private readonly IBottomSheetNavigationService _bottomSheetNavigationService;

    public ShowCaseViewModel(IBottomSheetNavigationService bottomSheetNavigationService)
    {
        _bottomSheetNavigationService = bottomSheetNavigationService;
    }

    [ObservableProperty]
    private BottomSheetHeaderButtonAppearanceMode _headerButtonAppearanceMode = BottomSheetHeaderButtonAppearanceMode.LeftAndRightButton;
    
    [ObservableProperty]
    private string _title = "My Title";
    
    [ObservableProperty]
    private float _cornerRadius = 20;
    
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
    
    [RelayCommand]
    private void OpenShowcasePageAsBottomSheet()
    {
        _bottomSheetNavigationService.NavigateTo("Showcase");
    }
    
    [RelayCommand]
    private void OpenCustomHeaderShowcaseViewAsBottomSheet()
    {
        _bottomSheetNavigationService.NavigateTo<CustomHeaderShowcaseViewModel>("CustomHeaderShowcase");
    }

    [RelayCommand]
    private void CloseAllOpenSheets()
    {
        _bottomSheetNavigationService.ClearBottomSheetStack();
    }

    [RelayCommand]
    private void CloseCurrentSheet()
    {
        _bottomSheetNavigationService.GoBack();
    }
}
