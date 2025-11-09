namespace Plugin.BottomSheet;

/// <summary>
/// Event arguments for bottom sheet state change events.
/// </summary>
public sealed class BottomSheetStateChangedEventArgs : EventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BottomSheetStateChangedEventArgs"/> class.
    /// </summary>
    /// <param name="state">The new state of the bottom sheet.</param>
    public BottomSheetStateChangedEventArgs(BottomSheetState state)
    {
        State = state;
    }

    /// <summary>
    /// Gets the new state of the bottom sheet.
    /// </summary>
    public BottomSheetState State { get; }
}