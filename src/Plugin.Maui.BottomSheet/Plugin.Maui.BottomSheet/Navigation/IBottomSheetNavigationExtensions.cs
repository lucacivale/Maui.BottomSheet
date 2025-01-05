namespace Plugin.Maui.BottomSheet.Navigation;

/// <summary>
/// <see cref="IBottomSheetNavigationService"/> extension methods.
/// </summary>
// ReSharper disable once InconsistentNaming
public static class IBottomSheetNavigationExtensions
{
    /// <summary>
    /// Open a <see cref="BottomSheet"/>.
    /// </summary>
    /// <param name="navigationService">Navigation service.</param>
    /// <param name="name">Name of the <see cref="BottomSheet"/> to be opened.</param>
    /// <param name="parameters">Navigation parameters.</param>
    /// <param name="configure">Action to modify the <see cref="BottomSheet"/>.</param>
    public static void NavigateTo(this IBottomSheetNavigationService navigationService, string name, IBottomSheetNavigationParameters? parameters = null, Action<IBottomSheet>? configure = null)
    {
        ArgumentNullException.ThrowIfNull(navigationService);

        var bottomSheet = navigationService.ServiceProvider.GetRequiredKeyedService<IBottomSheet>(name);

        navigationService.NavigateTo(bottomSheet, null, parameters, configure);
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
    public static void NavigateTo<TViewModel>(this IBottomSheetNavigationService navigationService, string name, IBottomSheetNavigationParameters? parameters = null, Action<IBottomSheet>? configure = null)
        where TViewModel : class
    {
        ArgumentNullException.ThrowIfNull(navigationService);

        var bottomSheet = navigationService.ServiceProvider.GetRequiredKeyedService<IBottomSheet>(name);
        var viewModel = navigationService.ServiceProvider.GetRequiredService<TViewModel>();

        navigationService.NavigateTo(bottomSheet, viewModel, parameters, configure);
    }
}