namespace Plugin.Maui.BottomSheet.Navigation;

/// <summary>
/// <see cref="IBottomSheetNavigationService"/> extension methods.
/// </summary>
// ReSharper disable once InconsistentNaming
public static class IBottomSheetNavigationServiceExtensions
{
    /// <summary>
    /// Open a <see cref="BottomSheet"/>.
    /// </summary>
    /// <param name="navigationService">Navigation service.</param>
    /// <param name="name">Name of the <see cref="BottomSheet"/> to be opened.</param>
    /// <param name="parameters">Navigation parameters.</param>
    /// <param name="configure">Action to modify the <see cref="BottomSheet"/>.</param>
    [Obsolete("Use NavigateToAsync instead.")]
    public static void NavigateTo(this IBottomSheetNavigationService navigationService, string name, IBottomSheetNavigationParameters? parameters = null, Action<IBottomSheet>? configure = null)
    {
        ArgumentNullException.ThrowIfNull(navigationService);

        var bottomSheet = navigationService.ServiceProvider.GetRequiredKeyedService<IBottomSheet>(name);

        object? viewModel = null;

        if (IBottomSheetNavigationService.BottomSheetToViewModelMapping.TryGetValue(name, out var viewModelType))
        {
            viewModel = navigationService.ServiceProvider.GetService(viewModelType);
        }

        navigationService.NavigateTo(bottomSheet, viewModel, parameters, configure);
    }

    /// <summary>
    /// Open a <see cref="BottomSheet"/>.
    /// If <typeparamref name="TViewModel"/> isn't null it'll be assigned to BindingContext.
    /// If <typeparamref name="TViewModel"/> implements <see cref="IQueryAttributable"/> <paramref name="parameters"/> will be applied on navigation.
    /// </summary>
    /// <param name="navigationService">Navigation service.</param>
    /// <param name="name">Name of the <see cref="BottomSheet"/> to be opened.</param>
    /// <param name="parameters">Navigation parameters.</param>
    /// <param name="configure">Action to modify the <see cref="BottomSheet"/>.</param>
    /// <typeparam name="TViewModel">View model type.</typeparam>
    [Obsolete("Use NavigateToAsync instead.")]
    public static void NavigateTo<TViewModel>(this IBottomSheetNavigationService navigationService, string name, IBottomSheetNavigationParameters? parameters = null, Action<IBottomSheet>? configure = null)
        where TViewModel : class
    {
        ArgumentNullException.ThrowIfNull(navigationService);

        var bottomSheet = navigationService.ServiceProvider.GetRequiredKeyedService<IBottomSheet>(name);
        var viewModel = navigationService.ServiceProvider.GetRequiredService<TViewModel>();

        navigationService.NavigateTo(bottomSheet, viewModel, parameters, configure);
    }

    /// <summary>
    /// Open a <see cref="BottomSheet"/>.
    /// </summary>
    /// <param name="navigationService">Navigation service.</param>
    /// <param name="name">Name of the <see cref="BottomSheet"/> to be opened.</param>
    /// <param name="parameters">Navigation parameters.</param>
    /// <param name="configure">Action to modify the <see cref="BottomSheet"/>.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public static Task NavigateToAsync(this IBottomSheetNavigationService navigationService, string name, IBottomSheetNavigationParameters? parameters = null, Action<IBottomSheet>? configure = null)
    {
        ArgumentNullException.ThrowIfNull(navigationService);

        var bottomSheet = navigationService.ServiceProvider.GetRequiredKeyedService<IBottomSheet>(name);

        object? viewModel = null;

        if (IBottomSheetNavigationService.BottomSheetToViewModelMapping.TryGetValue(name, out var viewModelType))
        {
            viewModel = navigationService.ServiceProvider.GetService(viewModelType);
        }

        return navigationService.NavigateToAsync(bottomSheet, viewModel, parameters, configure);
    }

    /// <summary>
    /// Open a <see cref="BottomSheet"/>.
    /// If <typeparamref name="TViewModel"/> isn't null it'll be assigned to BindingContext.
    /// If <typeparamref name="TViewModel"/> implements <see cref="IQueryAttributable"/> <paramref name="parameters"/> will be applied on navigation.
    /// </summary>
    /// <param name="navigationService">Navigation service.</param>
    /// <param name="name">Name of the <see cref="BottomSheet"/> to be opened.</param>
    /// <param name="parameters">Navigation parameters.</param>
    /// <param name="configure">Action to modify the <see cref="BottomSheet"/>.</param>
    /// <typeparam name="TViewModel">View model type.</typeparam>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public static Task NavigateToAsync<TViewModel>(this IBottomSheetNavigationService navigationService, string name, IBottomSheetNavigationParameters? parameters = null, Action<IBottomSheet>? configure = null)
        where TViewModel : class
    {
        ArgumentNullException.ThrowIfNull(navigationService);

        var bottomSheet = navigationService.ServiceProvider.GetRequiredKeyedService<IBottomSheet>(name);
        var viewModel = navigationService.ServiceProvider.GetRequiredService<TViewModel>();

        return navigationService.NavigateToAsync(bottomSheet, viewModel, parameters, configure);
    }
}