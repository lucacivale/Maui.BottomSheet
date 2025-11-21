using AsyncAwaitBestPractices;

namespace Plugin.BottomSheet.Android;

/// <summary>
/// An event listener designed to monitor and respond to layout changes
/// that occur in views associated with bottom sheets in Android.
/// </summary>
internal sealed class BottomSheetLayoutChangeListener : Java.Lang.Object, View.IOnLayoutChangeListener
{
    private readonly WeakEventManager _eventManager = new();

    /// <summary>
    /// Raised when the layout of a monitored bottom sheet view changes.
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
    /// <param name="left">The new left position of the view.</param>
    /// <param name="top">The new top position of the view.</param>
    /// <param name="right">The new right position of the view.</param>
    /// <param name="bottom">The new bottom position of the view.</param>
    /// <param name="oldLeft">The previous left position of the view.</param>
    /// <param name="oldTop">The previous top position of the view.</param>
    /// <param name="oldRight">The previous right position of the view.</param>
    /// <param name="oldBottom">The previous bottom position of the view.</param>
    public void OnLayoutChange(
        View? v,
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