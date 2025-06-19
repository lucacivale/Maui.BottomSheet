using System.Diagnostics.CodeAnalysis;
using Plugin.Maui.BottomSheet.Navigation;

namespace Plugin.Maui.BottomSheet;

/// <summary>
/// Provides helper methods for MVVM navigation patterns and bottom sheet lifecycle management.
/// </summary>
internal static class MvvmHelpers
{
    /// <summary>
    /// Determines whether the specified bottom sheet can be navigated away from by checking both synchronous and asynchronous navigation confirmation.
    /// </summary>
    /// <param name="bottomSheet">The bottom sheet to navigate from.</param>
    /// <param name="parameters">The navigation parameters.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains true if navigation is allowed; otherwise, false.</returns>
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
    /// Invokes the OnNavigatedFrom method on both the view and its ViewModel if they implement INavigationAware.
    /// </summary>
    /// <param name="view">The view object that is being navigated from.</param>
    /// <param name="parameters">The navigation parameters associated with the navigation event.</param>
    internal static void OnNavigatedFrom(object? view, IBottomSheetNavigationParameters parameters)
    {
        InvokeViewAndViewModelAction<INavigationAware>(view, v => v.OnNavigatedFrom(parameters));
    }

    /// <summary>
    /// Invokes the OnNavigatedTo method on both the view and its ViewModel if they implement INavigationAware.
    /// </summary>
    /// <param name="view">The view object that is being navigated to.</param>
    /// <param name="parameters">The navigation parameters associated with the navigation event.</param>
    internal static void OnNavigatedTo(object? view, IBottomSheetNavigationParameters parameters)
    {
        InvokeViewAndViewModelAction<INavigationAware>(view, v => v.OnNavigatedTo(parameters));
    }

    /// <summary>
    /// Determines whether the specified bottom sheet can be navigated away from using synchronous navigation confirmation.
    /// </summary>
    /// <param name="bottomSheet">The bottom sheet to navigate from.</param>
    /// <param name="parameters">The navigation parameters.</param>
    /// <returns>True if navigation is allowed; otherwise, false.</returns>
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
    /// Determines whether the specified bottom sheet can be navigated away from using asynchronous navigation confirmation.
    /// </summary>
    /// <param name="bottomSheet">The bottom sheet to navigate from.</param>
    /// <param name="parameters">The navigation parameters.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains true if navigation is allowed; otherwise, false.</returns>
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

    /// <summary>
    /// Invokes an action on both the view and its ViewModel if they implement the specified interface type.
    /// </summary>
    /// <typeparam name="T">The interface type to check for and invoke the action on.</typeparam>
    /// <param name="view">The view object to check and invoke the action on.</param>
    /// <param name="action">The action to invoke on objects that implement the interface.</param>
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