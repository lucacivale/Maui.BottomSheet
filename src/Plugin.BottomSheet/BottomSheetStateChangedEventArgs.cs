namespace Plugin.BottomSheet;

/// <summary>
/// Encapsulates information about the state change of a bottom sheet.
/// </summary>
public sealed class BottomSheetStateChangedEventArgs : EventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BottomSheetStateChangedEventArgs"/> class.
    /// Provides data for the bottom sheet state change event.
    /// </summary>
    /// <param name="oldState">Previous state of the bottom sheet, representing its level of expansion or visibility before the state change occurred.</param>
    /// <param name="newState">State of the bottom sheet, reflecting its level of expansion or visibility.</param>
    public BottomSheetStateChangedEventArgs(BottomSheetState oldState, BottomSheetState newState)
    {
        OldState = oldState;
        NewState = newState;
    }

    /// <summary>
    /// Gets the previous state of the bottom sheet, representing its level of expansion or visibility
    /// before the state change occurred.
    /// </summary>
    public BottomSheetState OldState { get; }

    /// <summary>
    /// Gets the current state of the bottom sheet, reflecting its level of expansion or visibility.
    /// </summary>
    public BottomSheetState NewState { get; }
}