namespace Plugin.Maui.BottomSheet.Platform.MaciOS;

using UIKit;

/// <summary>
/// Window utility methods.
/// </summary>
internal static class WindowUtils
{
    /// <summary>
    /// Gets safe area insets for current UIViewController.
    /// </summary>
    /// <returns>Safe area insets.</returns>
    public static UIEdgeInsets CurrentSafeAreaInsets()
    {
        if (DeviceInfo.Idiom == DeviceIdiom.Tablet)
        {
            using var scenes = UIApplication.SharedApplication.ConnectedScenes;
            var windowScene = scenes.ToArray().OfType<UIWindowScene>().FirstOrDefault(scene =>
                scene.Session.Role == UIWindowSceneSessionRole.Application);
            return windowScene?.Windows.FirstOrDefault()?.SafeAreaInsets ?? UIEdgeInsets.Zero;
        }
        else
        {
            return WindowStateManager.Default.GetCurrentUIViewController()?.View?.SafeAreaInsets ?? UIEdgeInsets.Zero;
        }
    }
}