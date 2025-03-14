namespace Plugin.Maui.BottomSheet.Navigation;

/// <summary>
/// BottomSheet navigation service.
/// </summary>
public interface IBottomSheetNavigationService
{
    /// <summary>
    /// Gets service provider.
    /// </summary>
    internal IServiceProvider ServiceProvider { get; }

    /// <summary>
    /// Open a <see cref="BottomSheet"/>.
    /// If <paramref name="viewModel"/> isn't null it'll be assigned to BindingContext.
    /// If <paramref name="viewModel"/> implements <see cref="IQueryAttributable"/> <paramref name="parameters"/> will be applied on navigation.
    /// </summary>
    /// <param name="bottomSheet"><see cref="BottomSheet"/> to be opened.</param>
    /// <param name="viewModel">BindingContext of <see cref="BottomSheet"/>.</param>
    /// <param name="parameters">Navigation parameters.</param>
    /// <param name="configure">Action to modify the <see cref="BottomSheet"/>.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous navigation operation.</returns>
    Task NavigateToAsync(IBottomSheet bottomSheet, object? viewModel = null, IBottomSheetNavigationParameters? parameters = null, Action<IBottomSheet>? configure = null);

    /// <summary>
    /// Open a <see cref="BottomSheet"/>.
    /// If <paramref name="viewModel"/> isn't null it'll be assigned to BindingContext.
    /// If <paramref name="viewModel"/> implements <see cref="IQueryAttributable"/> <paramref name="parameters"/> will be applied on navigation.
    /// </summary>
    /// <param name="bottomSheet"><see cref="BottomSheet"/> to be opened.</param>
    /// <param name="viewModel">BindingContext of <see cref="BottomSheet"/>.</param>
    /// <param name="parameters">Navigation parameters.</param>
    /// <param name="configure">Action to modify the <see cref="BottomSheet"/>.</param>
    [Obsolete("Use NavigateToAsync instead.")]
    void NavigateTo(IBottomSheet bottomSheet, object? viewModel = null, IBottomSheetNavigationParameters? parameters = null, Action<IBottomSheet>? configure = null);

    /// <summary>
    /// Close current <see cref="BottomSheet"/>.
    /// If BindingContext implements <see cref="IQueryAttributable"/> <paramref name="parameters"/> will be applied on navigation.
    /// </summary>
    /// <param name="parameters">Navigation parameters.</param>
    [Obsolete("Use GoBackAsync instead.")]
    void GoBack(IBottomSheetNavigationParameters? parameters = null);

    /// <summary>
    /// Close current <see cref="BottomSheet"/>.
    /// If BindingContext implements <see cref="IQueryAttributable"/> <paramref name="parameters"/> will be applied on navigation.
    /// </summary>
    /// <param name="parameters">Navigation parameters.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task GoBackAsync(IBottomSheetNavigationParameters? parameters = null);

    /// <summary>
    /// Close all BottomSheets.
    /// </summary>
    [Obsolete("Use ClearBottomSheetAsync instead.")]
    void ClearBottomSheetStack();

    /// <summary>
    /// Close all BottomSheets.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task ClearBottomSheetStackAsync();
}