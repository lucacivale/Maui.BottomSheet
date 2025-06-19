namespace Plugin.Maui.BottomSheet.Navigation;

/// <summary>
/// Provides notifications for navigation events to view models.
/// </summary>
public interface INavigationAware
{
    /// <summary>
    /// Called when navigating away from this view model.
    /// </summary>
    /// <param name="parameters">The navigation parameters.</param>
    void OnNavigatedFrom(IBottomSheetNavigationParameters parameters);

    /// <summary>
    /// Called when this view model has been navigated to.
    /// </summary>
    /// <param name="parameters">The navigation parameters.</param>
    void OnNavigatedTo(IBottomSheetNavigationParameters parameters);
}