namespace Plugin.Maui.BottomSheet.Platform.MaciOS;

using UIKit;

/// <summary>
/// Provides utility methods for working with windows and safe areas on macOS and iOS platforms.
/// </summary>
internal static class WindowUtils
{
    /// <summary>
    /// Gets the safe area insets for the current UIViewController based on the device idiom.
    /// </summary>
    /// <returns>The safe area insets as UIEdgeInsets.</returns>
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