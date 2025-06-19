namespace Plugin.Maui.BottomSheet.Navigation;

/// <summary>
/// Allows view models to asynchronously control whether navigation away from them should proceed.
/// </summary>
public interface IConfirmNavigationAsync
{
    /// <summary>
    /// Asynchronously determines if navigation away from this view model is allowed.
    /// </summary>
    /// <param name="parameters">The navigation parameters.</param>
    /// <returns>A task containing true if navigation can proceed; otherwise, false.</returns>
    Task<bool> CanNavigateAsync(IBottomSheetNavigationParameters parameters);
}