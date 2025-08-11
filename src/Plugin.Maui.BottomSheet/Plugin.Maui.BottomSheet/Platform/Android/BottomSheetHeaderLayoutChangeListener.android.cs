#pragma warning disable SA1200
using AView = Android.Views;
#pragma warning restore SA1200

namespace Plugin.Maui.BottomSheet.Platform.Android;

/// <summary>
/// Monitors layout changes for bottom sheet header views.
/// </summary>
internal sealed class BottomSheetHeaderLayoutChangeListener : Java.Lang.Object, AView.View.IOnLayoutChangeListener
{
    private BottomSheetHeader _bottomSheetHeader;

    /// <summary>
    /// Initializes a new instance of the <see cref="BottomSheetHeaderLayoutChangeListener"/> class.
    /// </summary>
    /// <param name="bottomSheetHeader">The header to monitor for layout changes.</param>
    public BottomSheetHeaderLayoutChangeListener(BottomSheetHeader bottomSheetHeader)
    {
        _bottomSheetHeader = bottomSheetHeader;
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
        AView.View? v,
        int left,
        int top,
        int right,
        int bottom,
        int oldLeft,
        int oldTop,
        int oldRight,
        int oldBottom)
    {
        _bottomSheetHeader.RaiseLayoutChangedEvent();
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (!disposing)
        {
            return;
        }

        _bottomSheetHeader = null!;
    }
}