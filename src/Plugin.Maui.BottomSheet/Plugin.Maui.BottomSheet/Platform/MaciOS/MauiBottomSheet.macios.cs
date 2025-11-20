namespace Plugin.Maui.BottomSheet.Platform.MaciOS;

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using AsyncAwaitBestPractices;
using Microsoft.Maui;
using Microsoft.Maui.Platform;
using Plugin.Maui.BottomSheet.Navigation;
using Plugin.BottomSheet;
using UIKit;

/// <summary>
/// MAUI implementation of bottom sheet for macOS and iOS platforms.
/// </summary>
internal sealed class MauiBottomSheet : UIView
{
    private readonly IMauiContext _mauiContext;
    private readonly TaskCompletionSource _isAttachedToWindowTcs;

    private Plugin.BottomSheet.iOSMacCatalyst.BottomSheet? _bottomSheet;
    private IBottomSheet? _virtualView;

    private bool _isAttachedToWindow;

    /// <summary>
    /// Initializes a new instance of the <see cref="MauiBottomSheet"/> class.
    /// </summary>
    /// <param name="mauiContext">The MAUI context for platform services.</param>
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
    /// Called when the view is moved to a window, handles auto-opening if needed.
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
    /// Cleans up resources used by the bottom sheet.
    /// </summary>
    public void Cleanup()
    {
        _bottomSheet?.Dispose();
    }

    /// <summary>
    /// Sets whether the bottom sheet can be canceled by user interaction.
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
    /// Opens the bottom sheet asynchronously.
    /// </summary>
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
        SetIsDraggable();
        SetCurrentState();

        SetWindowBackgroundColor();
        SetBottomSheetBackgroundColor();
        SetIsModal();
        SetCornerRadius();

        _bottomSheet.SetContentView(_virtualView.ContainerView.ToPlatform(_mauiContext));

        _virtualView.OnOpeningBottomSheet();

        await _bottomSheet.OpenAsync(Window).ConfigureAwait(true);

        SetPeekHeight();
        SetFrame();

        _virtualView.OnOpenedBottomSheet();
    }

    /// <summary>
    /// Closes the bottom sheet asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
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
    /// Sets the open state of the bottom sheet based on the virtual view configuration.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <summary>
    /// Sets the open state of the bottom sheet.
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

    public void Cancel()
    {
        _bottomSheet?.Cancel();
    }

    /// <summary>
    /// Sets whether the bottom sheet is draggable.
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
    /// Sets the available states and current state for the bottom sheet.
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
    /// Sets the current state of the bottom sheet.
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
    /// Sets the peek height for the bottom sheet.
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
    /// Sets whether the bottom sheet should be presented modally.
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
    /// Sets the background color for the bottom sheet.
    /// </summary>
    public void SetBottomSheetBackgroundColor()
    {
        if (_bottomSheet is null)
        {
            return;
        }

        _bottomSheet.BackgroundColor = _virtualView?.BackgroundColor?.ToPlatform();
    }

    /// <summary>
    /// Sets the corner radius for the bottom sheet.
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
    /// Sets the window background color for the bottom sheet.
    /// </summary>
    public void SetWindowBackgroundColor()
    {
        if (_bottomSheet is null)
        {
            return;
        }

        _bottomSheet.WindowBackgroundColor = _virtualView?.WindowBackgroundColor?.ToPlatform();
    }

    /// <summary>
    /// Releases resources used by the view.
    /// </summary>
    /// <param name="disposing">True if disposing managed resources.</param>
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
    /// Handles state change events from the bottom sheet and updates the virtual view.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The state change event arguments.</param>
    private void BottomSheetOnStateChanged(object? sender, BottomSheetStateChangedEventArgs e)
    {
        if (_virtualView is null
            || _bottomSheet is null)
        {
            return;
        }

        _bottomSheet.State = e.State;
        SetFrame();
    }

    /// <summary>
    /// Handles confirm dismiss events and manages navigation when the bottom sheet is dismissed.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
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
            else
            {
                _virtualView.IsOpen = false;
            }
        }
        catch (Exception ex)
        {
            Trace.TraceError("Invoking IConfirmNavigation or IConfirmNavigationAsync failed: {0}", ex);
        }
    }

    /// <summary>
    /// Handles frame change events from the bottom sheet and updates the virtual view frame.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The frame change event arguments.</param>
    private void BottomSheetOnFrameChanged(object? sender, Rect e)
    {
        if (_virtualView is null)
        {
            return;
        }

        SetFrame();
    }

    private void BottomSheetOnLayoutChanged(object? sender, EventArgs e)
    {
        if (_virtualView is null)
        {
            return;
        }

        _virtualView.OnLayoutChanged();
    }

    /// <summary>
    /// Updates the virtual view's frame based on the bottom sheet's current state.
    /// </summary>
    /// <param name="isClosed">Whether the bottom sheet is closed.</param>
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