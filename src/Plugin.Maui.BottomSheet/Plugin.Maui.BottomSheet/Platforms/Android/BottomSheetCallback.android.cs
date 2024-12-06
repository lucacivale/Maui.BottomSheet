using AView = Android.Views.View;

// ReSharper disable once CheckNamespace
namespace Plugin.Maui.BottomSheet.Platforms.Android;

using Google.Android.Material.BottomSheet;

public sealed class BottomSheetCallback : BottomSheetBehavior.BottomSheetCallback
{
    private readonly WeakEventManager _eventManager = new();

    /// <summary>
    /// BottomSheet state changed.
    /// </summary>
    public event EventHandler<BottomSheetStateChangedEventArgs> StateChanged
    {
        add => _eventManager.AddEventHandler(value);
        remove => _eventManager.RemoveEventHandler(value);
    }

    /// <inheritdoc />
    public override void OnSlide(AView bottomSheet, float newState)
    {
    }

    /// <inheritdoc />
    public override void OnStateChanged(AView p0, int p1)
    {
        if (p1 is BottomSheetBehavior.StateCollapsed
            or BottomSheetBehavior.StateHalfExpanded
            or BottomSheetBehavior.StateExpanded)
        {
            _eventManager.HandleEvent(
                p0,
                new BottomSheetStateChangedEventArgs(p1.ToBottomSheetState()),
                nameof(StateChanged));
        }
    }
}