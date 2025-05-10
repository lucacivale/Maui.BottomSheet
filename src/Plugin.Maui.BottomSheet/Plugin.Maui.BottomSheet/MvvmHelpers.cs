using System.Diagnostics.CodeAnalysis;
using Plugin.Maui.BottomSheet.Navigation;

namespace Plugin.Maui.BottomSheet;

/// <summary>
/// Collection of navigation helper methods.
/// </summary>
internal static class MvvmHelpers
{
    /// <summary>
    /// Determines whether <paramref name="bottomSheet"/> accepts being navigated away from.
    /// </summary>
    /// <param name="bottomSheet">The <see cref="IBottomSheet"/> to navigate from.</param>
    /// <param name="parameters">Navigation parameters.</param>
    /// <returns>Can navigate away from <see cref="IBottomSheet"/>.</returns>
    [SuppressMessage("Usage", "MethodHasAsyncOverload: ReSharper disable once MethodHasAsyncOverload", Justification = "False positive.")]
    [SuppressMessage("Performance", "CA1849: Call async methods when in an async method", Justification = "False positive.")]
    [SuppressMessage("Usage", "VSTHRD103: Call async methods when in an async method", Justification = "False positive.")]
    [SuppressMessage("Usage", "S6966: Awaitable method should be used", Justification = "False positive.")]
    internal static async Task<bool> ConfirmNavigationAsync(IBottomSheet bottomSheet, IBottomSheetNavigationParameters parameters)
    {
        return CanNavigate(bottomSheet, parameters)
            && await CanNavigateAsync(bottomSheet, parameters).ConfigureAwait(false);
    }

    /// <summary>
    /// Invokes the <see cref="INavigationAware.OnNavigatedFrom"/> method on both the view and its ViewModel, if they implement <see cref="INavigationAware"/>.
    /// </summary>
    /// <param name="view"> The view object that is being navigated from. This may be a visual element or a container such as a page or bottom sheet.</param>
    /// <param name="parameters"> Optional navigation parameters associated with the navigation event. </param>
    internal static void OnNavigatedFrom(object? view, IBottomSheetNavigationParameters parameters)
    {
        InvokeViewAndViewModelAction<INavigationAware>(view, v => v.OnNavigatedFrom(parameters));
    }

    /// <summary>
    /// Invokes the <see cref="INavigationAware.OnNavigatedFrom"/> method on both the view and its ViewModel, if they implement <see cref="INavigationAware"/>.
    /// </summary>
    /// <param name="view"> The view object that is being navigated to. This may be a visual element or a container such as a page or bottom sheet.</param>
    /// <param name="parameters"> Optional navigation parameters associated with the navigation event. </param>
    internal static void OnNavigatedTo(object? view, IBottomSheetNavigationParameters parameters)
    {
        InvokeViewAndViewModelAction<INavigationAware>(view, v => v.OnNavigatedTo(parameters));
    }

    /// <summary>
    /// Determines whether <paramref name="bottomSheet"/> accepts being navigated away from.
    /// </summary>
    /// <param name="bottomSheet">The <see cref="IBottomSheet"/> to navigate from.</param>
    /// <param name="parameters">Navigation parameters.</param>
    /// <returns>Can navigate away from <see cref="IBottomSheet"/>.</returns>
    [SuppressMessage("Usage", "SuspiciousTypeConversion.Global: ReSharper disable once SuspiciousTypeConversion.Global", Justification = "False positive.")]
    private static bool CanNavigate(IBottomSheet bottomSheet, IBottomSheetNavigationParameters parameters)
    {
        bool canNavigate = true;

        if (bottomSheet.Parent is IConfirmNavigation
            || bottomSheet.Parent.BindingContext is IConfirmNavigation)
        {
            if (bottomSheet.Parent is IConfirmNavigation parentConfirmNavigation)
            {
                canNavigate = parentConfirmNavigation.CanNavigate(parameters);
            }

            if (bottomSheet.Parent.BindingContext is IConfirmNavigation parentBindableConfirmNavigation)
            {
                canNavigate = canNavigate
                    && parentBindableConfirmNavigation.CanNavigate(parameters);
            }
        }
        else if (bottomSheet is IConfirmNavigation
            || bottomSheet.BindingContext is IConfirmNavigation)
        {
            if (bottomSheet is IConfirmNavigation confirmNavigation)
            {
                canNavigate = confirmNavigation.CanNavigate(parameters);
            }

            if (bottomSheet.BindingContext is IConfirmNavigation bindableConfirmNavigation)
            {
                canNavigate = canNavigate
                    && bindableConfirmNavigation.CanNavigate(parameters);
            }
        }

        return canNavigate;
    }

    /// <summary>
    /// Determines whether <paramref name="bottomSheet"/> accepts being navigated away from.
    /// </summary>
    /// <param name="bottomSheet">The <see cref="IBottomSheet"/> to navigate from.</param>
    /// <param name="parameters">Navigation parameters.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [SuppressMessage("Usage", "SuspiciousTypeConversion.Global: ReSharper disable once SuspiciousTypeConversion.Global", Justification = "False positive.")]
    private static async Task<bool> CanNavigateAsync(IBottomSheet bottomSheet, IBottomSheetNavigationParameters parameters)
    {
        bool canNavigate = true;

        if (bottomSheet.Parent is IConfirmNavigationAsync
            || bottomSheet.Parent.BindingContext is IConfirmNavigationAsync)
        {
            if (bottomSheet.Parent is IConfirmNavigationAsync parentConfirmNavigation)
            {
                canNavigate = await parentConfirmNavigation.CanNavigateAsync(parameters).ConfigureAwait(false);
            }

            if (bottomSheet.Parent.BindingContext is IConfirmNavigationAsync parentBindableConfirmNavigation)
            {
                canNavigate = canNavigate
                    && await parentBindableConfirmNavigation.CanNavigateAsync(parameters).ConfigureAwait(false);
            }
        }
        else if (bottomSheet is IConfirmNavigationAsync
            || bottomSheet.BindingContext is IConfirmNavigationAsync)
        {
            if (bottomSheet is IConfirmNavigationAsync confirmNavigation)
            {
                canNavigate = await confirmNavigation.CanNavigateAsync(parameters).ConfigureAwait(false);
            }

            if (bottomSheet.BindingContext is IConfirmNavigationAsync bindableConfirmNavigation)
            {
                canNavigate = canNavigate
                    && await bindableConfirmNavigation.CanNavigateAsync(parameters).ConfigureAwait(false);
            }
        }

        return canNavigate;
    }

    private static void InvokeViewAndViewModelAction<T>(object? view, Action<T> action)
        where T : class
    {
        if (view is T viewAsT)
        {
            action(viewAsT);
        }

        if (view is BindableObject { BindingContext: T viewModelAsT })
        {
            action(viewModelAsT);
        }
    }
}