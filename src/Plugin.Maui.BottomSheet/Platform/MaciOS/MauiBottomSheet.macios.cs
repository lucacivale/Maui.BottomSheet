namespace Plugin.Maui.BottomSheet.Platform.MaciOS;

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Maui;
using Microsoft.Maui.Platform;
using Plugin.Maui.BottomSheet.Navigation;
using Plugin.BottomSheet;
using UIKit;

/// <summary>
/// Represents a platform-specific implementation of a bottom sheet for macOS and iOS, integrated into the .NET MAUI framework.
/// </summary>
public sealed class MauiBottomSheet : UIView, IEnumerable<UIView>
{
    private readonly IMauiContext _mauiContext;
    private readonly TaskCompletionSource _isAttachedToWindowTcs;

    private Plugin.BottomSheet.iOSMacCatalyst.BottomSheet? _bottomSheet;

    private IBottomSheet? _virtualView;

    private bool _isAttachedToWindow;

    /// <summary>
    /// Initializes a new instance of the <see cref="MauiBottomSheet"/> class.
    /// MAUI implementation of a bottom sheet for macOS and iOS platforms.
    /// </summary>
    /// <param name="mauiContext">The MAUI context associated with the bottom sheet.</param>
    public MauiBottomSheet(IMauiContext mauiContext)
    {
        _mauiContext = mauiContext;
        _isAttachedToWindowTcs = new TaskCompletionSource();
    }

    /// <summary>
    /// Gets a value indicating whether the bottom sheet is currently open.
    /// </summary>
    public bool IsOpen => _bottomSheet?.IsOpen == true;

    /// <summary>
    /// Gets the underlying UIViewController instance associated with the MAUI bottom sheet.
    /// </summary>
    public Plugin.BottomSheet.iOSMacCatalyst.BottomSheet? BottomSheet => _bottomSheet;

    /// <summary>
    /// Returns an enumerator that iterates through the collection of subviews.
    /// </summary>
    /// <returns>An enumerator for the collection of <see cref="UIView"/> objects.</returns>
    IEnumerator<UIView> IEnumerable<UIView>.GetEnumerator()
        => (IEnumerator<UIView>)Subviews.GetEnumerator();

    /// <summary>
    /// Called when the view is moved to a window, and ensures any pending operations
    /// waiting for the view to be attached to a window are completed.
    /// </summary>
    public override void MovedToWindow()
    {
        base.MovedToWindow();

        _isAttachedToWindow = true;
        _isAttachedToWindowTcs.TrySetResult();
    }

    /// <summary>
    /// Sets the virtual view for the bottom sheet.
    /// </summary>
    /// <param name="virtualView">The virtual view to associate with this bottom sheet.</param>
    public void SetView(IBottomSheet virtualView)
    {
        _virtualView = virtualView;
    }

    /// <summary>
    /// Releases resources allocated by the bottom sheet and performs any necessary cleanup operations.
    /// </summary>
    public void Cleanup()
    {
        _bottomSheet?.Dispose();

        RemoveFromSuperview();
    }

    /// <summary>
    /// Configures the bottom sheet to indicate whether it can be dismissed by user interaction.
    /// </summary>
    public void SetIsCancelable()
    {
        if (_bottomSheet is null)
        {
            return;
        }

        _bottomSheet.ModalInPresentation = _virtualView?.IsCancelable != true;
    }

    /// <summary>
    /// Opens the bottom sheet asynchronously, initializing the required states and configurations.
    /// </summary>
    /// <param name="force">A boolean indicating whether to forcefully open the bottom sheet, even if not attached to the window.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task OpenAsync(bool force = false)
    {
        if (_virtualView is null)
        {
            return;
        }

        if (!_isAttachedToWindow
            && force == false)
        {
            using CancellationTokenSource cts = new(TimeSpan.FromSeconds(20));
            await _isAttachedToWindowTcs.Task.WaitAsync(cts.Token).ConfigureAwait(true);
        }

        _bottomSheet = new Plugin.BottomSheet.iOSMacCatalyst.BottomSheet();
        _bottomSheet.StateChanged += BottomSheetOnStateChanged;
        _bottomSheet.Canceled += BottomSheetOnCanceled;
        _bottomSheet.FrameChanged += BottomSheetOnFrameChanged;
        _bottomSheet.LayoutChanged += BottomSheetOnLayoutChanged;

        SetStates();
        SetIsCancelable();
        SetCurrentState();

        SetWindowBackgroundColor();
        SetBottomSheetBackgroundColor();
        SetIsModal();
        SetCornerRadius();

        UIView view = _virtualView.ContainerView.ToPlatform(_mauiContext);
        view.UpdateAutomationId(_virtualView);

        _bottomSheet.SetContentView(view);

        _virtualView.OnOpeningBottomSheet();

        if (Window is null
            && _virtualView.Parent.ToPlatform(_mauiContext) is UIView parent)
        {
            parent.Window?.AddSubview(this);
        }

        await _bottomSheet.OpenAsync(Window).ConfigureAwait(true);

        SetPeekHeight();
        SetFrame();
        SetIsDraggable();

        _virtualView.OnOpenedBottomSheet();
    }

