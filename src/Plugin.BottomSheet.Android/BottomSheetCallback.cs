using AsyncAwaitBestPractices;
using Google.Android.Material.BottomSheet;

namespace Plugin.BottomSheet.Android;

/// <summary>
/// Callback class used to receive updates when the state of a bottom sheet changes or slides.
/// </summary>
/// <remarks>
/// This class listens for state changes and slide updates on a bottom sheet and provides corresponding event notifications.
/// </remarks>
internal sealed class BottomSheetCallback : BottomSheetBehavior.BottomSheetCallback
{
    private readonly WeakEventManager _eventManager = new();

    /// <summary>
    /// Event triggered when the state of the bottom sheet changes.
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
    /// <param name="newState">The amount the bottom sheet has been dragged, represented as a float.</param>
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
                new BottomSheetStateChangedEventArgs(p1.ToBottomSheetState(), p1.ToBottomSheetState()),
                nameof(StateChanged));
        }
    }
}