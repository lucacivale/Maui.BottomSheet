namespace Plugin.Maui.BottomSheet.Navigation;

/// <summary>
/// Provides functionality for view models to determine asynchronously
/// whether they allow navigation away from their current state.
/// </summary>
public interface IConfirmNavigationAsync
{
    /// <summary>
    /// Asynchronously determines if navigation away from this view model is allowed.
    /// </summary>
    /// <param name="parameters">The navigation parameters, providing context and data for the navigation request.</param>
    /// <returns>A task containing true if navigation can proceed; otherwise, false.</returns>
    Task<bool> CanNavigateAsync(IBottomSheetNavigationParameters parameters);
}