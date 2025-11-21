namespace Plugin.Maui.BottomSheet.Navigation;

/// <summary>
/// Defines a contract for view models to determine if navigation away from them is permitted.
/// </summary>
public interface IConfirmNavigation
{
    /// <summary>
    /// Determines if navigation away from the current view model is allowed.
    /// </summary>
    /// <param name="parameters">The navigation parameters used for determining navigation conditions.</param>
    /// <returns>True if navigation is permitted; otherwise, false.</returns>
    bool CanNavigate(IBottomSheetNavigationParameters parameters);
}