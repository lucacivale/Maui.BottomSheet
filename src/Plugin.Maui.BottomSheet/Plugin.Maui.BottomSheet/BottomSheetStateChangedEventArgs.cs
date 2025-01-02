namespace Plugin.Maui.BottomSheet;

/// <summary>
/// Event args for bottom sheet state changes.
/// </summary>
internal sealed class BottomSheetStateChangedEventArgs : EventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BottomSheetStateChangedEventArgs"/> class.
    /// </summary>
    /// <param name="state">New state.</param>
    public BottomSheetStateChangedEventArgs(BottomSheetState state)
    {
        State = state;
    }

    /// <summary>
    /// Gets new state.
    /// </summary>
    public BottomSheetState State { get; }
}