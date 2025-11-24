namespace Plugin.BottomSheet.iOSMacCatalyst;

/// <summary>
/// Provides extension methods for the <see cref="UIWindow"/> class specific to the iOS and Mac Catalyst platforms.
/// </summary>
internal static class UiWindowExtensions
{
    /// <summary>
    /// Finds the topmost <see cref="UIViewController"/> from the window's view hierarchy.
    /// </summary>
    /// <param name="window">The <see cref="UIWindow"/> from which to locate the topmost <see cref="UIViewController"/>.</param>
    /// <returns>The topmost <see cref="UIViewController"/> or null if no view controller is found.</returns>
    internal static UIViewController? CurrentViewController(this UIWindow? window)
    {
        UIViewController? topViewController = window?.RootViewController;

        if (topViewController is not null
            && topViewController.PresentedViewController is not null)
        {
            topViewController = topViewController.PresentedViewController;

            while (topViewController?.PresentedViewController is not null)
            {
                topViewController = topViewController.PresentedViewController;
            }
        }

        return topViewController;
    }
}