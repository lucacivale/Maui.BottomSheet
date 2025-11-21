namespace Plugin.BottomSheet.iOSMacCatalyst;

/// <summary>
/// Provides extension methods for converting between the BottomSheetState enum and platform-specific state constants
/// for use in iOS and Mac Catalyst environments.
/// </summary>
internal static class BottomSheetStateExtensions
{
    /// <summary>
    /// Converts a BottomSheetState enum value to the corresponding iOS UISheetPresentationControllerDetentIdentifier constant.
    /// </summary>
    /// <param name="state">The bottom sheet state to convert.</param>
    /// <returns>The iOS UISheetPresentationControllerDetentIdentifier constant corresponding to the provided BottomSheetState.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the provided BottomSheetState value is not recognized.</exception>
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
    /// Converts a UISheetPresentationControllerDetentIdentifier constant to the corresponding BottomSheetState enum value.
    /// </summary>
    /// <param name="state">The platform state constant to convert.</param>
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