    /// <summary>
    /// Closes the bottom sheet asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous close operation.</returns>
    public async Task CloseAsync()
    {
        if (_bottomSheet is null
            || _virtualView is null)
        {
            return;
        }

        _virtualView.OnClosingBottomSheet();

        _bottomSheet.StateChanged -= BottomSheetOnStateChanged;
        _bottomSheet.Canceled -= BottomSheetOnCanceled;
        _bottomSheet.FrameChanged -= BottomSheetOnFrameChanged;
        _bottomSheet.LayoutChanged -= BottomSheetOnLayoutChanged;

        await _bottomSheet.CloseAsync().ConfigureAwait(true);

        SetFrame(true);
        _virtualView.OnClosedBottomSheet();

        _bottomSheet.Dispose();
        _bottomSheet = null;
    }

    /// <summary>
    /// Sets the open state of the bottom sheet asynchronously based on its virtual view configuration.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task SetIsOpenAsync()
    {
        if (_virtualView?.IsOpen == true)
        {
            if (_bottomSheet?.IsOpen == false
                || _bottomSheet is null)
            {
                await OpenAsync().ConfigureAwait(true);
            }
        }
        else
        {
            if (_bottomSheet?.IsOpen == true)
            {
                Cancel();
            }
        }
    }

    /// <summary>
    /// Cancels the current bottom sheet operation, if applicable.
    /// </summary>
    public void Cancel()
    {
        _bottomSheet?.Cancel();
    }

    /// <summary>
    /// Configures the bottom sheet to determine whether it can be dragged by the user.
    /// </summary>
    public void SetIsDraggable()
    {
        if (_bottomSheet is null)
        {
            return;
        }

        _bottomSheet.Draggable = _virtualView?.IsDraggable == true;
    }

    /// <summary>
    /// Configures the available states and the current state for the bottom sheet.
    /// </summary>
    public void SetStates()
    {
        if (_virtualView is null
            || _bottomSheet is null)
        {
            return;
        }

        _bottomSheet.States = _virtualView.States;
        _bottomSheet.State = _virtualView.CurrentState;
    }

    /// <summary>
    /// Updates the current state of the bottom sheet based on its virtual view.
    /// </summary>
    public void SetCurrentState()
    {
        if (_virtualView is null
            || _bottomSheet is null)
        {
            return;
        }

        _bottomSheet.State = _virtualView.CurrentState;
    }

    /// <summary>
    /// Sets the peek height for the bottom sheet. This determines the default visible height when the bottom sheet is in its collapsed state.
    /// </summary>
    public void SetPeekHeight()
    {
        if (_virtualView is null
            || _bottomSheet is null)
        {
            return;
        }

        _bottomSheet.PeekHeight = _virtualView.PeekHeight;
    }

    /// <summary>
    /// Configures the bottom sheet to be either modal or non-modal.
    /// </summary>
    public void SetIsModal()
    {
        if (_bottomSheet is null)
        {
            return;
        }

        _bottomSheet.IsModal = _virtualView?.IsModal == true;
    }

    /// <summary>
    /// Sets the background color of the bottom sheet based on the current virtual view's background color.
    /// </summary>
    public void SetBottomSheetBackgroundColor()
    {
        if (_bottomSheet is null)
        {
            return;
        }

        // ReSharper disable once ConditionalAccessQualifierIsNonNullableAccordingToAPIContract
        _bottomSheet.BackgroundColor = _virtualView?.BackgroundColor?.ToPlatform();
    }

    /// <summary>
    /// Sets the corner radius for the bottom sheet to align with the desired appearance.
    /// </summary>
    public void SetCornerRadius()
    {
        if (_bottomSheet?.SheetPresentationController is not null
            && _virtualView is not null)
        {
            _bottomSheet.SheetPresentationController.PreferredCornerRadius = _virtualView.CornerRadius;
        }
    }

