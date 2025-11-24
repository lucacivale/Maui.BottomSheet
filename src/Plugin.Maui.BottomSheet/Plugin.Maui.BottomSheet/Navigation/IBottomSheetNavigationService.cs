namespace Plugin.Maui.BottomSheet.Navigation;

/// <summary>
/// Defines navigation functionalities for managing bottom sheet components in a Maui application.
/// </summary>
public interface IBottomSheetNavigationService
{
    /// <summary>
    /// Gets access to the service provider for resolving dependencies.
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
    /// <param name="parameters">Optional navigation parameters to pass during the back navigation.</param>
    /// <returns>A task containing the result of the navigation operation.</returns>
    Task<INavigationResult> GoBackAsync(IBottomSheetNavigationParameters? parameters = null);

    /// <summary>
    /// Clears the navigation stack of all bottom sheets by closing them sequentially.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.
    /// The result contains a collection of <see cref="INavigationResult"/> detailing the outcome
    /// of each close operation.</returns>
    Task<IEnumerable<INavigationResult>> ClearBottomSheetStackAsync();

    /// <summary>
    /// Retrieves the current stack of bottom sheets being managed by the navigation service.
    /// </summary>
    /// <returns>A read-only collection representing the bottom sheets currently in the navigation stack.</returns>
    IReadOnlyCollection<IBottomSheet> NavigationStack();
}