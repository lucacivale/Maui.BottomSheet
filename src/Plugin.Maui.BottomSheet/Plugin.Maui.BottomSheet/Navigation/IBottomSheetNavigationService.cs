namespace Plugin.Maui.BottomSheet.Navigation;

/// <summary>
/// Provides navigation capabilities for bottom sheet management.
/// </summary>
public interface IBottomSheetNavigationService
{
    /// <summary>
    /// Gets the service provider for dependency resolution.
    /// </summary>
    internal IServiceProvider ServiceProvider { get; }

    /// <summary>
    /// Navigates to a bottom sheet with optional view model and parameters.
    /// </summary>
    /// <param name="bottomSheet">The bottom sheet to display.</param>
    /// <param name="viewModel">Optional view model to bind to the bottom sheet.</param>
    /// <param name="parameters">Optional navigation parameters.</param>
    /// <param name="configure">Optional action to configure the bottom sheet.</param>
    /// <returns>A task containing the navigation result.</returns>
    Task<INavigationResult> NavigateToAsync(IBottomSheet bottomSheet, object? viewModel = null, IBottomSheetNavigationParameters? parameters = null, Action<IBottomSheet>? configure = null);

    /// <summary>
    /// Closes the current bottom sheet and navigates back.
    /// </summary>
    /// <param name="parameters">Optional navigation parameters.</param>
    /// <returns>A task containing the navigation result.</returns>
    Task<INavigationResult> GoBackAsync(IBottomSheetNavigationParameters? parameters = null);

    /// <summary>
    /// Closes all open bottom sheets in the navigation stack.
    /// </summary>
    /// <returns>A task containing the results of all closing operations.</returns>
    Task<IEnumerable<INavigationResult>> ClearBottomSheetStackAsync();

    /// <summary>
    /// Gets the current bottom sheet navigation stack.
    /// </summary>
    /// <returns>A read-only collection of bottom sheets in the stack.</returns>
    IReadOnlyCollection<IBottomSheet> NavigationStack();
}