    /// <summary>
    /// Sets the background color of the window associated with the bottom sheet.
    /// </summary>
    public void SetWindowBackgroundColor()
    {
        if (_bottomSheet is null)
        {
            return;
        }

        // ReSharper disable once ConditionalAccessQualifierIsNonNullableAccordingToAPIContract
        _bottomSheet.WindowBackgroundColor = _virtualView?.WindowBackgroundColor?.ToPlatform();
    }

    /// <summary>
    /// Releases the resources used by the <see cref="MauiBottomSheet"/> instance.
    /// </summary>
    /// <param name="disposing">A boolean value indicating whether to release managed resources.</param>
    protected override void Dispose(bool disposing)
    {
        if (!disposing)
        {
            return;
        }

        _bottomSheet?.Dispose();

        base.Dispose(disposing);
    }

    /// <summary>
    /// Handles the state change event for the bottom sheet and updates the associated virtual view and bottom sheet state.
    /// </summary>
    /// <param name="sender">The source of the event triggering the state change.</param>
    /// <param name="e">The event arguments containing details of the state change.</param>
    private void BottomSheetOnStateChanged(object? sender, BottomSheetStateChangedEventArgs e)
    {
        if (_virtualView is null)
        {
            return;
        }

        _virtualView.CurrentState = e.NewState;
        SetFrame();
    }

    /// <summary>
    /// Handles the cancellation event of the bottom sheet and manages navigation when the bottom sheet is dismissed.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event arguments indicating the cancellation event.</param>
    [SuppressMessage("Usage", "VSTHRD100: Avoid async void methods", Justification = "Is okay here.")]
    [SuppressMessage("Design", "CA1031: Do not catch general exception types", Justification = "Catch all exceptions to prevent crash.")]
    private async void BottomSheetOnCanceled(object? sender, EventArgs e)
    {
        try
        {
            if (_virtualView is null
                || _bottomSheet is null)
            {
                return;
            }

            BottomSheetNavigationParameters parameters = BottomSheetNavigationParameters.Empty();

            bool closed = true;
            if (_mauiContext.Services.GetRequiredService<IBottomSheetNavigationService>().NavigationStack().Contains(_virtualView))
            {
                INavigationResult result = await _mauiContext.Services.GetRequiredService<IBottomSheetNavigationService>().GoBackAsync(parameters).ConfigureAwait(true);

                closed = !(result.Success == false
                    || result.Cancelled);
            }
            else
            {
                if (_virtualView.IsCancelable
                    && await MvvmHelpers.ConfirmNavigationAsync(_virtualView, parameters).ConfigureAwait(true))
                {
                    await CloseAsync().ConfigureAwait(true);
                    MvvmHelpers.OnNavigatedFrom(_virtualView, parameters);
                    MvvmHelpers.OnNavigatedTo(_virtualView.GetPageParent(), parameters);
                }
                else
                {
                    closed = false;
                }
            }

            if (closed == false)
            {
                _bottomSheet.State = _virtualView.CurrentState;
            }

            _virtualView.IsOpen = closed == false;
        }
        catch (Exception ex)
        {
            Trace.TraceError("Invoking IConfirmNavigation or IConfirmNavigationAsync failed: {0}", ex);
        }
    }

    /// <summary>
    /// Handles frame change events triggered by the bottom sheet and updates the frame in the associated virtual view.
    /// </summary>
    /// <param name="sender">The source triggering the event, typically the bottom sheet instance.</param>
    /// <param name="e">The new dimensions of the frame.</param>
    private void BottomSheetOnFrameChanged(object? sender, EventArgs e)
    {
        if (_virtualView is null)
        {
            return;
        }

        SetFrame();
    }

    /// <summary>
    /// Handles the layout changes of the bottom sheet.
    /// </summary>
    /// <param name="sender">The source of the event, typically the bottom sheet instance.</param>
    /// <param name="e">The event data that contains information about the layout change.</param>
    private void BottomSheetOnLayoutChanged(object? sender, EventArgs e)
    {
        _virtualView?.OnLayoutChanged();
    }

    /// <summary>
    /// Updates the virtual view's frame based on the bottom sheet's current state.
    /// </summary>
    /// <param name="isClosed">Indicates whether the bottom sheet is closed. If true, the frame will be set to zero dimensions.</param>
    private void SetFrame(bool isClosed = false)
    {
        if (_virtualView is null
            || _bottomSheet is null)
        {
            return;
        }

        _virtualView.Frame = isClosed ? Microsoft.Maui.Graphics.Rect.Zero : new Microsoft.Maui.Graphics.Rect(
            _bottomSheet.Frame.X,
            _bottomSheet.Frame.Y,
            _bottomSheet.Frame.Width,
            _bottomSheet.Frame.Height);
    }
}