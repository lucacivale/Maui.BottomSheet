using Android.Content;

namespace Plugin.Maui.BottomSheet.Platform.Android;

/// <inheritdoc />
internal sealed class BottomSheetDialog : Google.Android.Material.BottomSheet.BottomSheetDialog
{
    private readonly WeakEventManager _eventManager = new();
    private readonly BottomSheetCallback _bottomSheetCallback;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2213: Disposable fields should be disposed", Justification = "Is disposed in dismiss method.")]
    private BottomSheetDialogOnBackPressedCallback? _onBackPressedCallback;

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
    }

    /// <summary>
    /// BottomSheet canceled.
    /// </summary>
    public event EventHandler Canceled
    {
        add => _eventManager.AddEventHandler(value);
        remove => _eventManager.RemoveEventHandler(value);
    }

    /// <summary>
    /// Back button pressed.
    /// </summary>
    public event EventHandler BackPressed
    {
        add => _eventManager.AddEventHandler(value);
        remove => _eventManager.RemoveEventHandler(value);
    }

    /// <inheritdoc />
    public override void Show()
    {
        _onBackPressedCallback = new BottomSheetDialogOnBackPressedCallback(true);
        _onBackPressedCallback.BackPressed += OnBackPressedCallbackOnBackPressed;
        OnBackPressedDispatcher.AddCallback(_onBackPressedCallback);

        base.Show();
    }

    /// <inheritdoc />
    public override void Cancel()
    {
        _eventManager.HandleEvent(this, EventArgs.Empty, nameof(Canceled));
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1422: Validate platform compatibility - obsoleted APIs", Justification = "Needed for backwards compatibility.")]
    public override void OnBackPressed()
    {
        base.OnBackPressed();

        _eventManager.HandleEvent(this, EventArgs.Empty, nameof(BackPressed));
    }

    /// <inheritdoc />
    public override void Dismiss()
    {
        Behavior.RemoveBottomSheetCallback(_bottomSheetCallback);

        if (_onBackPressedCallback is not null)
        {
            _onBackPressedCallback.BackPressed -= OnBackPressedCallbackOnBackPressed;
            _onBackPressedCallback.Remove();
            _onBackPressedCallback.Dispose();
        }

        base.Dismiss();
    }

    private void OnBackPressedCallbackOnBackPressed(object? sender, EventArgs e)
    {
        _eventManager.HandleEvent(this, EventArgs.Empty, nameof(BackPressed));
    }
}