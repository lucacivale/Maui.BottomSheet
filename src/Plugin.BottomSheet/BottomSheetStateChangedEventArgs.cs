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
    /// <param name="state">State of the bottom sheet, reflecting its level of expansion or visibility.</param>
    public BottomSheetStateChangedEventArgs(BottomSheetState state)
    {
        State = state;
    }

    /// <summary>
    /// Gets the current state of the bottom sheet, reflecting its level of expansion or visibility.
    /// </summary>
    public BottomSheetState State { get; }
}