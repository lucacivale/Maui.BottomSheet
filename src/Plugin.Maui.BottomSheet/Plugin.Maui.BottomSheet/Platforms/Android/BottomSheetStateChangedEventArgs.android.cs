// ReSharper disable once CheckNamespace
namespace Plugin.Maui.BottomSheet.Platforms.Android;

public sealed class BottomSheetStateChangedEventArgs : EventArgs
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