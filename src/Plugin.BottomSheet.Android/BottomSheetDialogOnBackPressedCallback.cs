using AndroidX.Activity;
using AsyncAwaitBestPractices;

namespace Plugin.BottomSheet.Android;

/// <summary>
/// A callback class designed to manage back button presses specifically for bottom sheet dialogs.
/// It ensures appropriate behavior when the back button is pressed while a bottom sheet dialog is active.
/// </summary>
internal sealed class BottomSheetDialogOnBackPressedCallback : OnBackPressedCallback
{
    private readonly WeakEventManager _eventManager = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="BottomSheetDialogOnBackPressedCallback"/> class.
    /// Handles back button press events for bottom sheet dialogs.
    /// </summary>
    /// <param name="enabled">Indicates whether the callback is enabled.</param>
    public BottomSheetDialogOnBackPressedCallback(bool enabled)
        : base(enabled)
    {
    }

    /// <summary>
    /// Event that is triggered when the back button is pressed while the bottom sheet dialog is active.
    /// </summary>
    public event EventHandler BackPressed
    {
        add => _eventManager.AddEventHandler(value);
        remove => _eventManager.RemoveEventHandler(value);
    }

    /// <summary>
    /// Handles the back button press event by raising the associated BackPressed event.
    /// </summary>
    public override void HandleOnBackPressed()
    {
        _eventManager.RaiseEvent(this, EventArgs.Empty, nameof(BackPressed));
    }
}