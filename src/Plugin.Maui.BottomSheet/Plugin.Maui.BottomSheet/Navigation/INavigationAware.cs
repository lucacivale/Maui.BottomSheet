namespace Plugin.Maui.BottomSheet.Navigation;

/// <summary>
/// Provides a way for objects involved in navigation to be notified of navigation activities.
/// </summary>
public interface INavigationAware
{
    /// <summary>
    /// Called when the implementer is being navigated away from.
    /// </summary>
    /// <param name="parameters">Navigation parameters.</param>
    void OnNavigatedFrom(IBottomSheetNavigationParameters parameters);

    /// <summary>
    /// Called when the implementer has been navigated to.
    /// </summary>
    /// <param name="parameters">Navigation parameters.</param>
    void OnNavigatedTo(IBottomSheetNavigationParameters parameters);
}