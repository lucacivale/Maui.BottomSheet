#pragma warning disable SA1200
using AView = Android.Views;
#pragma warning restore SA1200

// ReSharper disable once CheckNamespace
namespace Plugin.Maui.BottomSheet.Platforms.Android;

/// <summary>
/// <see cref="IBottomSheet"/> layout change callback.
/// </summary>
internal sealed class BottomSheetHeaderLayoutChangeListener : Java.Lang.Object, AView.View.IOnLayoutChangeListener
{
    private readonly BottomSheetHeader _bottomSheetHeader;

    /// <summary>
    /// Initializes a new instance of the <see cref="BottomSheetHeaderLayoutChangeListener"/> class.
    /// </summary>
    /// <param name="bottomSheetHeader"><see cref="BottomSheetHeader"/>.</param>
    public BottomSheetHeaderLayoutChangeListener(BottomSheetHeader bottomSheetHeader)
    {
        _bottomSheetHeader = bottomSheetHeader;
    }

    /// <inheritdoc />
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
}