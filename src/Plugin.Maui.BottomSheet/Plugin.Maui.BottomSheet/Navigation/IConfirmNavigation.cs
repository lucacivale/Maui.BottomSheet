namespace Plugin.Maui.BottomSheet.Navigation;

/// <summary>
/// Allows view models to control whether navigation away from them should proceed.
/// </summary>
public interface IConfirmNavigation
{
    /// <summary>
    /// Determines if navigation away from this view model is allowed.
    /// </summary>
    /// <param name="parameters">The navigation parameters.</param>
    /// <returns>True if navigation can proceed; otherwise, false.</returns>
    bool CanNavigate(IBottomSheetNavigationParameters parameters);
}