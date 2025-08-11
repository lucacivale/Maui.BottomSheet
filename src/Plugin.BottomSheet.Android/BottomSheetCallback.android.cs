namespace Plugin.BottomSheet.Android;

using AsyncAwaitBestPractices;
using Google.Android.Material.BottomSheet;

/// <summary>
/// Handles bottom sheet behavior state change callbacks.
/// </summary>
internal sealed class BottomSheetCallback : BottomSheetBehavior.BottomSheetCallback
{
    private readonly WeakEventManager _eventManager = new();

    /// <summary>
    /// Occurs when the bottom sheet state changes.
    /// </summary>
    public event EventHandler<BottomSheetStateChangedEventArgs> StateChanged
    {
        add => _eventManager.AddEventHandler(value);
        remove => _eventManager.RemoveEventHandler(value);
    }

    /// <summary>
    /// Called when the bottom sheet slides.
    /// </summary>
    /// <param name="bottomSheet">The bottom sheet view.</param>
    /// <param name="newState">The new slide state.</param>
    public override void OnSlide(View bottomSheet, float newState)
    {
    }

    /// <summary>
    /// Called when the bottom sheet state changes.
    /// </summary>
    /// <param name="p0">The bottom sheet view.</param>
    /// <param name="p1">The new state constant.</param>
    public override void OnStateChanged(View p0, int p1)
    {
        if (p1 is BottomSheetBehavior.StateCollapsed
            or BottomSheetBehavior.StateHalfExpanded
            or BottomSheetBehavior.StateExpanded)
        {
            _eventManager.RaiseEvent(
                p0,
                new BottomSheetStateChangedEventArgs(p1.ToBottomSheetState()),
                nameof(StateChanged));
        }
    }
}