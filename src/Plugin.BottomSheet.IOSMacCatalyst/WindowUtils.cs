namespace Plugin.BottomSheet.IOSMacCatalyst;

/// <summary>
/// Window utilities.
/// </summary>
internal static class WindowUtils
{
    /// <summary>
    /// Find top most <see cref="UIViewController"/>.
    /// </summary>
    /// <returns>Top most <see cref="UIViewController"/>.</returns>
    internal static UIViewController? GetTopViewController()
    {
        NSSet<UIScene> scenes = UIApplication.SharedApplication.ConnectedScenes;
        UIWindowScene? windowScene = scenes.ToArray().OfType<UIWindowScene>().FirstOrDefault(scene =>
            scene.Session.Role == UIWindowSceneSessionRole.Application);
        UIWindow? window = windowScene?.Windows.FirstOrDefault();
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