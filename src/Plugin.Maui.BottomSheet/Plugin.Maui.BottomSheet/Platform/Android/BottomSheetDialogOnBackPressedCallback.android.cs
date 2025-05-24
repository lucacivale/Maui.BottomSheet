using AndroidX.Activity;

namespace Plugin.Maui.BottomSheet.Platform.Android;

/// <inheritdoc />
internal sealed class BottomSheetDialogOnBackPressedCallback : OnBackPressedCallback
{
    private readonly WeakEventManager _eventManager = new();

    /// <inheritdoc />
    public BottomSheetDialogOnBackPressedCallback(bool enabled)
        : base(enabled)
    {
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
    public override void HandleOnBackPressed()
    {
        _eventManager.HandleEvent(this, EventArgs.Empty, nameof(BackPressed));
    }
}