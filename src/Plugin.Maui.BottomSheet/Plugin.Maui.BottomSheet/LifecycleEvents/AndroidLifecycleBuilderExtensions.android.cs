using Microsoft.Maui.LifecycleEvents;

namespace Plugin.Maui.BottomSheet.LifecycleEvents;

/// <summary>
/// IAndroidLifecycleBuilder extension methods.
/// </summary>
public static class AndroidLifecycleBuilderExtensions
{
    /// <summary>
    /// Add BottomSheet back pressed delegate.
    /// </summary>
    /// <param name="lifecycle"><see cref="IAndroidLifecycleBuilder"/>.</param>
    /// <param name="onBottomSheetBackPressed">Action to execute on back press.</param>
    /// <returns><see cref="IAndroidLifecycleBuilder"/> with added delegate.</returns>
    /// <remarks>LIMITATION: Delegate will only be invoked if <see cref="IBottomSheet.IsCancelable"/> is false.</remarks>
    public static IAndroidLifecycleBuilder OnBottomSheetBackPressed(this IAndroidLifecycleBuilder lifecycle, Action<Android.App.Activity?> onBottomSheetBackPressed)
    {
        lifecycle.AddEvent(AndroidLifecycle.BottomSheetBackPressedEventName, onBottomSheetBackPressed);

        return lifecycle;
    }
}