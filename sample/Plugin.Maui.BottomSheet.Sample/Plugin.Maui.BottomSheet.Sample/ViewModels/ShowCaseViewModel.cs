using AsyncAwaitBestPractices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plugin.BottomSheet;
using Plugin.Maui.BottomSheet.Navigation;

namespace Plugin.Maui.BottomSheet.Sample.ViewModels;

public sealed partial class ShowCaseViewModel : ObservableObject, IConfirmNavigationAsync
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
    public partial BottomSheetHeaderButtonAppearanceMode HeaderButtonAppearanceMode { get; set; } = BottomSheetHeaderButtonAppearanceMode.LeftAndRightButton;

    [ObservableProperty]
    public partial string Title { get; set; } = "My Title";

    [ObservableProperty]
    public partial float CornerRadius { get; set; } = 20;

    [ObservableProperty]
    public partial Color WindowBackgroundColor { get; set; } = Color.FromArgb("#80000000");

    [ObservableProperty]
    public partial bool ShowCloseButton { get; set; }

    [ObservableProperty]
    public partial bool IsModal { get; set; }

    [ObservableProperty]
    public partial BottomSheetHeaderCloseButtonPosition CloseButtonPosition { get; set; } = BottomSheetHeaderCloseButtonPosition.TopRight;

    [RelayCommand]
    private void TopLeftCloseButton()
    {
        CloseButtonPosition = BottomSheetHeaderCloseButtonPosition.TopLeft;
    }

    [RelayCommand]
    private void TopRightCloseButton()
    {
        CloseButtonPosition = BottomSheetHeaderCloseButtonPosition.TopRight;
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
        Shell.Current.DisplayAlertAsync("Notification", "Top left clicked", "Cancel").SafeFireAndForget(continueOnCapturedContext: false);
    }

    [RelayCommand]
    private static void TopRightButton()
    {
        Shell.Current.DisplayAlertAsync("Notification", "Top right clicked", "Cancel").SafeFireAndForget(continueOnCapturedContext: false);
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
    public partial bool IsOpen { get; set; }

    [ObservableProperty]
    public partial bool IsNonModalOpen { get; set; }

    [ObservableProperty]
    public partial bool HasHandle { get; set; } = true;

    [ObservableProperty]
    public partial bool IsCancelable { get; set; } = true;

    [ObservableProperty]
    public partial bool ShowHeader { get; set; } = true;

    [ObservableProperty]
    public partial bool IsDraggable { get; set; } = true;

    [ObservableProperty]
    public partial bool AboveTabBar { get; set; } = false;

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

    public Task<bool> CanNavigateAsync(IBottomSheetNavigationParameters? parameters)
    {
        return Task.FromResult(true);
        /*return Shell.Current.CurrentPage.DisplayAlertAsync(
            "Warning",
            "You are about to navigate away",
            "OK",
            "Cancel");*/
    }
}