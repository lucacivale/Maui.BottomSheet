namespace Plugin.Maui.BottomSheet.PlatformViews;

using AndroidView = Android.Views.View;

/// <summary>
/// <see cref="IBottomSheet"/> layout change callback.
/// </summary>
public class BottomSheetLayoutChangeListener : Java.Lang.Object, AndroidView.IOnLayoutChangeListener
{
    private readonly MauiBottomSheet bottomSheet;

    /// <summary>
    /// Initializes a new instance of the <see cref="BottomSheetLayoutChangeListener"/> class.
    /// </summary>
    /// <param name="mauiBottomSheet"><see cref="MauiBottomSheet"/>.</param>
    public BottomSheetLayoutChangeListener(MauiBottomSheet mauiBottomSheet)

        // ReSharper disable once RedundantBaseConstructorCall
        : base()
    {
        bottomSheet = mauiBottomSheet;
    }

    /// <inheritdoc/>
    public void OnLayoutChange(
        AndroidView? v,
        int left,
        int top,
        int right,
        int bottom,
        int oldLeft,
        int oldTop,
        int oldRight,
        int oldBottom)
    {
        bottomSheet.SetPeek();
    }
}