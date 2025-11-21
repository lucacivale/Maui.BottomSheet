namespace Plugin.Maui.BottomSheet.Navigation;

/// <summary>
/// Defines an interface for handling navigation lifecycle events.
/// </summary>
public interface INavigationAware
{
    /// <summary>
    /// Called when navigating away from this view model.
    /// </summary>
    /// <param name="parameters">The navigation parameters used during the operation.</param>
    void OnNavigatedFrom(IBottomSheetNavigationParameters parameters);

    /// <summary>
    /// Called when this view model has been navigated to.
    /// </summary>
    /// <param name="parameters">The navigation parameters.</param>
    void OnNavigatedTo(IBottomSheetNavigationParameters parameters);
}