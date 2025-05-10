using Android.Content;

namespace Plugin.Maui.BottomSheet.Platform.Android;

/// <inheritdoc />
internal sealed class BottomSheetDialog : Google.Android.Material.BottomSheet.BottomSheetDialog
{
    private readonly WeakEventManager _eventManager = new();
    private readonly BottomSheetCallback _bottomSheetCallback;

    /// <summary>
    /// Initializes a new instance of the <see cref="BottomSheetDialog"/> class.
    /// </summary>
    /// <param name="context"><see cref="Context"/>.</param>
    /// <param name="theme">Theme Id.</param>
    /// <param name="bottomSheetCallback"><see cref="BottomSheetCallback"/>.</param>
    public BottomSheetDialog(
        Context context,
        int theme,
        BottomSheetCallback bottomSheetCallback)
        : base(context, theme)
    {
        _bottomSheetCallback = bottomSheetCallback;

        Behavior.AddBottomSheetCallback(_bottomSheetCallback);
    }

    /// <summary>
    /// BottomSheet canceled.
    /// </summary>
    public event EventHandler Canceled
    {
        add => _eventManager.AddEventHandler(value);
        remove => _eventManager.RemoveEventHandler(value);
    }

    /// <inheritdoc />
    public override void Cancel()
    {
        _eventManager.HandleEvent(this, EventArgs.Empty, nameof(Canceled));
    }

    /// <inheritdoc />
    public override void Dismiss()
    {
        Behavior.RemoveBottomSheetCallback(_bottomSheetCallback);

        base.Dismiss();
    }
}