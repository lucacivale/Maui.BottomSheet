using AndroidX.Activity;

namespace Plugin.Maui.BottomSheet.Platform.Android;

/// <summary>
/// Handles back button press events for bottom sheet dialogs.
/// </summary>
internal sealed class BottomSheetDialogOnBackPressedCallback : OnBackPressedCallback
{
    private readonly WeakEventManager _eventManager = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="BottomSheetDialogOnBackPressedCallback"/> class.
    /// </summary>
    /// <param name="enabled">Whether the callback is enabled.</param>
    public BottomSheetDialogOnBackPressedCallback(bool enabled)
        : base(enabled)
    {
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
    /// Handles the back button press event.
    /// </summary>
    public override void HandleOnBackPressed()
    {
        _eventManager.HandleEvent(this, EventArgs.Empty, nameof(BackPressed));
    }
}