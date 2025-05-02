#pragma warning disable SA1200
using Android.Views;
using AndroidX.AppCompat.App;
using AView = Android.Views.View;
#pragma warning restore SA1200

namespace Plugin.Maui.BottomSheet.Platform.Android;

/// <summary>
/// Touch outside listener for <see cref="BottomSheet"/>.
/// </summary>
internal sealed class BottomSheetDialogTouchOutsideListener : Java.Lang.Object, AView.IOnTouchListener
{
    private readonly AppCompatActivity _activity;

    /// <summary>
    /// Initializes a new instance of the <see cref="BottomSheetDialogTouchOutsideListener"/> class.
    /// </summary>
    /// <param name="activity"><see cref="AppCompatActivity"/>.</param>
    public BottomSheetDialogTouchOutsideListener(AppCompatActivity activity)
    {
        _activity = activity;
    }

    /// <inheritdoc />
    public bool OnTouch(AView? v, MotionEvent? e)
    {
        _activity.DispatchTouchEvent(e);

        return false;
    }
}
