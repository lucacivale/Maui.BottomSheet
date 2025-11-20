namespace Plugin.BottomSheet.IOSMacCatalyst;

internal static class UiWindowExtensions
{
    /// <summary>
    /// Find top most <see cref="UIViewController"/>.
    /// </summary>
    /// <returns>Top most <see cref="UIViewController"/>.</returns>
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