namespace Plugin.BottomSheet.iOSMacCatalyst;

using Plugin.BottomSheet;
using UIKit;

/// <summary>
/// Provides delegate methods for managing bottom sheet behaviors and transitions, including handling detent changes and dismissal events.
/// </summary>
internal sealed class BottomSheetDelegate : UISheetPresentationControllerDelegate
{
    private readonly AsyncAwaitBestPractices.WeakEventManager _eventManager = new();

    /// <summary>
    /// Triggered when the state of the bottom sheet transitions to a different level.
    /// </summary>
    public event EventHandler<BottomSheetStateChangedEventArgs> StateChanged
    {
        add => _eventManager.AddEventHandler(value);
        remove => _eventManager.RemoveEventHandler(value);
    }

    /// <summary>
    /// Occurs when a dismissal of the bottom sheet is triggered, allowing confirmation before it is completed.
    /// </summary>
    public event EventHandler ConfirmDismiss
    {
        add => _eventManager.AddEventHandler(value);
        remove => _eventManager.RemoveEventHandler(value);
    }

    /// <summary>
    /// Determines whether the presentation controller should dismiss when touched outside or swiped.
    /// Note: DidAttemptToDismiss is only called for swipe dismissal, while ShouldDismiss handles background taps and swipes.
    /// </summary>
    /// <param name="presentationController">The presentation controller requesting dismissal.</param>
    /// <returns>Returns false to prevent automatic dismissal and trigger confirmation.</returns>
    public override bool ShouldDismiss(UIPresentationController presentationController)
    {
        _eventManager.RaiseEvent(
            presentationController,
            EventArgs.Empty,
            nameof(ConfirmDismiss));

        return false;
    }

    /// <summary>
    /// Invoked when the selected detent identifier of the sheet presentation controller changes.
    /// Converts the detent identifier to a corresponding bottom sheet state and raises a state change event.
    /// </summary>
    /// <param name="sheetPresentationController">The sheet presentation controller with the updated detent identifier.</param>
    public override void DidChangeSelectedDetentIdentifier(UISheetPresentationController sheetPresentationController)
    {
        BottomSheetState state = sheetPresentationController.SelectedDetentIdentifier switch
        {
            UISheetPresentationControllerDetentIdentifier.Unknown => BottomSheetState.Peek,
            UISheetPresentationControllerDetentIdentifier.Medium => BottomSheetState.Medium,
            UISheetPresentationControllerDetentIdentifier.Large => BottomSheetState.Large,
            _ => throw new ArgumentOutOfRangeException(sheetPresentationController.SelectedDetentIdentifier.ToString()),
        };

        _eventManager.RaiseEvent(
            sheetPresentationController,
            new BottomSheetStateChangedEventArgs(state, state),
            nameof(StateChanged));
    }
}