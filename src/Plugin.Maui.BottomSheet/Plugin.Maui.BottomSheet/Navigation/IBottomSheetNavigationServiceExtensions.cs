namespace Plugin.Maui.BottomSheet.Navigation;

/// <summary>
/// Provides extension methods for navigating to bottom sheets using the
/// <see cref="IBottomSheetNavigationService"/> with options for view model resolution and customization.
/// </summary>
// ReSharper disable once InconsistentNaming
public static class IBottomSheetNavigationServiceExtensions
{
    /// <summary>
    /// Navigates to a bottom sheet by name with automatic view model resolution.
    /// </summary>
    /// <param name="navigationService">The navigation service.</param>
    /// <param name="name">The registered name of the bottom sheet.</param>
    /// <param name="parameters">Optional navigation parameters.</param>
    /// <param name="configure">Optional action to configure the bottom sheet.</param>
    /// <returns>A task representing the navigation operation.</returns>
    public static Task NavigateToAsync(this IBottomSheetNavigationService navigationService, string name, IBottomSheetNavigationParameters? parameters = null, Action<IBottomSheet>? configure = null)
    {
        ArgumentNullException.ThrowIfNull(navigationService);

        IBottomSheet bottomSheet = navigationService.ServiceProvider.GetRequiredKeyedService<IBottomSheet>(name);

        object? viewModel = null;

        if (BottomSheetNavigationService.BottomSheetToViewModelMapping.TryGetValue(name, out Type? viewModelType))
        {
            viewModel = navigationService.ServiceProvider.GetService(viewModelType);
        }

        return navigationService.NavigateToAsync(bottomSheet, viewModel, parameters, configure);
    }

    /// <summary>
    /// Navigates to a bottom sheet by name with a strongly-typed view model.
    /// </summary>
    /// <typeparam name="TViewModel">The type of view model to bind.</typeparam>
    /// <param name="navigationService">The navigation service.</param>
    /// <param name="name">The registered name of the bottom sheet.</param>
    /// <param name="parameters">Optional navigation parameters.</param>
    /// <param name="configure">Optional action to configure the bottom sheet.</param>
    /// <returns>A task representing the navigation operation.</returns>
    public static Task NavigateToAsync<TViewModel>(this IBottomSheetNavigationService navigationService, string name, IBottomSheetNavigationParameters? parameters = null, Action<IBottomSheet>? configure = null)
        where TViewModel : class
    {
        ArgumentNullException.ThrowIfNull(navigationService);

        IBottomSheet bottomSheet = navigationService.ServiceProvider.GetRequiredKeyedService<IBottomSheet>(name);
        TViewModel viewModel = navigationService.ServiceProvider.GetRequiredService<TViewModel>();

        return navigationService.NavigateToAsync(bottomSheet, viewModel, parameters, configure);
    }
}