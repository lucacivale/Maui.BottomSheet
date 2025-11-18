namespace Plugin.BottomSheet.IOSMacCatalyst;

using Plugin.BottomSheet;
using UIKit;

/// <summary>
/// Handles presentation controller delegate methods for bottom sheet behavior and state management.
/// </summary>
internal sealed class BottomSheetDelegate : UISheetPresentationControllerDelegate
{
    private readonly AsyncAwaitBestPractices.WeakEventManager _eventManager = new();

    /// <summary>
    /// Occurs when the bottom sheet state changes.
    /// </summary>
    public event EventHandler<BottomSheetStateChangedEventArgs> StateChanged
    {
        add => _eventManager.AddEventHandler(value);
        remove => _eventManager.RemoveEventHandler(value);
    }

    /// <summary>
    /// Occurs when the bottom sheet dismissal needs confirmation.
    /// </summary>
    public event EventHandler ConfirmDismiss
    {
        add => _eventManager.AddEventHandler(value);
        remove => _eventManager.RemoveEventHandler(value);
    }

    /// <summary>
    /// Determines whether the presentation controller should dismiss when touched outside or swiped.
    /// Note: DidAttemptToDismiss is only called for swipe dismissal, while ShouldDismiss handles background taps.
    /// </summary>
    /// <param name="presentationController">The presentation controller requesting dismissal.</param>
    /// <returns>Always returns false to prevent automatic dismissal and trigger confirmation.</returns>
    public override bool ShouldDismiss(UIPresentationController presentationController)
    {
        _eventManager.RaiseEvent(
            presentationController,
            EventArgs.Empty,
            nameof(ConfirmDismiss));

        return false;
    }

    /// <summary>
    /// Called when the selected detent identifier changes, translating it to bottom sheet state.
    /// </summary>
    /// <param name="sheetPresentationController">The sheet presentation controller with the changed detent.</param>
    public override void DidChangeSelectedDetentIdentifier(UISheetPresentationController sheetPresentationController)
    {
        var state = sheetPresentationController.SelectedDetentIdentifier switch
        {
            UISheetPresentationControllerDetentIdentifier.Unknown => BottomSheetState.Peek,
            UISheetPresentationControllerDetentIdentifier.Medium => BottomSheetState.Medium,
            UISheetPresentationControllerDetentIdentifier.Large => BottomSheetState.Large,
            _ => throw new ArgumentOutOfRangeException(sheetPresentationController.SelectedDetentIdentifier.ToString()),
        };

        _eventManager.RaiseEvent(
            sheetPresentationController,
            new BottomSheetStateChangedEventArgs(state),
            nameof(StateChanged));
    }
}