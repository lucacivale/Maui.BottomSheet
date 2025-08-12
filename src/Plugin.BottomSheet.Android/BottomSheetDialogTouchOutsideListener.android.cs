using AndroidX.AppCompat.App;

namespace Plugin.BottomSheet.Android;

/// <summary>
/// Handles touch events outside the bottom sheet for non-modal behavior.
/// </summary>
internal sealed class BottomSheetDialogTouchOutsideListener : Java.Lang.Object, View.IOnTouchListener
{
    private readonly AppCompatActivity _activity;

    /// <summary>
    /// Initializes a new instance of the <see cref="BottomSheetDialogTouchOutsideListener"/> class.
    /// </summary>
    /// <param name="activity">The host activity.</param>
    public BottomSheetDialogTouchOutsideListener(AppCompatActivity activity)
    {
        _activity = activity;
    }

    /// <summary>
    /// Handles touch events and dispatches them to the activity.
    /// </summary>
    /// <param name="v">The view that was touched.</param>
    /// <param name="e">The motion event details.</param>
    /// <returns>Always returns false to allow further event processing.</returns>
    public bool OnTouch(View? v, MotionEvent? e)
    {
        _ = _activity.DispatchTouchEvent(e);

        return false;
    }
}