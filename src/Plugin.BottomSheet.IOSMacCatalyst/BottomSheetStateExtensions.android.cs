namespace Plugin.BottomSheet.IOSMacCatalyst;

/// <summary>
/// Extension methods for converting between BottomSheetState enum and Android platform constants.
/// </summary>
internal static class BottomSheetStateExtensions
{
    /// <summary>
    /// Converts a BottomSheetState enum value to the corresponding Android platform state constant.
    /// </summary>
    /// <param name="state">The bottom sheet state to convert.</param>
    /// <returns>The Android BottomSheetBehavior state constant.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the state is not recognized.</exception>
    public static UISheetPresentationControllerDetentIdentifier ToPlatformState(this BottomSheetState state)
    {
        return state switch
        {
            BottomSheetState.Peek => UISheetPresentationControllerDetentIdentifier.Unknown,
            BottomSheetState.Medium => UISheetPresentationControllerDetentIdentifier.Medium,
            BottomSheetState.Large => UISheetPresentationControllerDetentIdentifier.Large,
            _ => throw new ArgumentOutOfRangeException(nameof(state), state, null),
        };
    }

    /// <summary>
    /// Converts an Android platform state constant to the corresponding BottomSheetState enum value.
    /// </summary>
    /// <param name="state">The Android state constant to convert.</param>
    /// <returns>The corresponding BottomSheetState enum value.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the state constant is not recognized.</exception>
    public static BottomSheetState ToBottomSheetState(this UISheetPresentationControllerDetentIdentifier state)
    {
        return state switch
        {
            UISheetPresentationControllerDetentIdentifier.Unknown => BottomSheetState.Peek,
            UISheetPresentationControllerDetentIdentifier.Medium => BottomSheetState.Medium,
            UISheetPresentationControllerDetentIdentifier.Large => BottomSheetState.Large,
            _ => throw new ArgumentOutOfRangeException(nameof(state), state, null),
        };
    }
}