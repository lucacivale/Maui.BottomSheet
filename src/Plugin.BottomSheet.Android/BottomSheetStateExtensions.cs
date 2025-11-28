using Google.Android.Material.BottomSheet;

namespace Plugin.BottomSheet.Android;

/// <summary>
/// Provides extension methods for converting between <see cref="BottomSheetState"/> enum values
/// and Android platform constants used by the BottomSheetBehavior.
/// </summary>
internal static class BottomSheetStateExtensions
{
    /// <summary>
    /// Converts a BottomSheetState enum value to the corresponding Android platform state constant.
    /// </summary>
    /// <param name="state">The bottom sheet state to convert.</param>
    /// <returns>The Android BottomSheetBehavior state constant.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the state is not recognized.</exception>
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
    /// Converts an Android platform state constant to the corresponding BottomSheetState enum value.
    /// </summary>
    /// <param name="state">The Android state constant to convert.</param>
    /// <returns>The corresponding BottomSheetState enum value.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the state constant is not recognized.</exception>
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