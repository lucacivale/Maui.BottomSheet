#pragma warning disable SA1200
using AsyncAwaitBestPractices;
using AView = Android.Views.View;
#pragma warning restore SA1200

namespace Plugin.BottomSheet.Android;

/// <summary>
/// Monitors layout changes for bottom sheet views and content.
/// </summary>
internal sealed class BottomSheetLayoutChangeListener : Java.Lang.Object, AView.IOnLayoutChangeListener
{
    private readonly WeakEventManager _eventManager = new();

    /// <summary>
    /// Occurs when the monitored view's layout changes.
    /// </summary>
    public event EventHandler? LayoutChange
    {
        add => _eventManager.AddEventHandler(value);
        remove => _eventManager.RemoveEventHandler(value);
    }

    /// <summary>
    /// Called when the layout of the monitored view changes.
    /// </summary>
    /// <param name="v">The view whose layout changed.</param>
    /// <param name="left">The new left position.</param>
    /// <param name="top">The new top position.</param>
    /// <param name="right">The new right position.</param>
    /// <param name="bottom">The new bottom position.</param>
    /// <param name="oldLeft">The previous left position.</param>
    /// <param name="oldTop">The previous top position.</param>
    /// <param name="oldRight">The previous right position.</param>
    /// <param name="oldBottom">The previous bottom position.</param>
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
        _eventManager.RaiseEvent(
            this,
            EventArgs.Empty,
            nameof(LayoutChange));
    }
}