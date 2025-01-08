#pragma warning disable SA1200
using AView = Android.Views.View;
#pragma warning restore SA1200

namespace Plugin.Maui.BottomSheet.Platform.Android;

/// <summary>
/// <see cref="IBottomSheet.Peek"/> layout change callback.
/// </summary>
internal sealed class BottomSheetLayoutChangeListener : Java.Lang.Object, AView.IOnLayoutChangeListener
{
    private readonly WeakEventManager _eventManager = new();

    /// <summary>
    /// Peek layout changed
    /// </summary>
    public event EventHandler? LayoutChange
    {
        add => _eventManager.AddEventHandler(value);
        remove => _eventManager.RemoveEventHandler(value);
    }

    /// <inheritdoc/>
    public void OnLayoutChange(
        AView? v,
        int left,
        int top,
        int right,
        int bottom,
        int oldLeft,
        int oldTop,
        int oldRight,
        int oldBottom)
    {
        _eventManager.HandleEvent(
            this,
            EventArgs.Empty,
            nameof(LayoutChange));
    }
}