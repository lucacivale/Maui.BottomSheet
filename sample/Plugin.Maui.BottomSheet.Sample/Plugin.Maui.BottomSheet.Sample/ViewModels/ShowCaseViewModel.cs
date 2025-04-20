using AsyncAwaitBestPractices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plugin.Maui.BottomSheet.Navigation;

namespace Plugin.Maui.BottomSheet.Sample.ViewModels;

public sealed partial class ShowCaseViewModel : ObservableObject
{
    private readonly List<string> _colors =
    [
        "#ffaf83aa",
        "#803983aa",
        "#ff398340",
        "#66b98340"
    ];
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

    [ObservableProperty]
    private Color _windowBackgroundColor = Color.FromArgb("#80000000");

    [ObservableProperty]
    private bool _showCloseButton;

    [ObservableProperty]
    private bool _isModal;

    [ObservableProperty]
    private CloseButtonPosition _closeButtonPosition = CloseButtonPosition.TopRight;

    [RelayCommand]
    private void TopLeftCloseButton()
    {
        CloseButtonPosition = CloseButtonPosition.TopLeft;
    }

    [RelayCommand]
    private void TopRightCloseButton()
    {
        CloseButtonPosition = CloseButtonPosition.TopRight;
    }

    [RelayCommand]
    private void ChangeWindowBackgroundColor()
    {
        int colorIndex = _colors.IndexOf(WindowBackgroundColor.ToArgbHex(true).ToLower());
        int elementAt = colorIndex == _colors.Count - 1 ? 0 : colorIndex + 1;

        WindowBackgroundColor = Color.FromArgb(_colors.ElementAt(elementAt));
    }

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
    private bool _isNonModalOpen;

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
    private void OpenShellPageCommand()
    {
        IsOpen = true;
    }

    [RelayCommand]
    private void OpenNonModalShowcase()
    {
        IsNonModalOpen = true;
    }

    [RelayCommand]
    private void OpenShowcasePageAsBottomSheet()
    {
        _bottomSheetNavigationService.NavigateToAsync("Showcase").SafeFireAndForget();
    }

    [RelayCommand]
    private void OpenCustomHeaderShowcaseViewAsBottomSheet()
    {
        _bottomSheetNavigationService.NavigateToAsync<CustomHeaderShowcaseViewModel>("CustomHeaderShowcase").SafeFireAndForget();
    }

    [RelayCommand]
    private void OpenSomeBottomSheet()
    {
        _bottomSheetNavigationService.NavigateToAsync("SomeBottomSheet").SafeFireAndForget();
    }

    [RelayCommand]
    private void CloseAllOpenSheets()
    {
        _bottomSheetNavigationService.ClearBottomSheetStackAsync().SafeFireAndForget();
    }

    [RelayCommand]
    private void CloseCurrentSheet()
    {
        _bottomSheetNavigationService.GoBackAsync().SafeFireAndForget();
    }
}