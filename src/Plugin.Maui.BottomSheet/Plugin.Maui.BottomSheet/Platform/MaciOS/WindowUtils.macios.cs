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
        return WindowStateManager.Default.GetCurrentUIViewController()?.View?.SafeAreaInsets ?? UIEdgeInsets.Zero;
    }
}