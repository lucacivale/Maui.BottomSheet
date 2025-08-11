namespace Plugin.BottomSheet.Android;

using global::Android.Content;

/// <summary>
/// Custom bottom sheet dialog with enhanced event handling and back press support.
/// </summary>
internal sealed class BottomSheetDialog : Google.Android.Material.BottomSheet.BottomSheetDialog
{
    private readonly WeakEventManager _eventManager = new();
    private readonly BottomSheetCallback _bottomSheetCallback;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2213: Disposable fields should be disposed", Justification = "Is disposed in dismiss method.")]
    private BottomSheetDialogOnBackPressedCallback? _onBackPressedCallback;

    /// <summary>
    /// Initializes a new instance of the <see cref="BottomSheetDialog"/> class.
    /// </summary>
    /// <param name="context">The Android context.</param>
    /// <param name="theme">The theme resource ID.</param>
    /// <param name="bottomSheetCallback">The bottom sheet callback handler.</param>
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
    /// Occurs when the bottom sheet is canceled.
    /// </summary>
    public event EventHandler Canceled
    {
        add => _eventManager.AddEventHandler(value);
        remove => _eventManager.RemoveEventHandler(value);
    }

    /// <summary>
    /// Occurs when the back button is pressed.
    /// </summary>
    public event EventHandler BackPressed
    {
        add => _eventManager.AddEventHandler(value);
        remove => _eventManager.RemoveEventHandler(value);
    }

    /// <summary>
    /// Shows the bottom sheet dialog.
    /// </summary>
    public override void Show()
    {
        _onBackPressedCallback = new BottomSheetDialogOnBackPressedCallback(true);
        _onBackPressedCallback.BackPressed += OnBackPressedCallbackOnBackPressed;
        OnBackPressedDispatcher.AddCallback(_onBackPressedCallback);

        base.Show();
    }

    /// <summary>
    /// Cancels the bottom sheet dialog.
    /// </summary>
    public override void Cancel()
    {
        _eventManager.HandleEvent(this, EventArgs.Empty, nameof(Canceled));
    }

    /// <summary>
    /// Handles the back button press event.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1422: Validate platform compatibility - obsoleted APIs", Justification = "Needed for backwards compatibility.")]
    public override void OnBackPressed()
    {
        base.OnBackPressed();

        _eventManager.HandleEvent(this, EventArgs.Empty, nameof(BackPressed));
    }

    /// <summary>
    /// Dismisses the bottom sheet dialog and cleans up resources.
    /// </summary>
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

    /// <summary>
    /// Handles the back pressed callback event.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    private void OnBackPressedCallbackOnBackPressed(object? sender, EventArgs e)
    {
        _eventManager.HandleEvent(this, EventArgs.Empty, nameof(BackPressed));
    }
}