namespace Plugin.Maui.BottomSheet.Platform.MaciOS;

using UIKit;

/// <inheritdoc />
internal sealed class BottomSheetControllerDelegate : UISheetPresentationControllerDelegate
{
    private readonly WeakEventManager _eventManager = new();

    /// <summary>
    /// BottomSheet state changed.
    /// </summary>
    public event EventHandler<BottomSheetStateChangedEventArgs> StateChanged
    {
        add => _eventManager.AddEventHandler(value);
        remove => _eventManager.RemoveEventHandler(value);
    }

    /// <summary>
    /// Confirm dismiss.
    /// </summary>
    public event EventHandler ConfirmDismiss
    {
        add => _eventManager.AddEventHandler(value);
        remove => _eventManager.RemoveEventHandler(value);
    }

    /// <inheritdoc/>
    // DidAttemptToDismiss is only called if UISheetPresentationController is closed via swipe. Tapping on backrgound only calls ShouldDismiss.
    // Maybe will be fixed or works as designed. File bug report.
    public override bool ShouldDismiss(UIPresentationController presentationController)
    {
        _eventManager.HandleEvent(
            presentationController,
            EventArgs.Empty,
            nameof(ConfirmDismiss));

        return false;
    }

    /// <inheritdoc/>
    public override void DidChangeSelectedDetentIdentifier(UISheetPresentationController sheetPresentationController)
    {
        var state = sheetPresentationController.SelectedDetentIdentifier switch
        {
            UISheetPresentationControllerDetentIdentifier.Unknown => BottomSheetState.Peek,
            UISheetPresentationControllerDetentIdentifier.Medium => BottomSheetState.Medium,
            UISheetPresentationControllerDetentIdentifier.Large => BottomSheetState.Large,
            _ => throw new ArgumentOutOfRangeException(sheetPresentationController.SelectedDetentIdentifier.ToString()),
        };

        _eventManager.HandleEvent(
            sheetPresentationController,
            new BottomSheetStateChangedEventArgs(state),
            nameof(StateChanged));
    }
}