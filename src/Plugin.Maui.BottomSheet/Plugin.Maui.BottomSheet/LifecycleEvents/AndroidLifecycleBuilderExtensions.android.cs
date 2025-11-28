using Microsoft.Maui.LifecycleEvents;

namespace Plugin.Maui.BottomSheet.LifecycleEvents;

/// <summary>
/// Provides extension methods for <see cref="IAndroidLifecycleBuilder"/> to support BottomSheet-related lifecycle events.
/// </summary>
public static class AndroidLifecycleBuilderExtensions
{
    /// <summary>
    /// Adds a delegate to be executed when the back button is pressed while a BottomSheet is visible.
    /// </summary>
    /// <param name="lifecycle">The <see cref="IAndroidLifecycleBuilder"/> instance to add the delegate to.</param>
    /// <param name="onBottomSheetBackPressed">An action to execute when the back button is pressed.</param>
    /// <returns>The <see cref="IAndroidLifecycleBuilder"/> instance with the delegate added.</returns>
    /// <remarks>
    /// LIMITATION: The delegate will only be invoked if <see cref="IBottomSheet.IsCancelable"/> is false.
    /// </remarks>
    public static IAndroidLifecycleBuilder OnBottomSheetBackPressed(this IAndroidLifecycleBuilder lifecycle, Action<Android.App.Activity?> onBottomSheetBackPressed)
    {
        lifecycle.AddEvent(AndroidLifecycle.BottomSheetBackPressedEventName, onBottomSheetBackPressed);

        return lifecycle;
    }
}