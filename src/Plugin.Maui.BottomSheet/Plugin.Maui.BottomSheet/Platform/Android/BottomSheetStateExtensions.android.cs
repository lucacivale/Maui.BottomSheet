namespace Plugin.Maui.BottomSheet.Platform.Android;

using Google.Android.Material.BottomSheet;

/// <summary>
/// <see cref="BottomSheetState"/> extension methods.
/// </summary>
internal static class BottomSheetStateExtensions
{
    /// <summary>
    /// Get <see cref="BottomSheetBehavior"/> state constant from <see cref="BottomSheetState"/> enum.
    /// </summary>
    /// <param name="state">State.</param>
    /// <returns>Behavior state constant.</returns>
    public static int ToPlatformState(this BottomSheetState state)
    {
        return state switch
        {
            BottomSheetState.Peek => BottomSheetBehavior.StateCollapsed,
            BottomSheetState.Medium => BottomSheetBehavior.StateHalfExpanded,
            BottomSheetState.Large => BottomSheetBehavior.StateExpanded,
            _ => throw new ArgumentOutOfRangeException(nameof(state), state, null),
        };
    }

    /// <summary>
    /// Get <see cref="BottomSheetState"/> from <see cref="BottomSheetBehavior"/> state constant.
    /// </summary>
    /// <param name="state">Constant value.</param>
    /// <returns>Behavior state.</returns>
    public static BottomSheetState ToBottomSheetState(this int state)
    {
        return state switch
        {
            BottomSheetBehavior.StateCollapsed => BottomSheetState.Peek,
            BottomSheetBehavior.StateHalfExpanded => BottomSheetState.Medium,
            BottomSheetBehavior.StateExpanded => BottomSheetState.Large,
            _ => throw new ArgumentOutOfRangeException(nameof(state), state, null),
        };
    }
}