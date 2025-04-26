using System.Diagnostics.CodeAnalysis;
using Plugin.Maui.BottomSheet.Navigation;

namespace Plugin.Maui.BottomSheet;

// ReSharper disable once InconsistentNaming

/// <summary>
/// <see cref="IBottomSheet"/> extension methods.
/// </summary>
internal static class IBottomSheetExtensions
{
    /// <summary>
    /// Is navigation confirmation configured for <paramref name="bottomSheet"/>.
    /// </summary>
    /// <param name="bottomSheet">BottomSheet.</param>
    /// <returns>Is navigation confirmation configured.</returns>
    [SuppressMessage("Usage", "SuspiciousTypeConversion.Global: ReSharper disable once SuspiciousTypeConversion.Global", Justification = "False positive.")]
    internal static bool ShouldConfirmNavigation(this IBottomSheet bottomSheet)
    {
        return bottomSheet.Parent is IConfirmNavigation
            || bottomSheet.Parent.BindingContext is IConfirmNavigation
            || bottomSheet.Parent is IConfirmNavigationAsync
            || bottomSheet.Parent.BindingContext is IConfirmNavigationAsync
            || bottomSheet is IConfirmNavigation
            || bottomSheet.BindingContext is IConfirmNavigation
            || bottomSheet is IConfirmNavigationAsync
            || bottomSheet.BindingContext is IConfirmNavigationAsync;
    }

    /// <summary>
    /// Get the <see cref="ContentPage"/> parent of <see cref="IBottomSheet"/>.
    /// </summary>
    /// <param name="bottomSheet"><see cref="IBottomSheet"/>.</param>
    /// <returns><see cref="ContentPage"/> parent.</returns>
    internal static Page? GetPageParent(this IBottomSheet bottomSheet)
    {
        Page? page = null;

        var parent = bottomSheet.Parent;

        if (parent is Shell shell)
        {
            page = shell.CurrentPage;
        }
        else
        {
            while (parent is not null)
            {
                if (parent is ContentPage contentPage)
                {
                    page = contentPage;
                    break;
                }

                parent = parent.Parent;
            }
        }

        return page;
    }
}