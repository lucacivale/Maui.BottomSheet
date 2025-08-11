using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Plugin.Maui.BottomSheet.Navigation;

namespace Plugin.Maui.BottomSheet.Platform.MaciOS;

using AsyncAwaitBestPractices;
using UIKit;

/// <summary>
/// MAUI implementation of bottom sheet for macOS and iOS platforms.
/// </summary>
internal sealed class MauiBottomSheet : UIView
{
    private readonly IMauiContext _mauiContext;
    private readonly BottomSheet _bottomSheet;
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    private IBottomSheet? _virtualView;

    /// <summary>
    /// Initializes a new instance of the <see cref="MauiBottomSheet"/> class.
    /// </summary>
    /// <param name="mauiContext">The MAUI context for platform services.</param>
    public MauiBottomSheet(IMauiContext mauiContext)
    {
        _mauiContext = mauiContext;
        _bottomSheet = new BottomSheet(_mauiContext);
        _bottomSheet.StateChanged += BottomSheetOnStateChanged;
        _bottomSheet.ConfirmDismiss += BottomSheetOnConfirmDismiss;
        _bottomSheet.FrameChanged += BottomSheetOnFrameChanged;
    }

    /// <summary>
    /// Gets a value indicating whether the bottom sheet is currently open.
    /// </summary>
    public bool IsOpen => _bottomSheet.IsOpen;

    /// <summary>
    /// Called when the view is moved to a window, handles auto-opening if needed.
    /// </summary>
    public override void MovedToWindow()
    {
        base.MovedToWindow();

        if (_virtualView?.IsOpen == true
            && _bottomSheet.IsOpen == false)
        {
            SetIsOpenAsync().SafeFireAndForget(continueOnCapturedContext: false);
        }
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
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task CleanupAsync()
    {
        await _bottomSheet.DisposeAsync().ConfigureAwait(true);
    }

    /// <summary>
    /// Sets whether the bottom sheet can be canceled by user interaction.
    /// </summary>
    public void SetIsCancelable()
    {
        _bottomSheet.SetIsCancelable(_virtualView?.IsCancelable == true);
    }

    /// <summary>
    /// Sets whether the bottom sheet should display a drag handle.
    /// </summary>
    public void SetHasHandle()
    {
        _bottomSheet.SetHasHandle(_virtualView?.HasHandle == true);
    }

    /// <summary>
    /// Sets the header configuration for the bottom sheet.
    /// </summary>
    public void SetHeader()
    {
        if (_virtualView?.Header is null)
        {
            return;
        }

        _bottomSheet.SetHeader(_virtualView.Header, _virtualView.BottomSheetStyle.HeaderStyle);
    }

    /// <summary>
    /// Shows or hides the header based on the virtual view configuration.
    /// </summary>
    public void SetShowHeader()
    {
        if (_virtualView?.ShowHeader == true)
        {
            _bottomSheet.ShowHeader();
        }
        else
        {
            _bottomSheet.HideHeader();
        }
    }

    /// <summary>
    /// Opens the bottom sheet asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task OpenAsync()
    {
        if (_virtualView is null)
        {
            return;
        }

        _virtualView.OnOpeningBottomSheet();
        await _bottomSheet.OpenAsync(_virtualView).ConfigureAwait(true);
        _virtualView.OnOpenedBottomSheet();
    }

    /// <summary>
    /// Closes the bottom sheet asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task CloseAsync()
    {
        if (_virtualView?.IsOpen == true)
        {
            _virtualView.OnClosingBottomSheet();
            await _bottomSheet.CloseAsync().ConfigureAwait(true);
            _virtualView.OnClosedBottomSheet();
        }
    }

    /// <summary>
    /// Sets the open state of the bottom sheet based on the virtual view configuration.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task SetIsOpenAsync()
    {
        if (_virtualView?.IsOpen == true)
        {
            await _semaphore.WaitAsync().ConfigureAwait(true);

            if (IsOpen)
            {
                return;
            }

            _virtualView.OnOpeningBottomSheet();
            await _bottomSheet.OpenAsync(_virtualView).ConfigureAwait(true);
            _virtualView.OnOpenedBottomSheet();

            _semaphore.Release();
        }
        else
        {
            _virtualView?.OnClosingBottomSheet();
            await _bottomSheet.CloseAsync().ConfigureAwait(true);
            _virtualView?.OnClosedBottomSheet();
        }
    }

    /// <summary>
    /// Sets whether the bottom sheet is draggable.
    /// </summary>
    public void SetIsDraggable()
    {
        _bottomSheet.SetIsDraggable(_virtualView?.IsDraggable == true);
    }

    /// <summary>
    /// Sets the available states and current state for the bottom sheet.
    /// </summary>
    public void SetStates()
    {
        if (_virtualView is null)
        {
            return;
        }

        _bottomSheet.SetStates(_virtualView.States);
        _bottomSheet.SetState(_virtualView.CurrentState);
    }

    /// <summary>
    /// Sets the current state of the bottom sheet.
    /// </summary>
    public void SetCurrentState()
    {
        if (_virtualView is null)
        {
            return;
        }

        _bottomSheet.SetState(_virtualView.CurrentState);
    }

    /// <summary>
    /// Sets the peek height for the bottom sheet.
    /// </summary>
    public void SetPeekHeight()
    {
        if (_virtualView is null)
        {
            return;
        }

        _bottomSheet.SetPeekHeight(_virtualView.PeekHeight);
    }

    /// <summary>
    /// Sets the content for the bottom sheet.
    /// </summary>
    public void SetContent()
    {
        if (_virtualView?.Content is null)
        {
            return;
        }

        _bottomSheet.SetContent(_virtualView.Content);
    }

    /// <summary>
    /// Sets whether the bottom sheet should be presented modally.
    /// </summary>
    public void SetIsModal()
    {
        _bottomSheet.SetIsModal(_virtualView?.IsModal == true);
    }

    /// <summary>
    /// Sets the padding for the bottom sheet content.
    /// </summary>
    public void SetPadding()
    {
        if (_virtualView?.Padding is null)
        {
            return;
        }

        _bottomSheet.SetPadding(_virtualView.Padding);
    }

    /// <summary>
    /// Sets the background color for the bottom sheet.
    /// </summary>
    public void SetBottomSheetBackgroundColor()
    {
        if (_virtualView?.BackgroundColor is null)
        {
            return;
        }

        _bottomSheet.SetBackgroundColor(_virtualView.BackgroundColor);
    }

    /// <summary>
    /// Sets whether the bottom sheet should ignore safe area constraints.
    /// </summary>
    public void SetIgnoreSafeArea()
    {
        _bottomSheet.SetIgnoreSafeArea(_virtualView?.IgnoreSafeArea == true);
    }

    /// <summary>
    /// Sets the corner radius for the bottom sheet.
    /// </summary>
    public void SetCornerRadius()
    {
        _bottomSheet.SetCornerRadius(_virtualView?.CornerRadius ?? 0);
    }

    /// <summary>
    /// Sets the window background color for the bottom sheet.
    /// </summary>
    public void SetWindowBackgroundColor()
    {
        _bottomSheet.SetWindowBackgroundColor(_virtualView?.WindowBackgroundColor ?? Colors.Transparent);
    }

    /// <summary>
    /// Sets the bottom sheet style configuration.
    /// </summary>
    public void SetBottomSheetStyle()
    {
        if (_virtualView?.BottomSheetStyle is not null)
        {
            _bottomSheet.SetHeaderStyle(_virtualView.BottomSheetStyle.HeaderStyle);
        }
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

        _bottomSheet.Dispose();
        _semaphore.Dispose();

        base.Dispose(disposing);
    }

    /// <summary>
    /// Handles state change events from the bottom sheet and updates the virtual view.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The state change event arguments.</param>
    private void BottomSheetOnStateChanged(object? sender, BottomSheetStateChangedEventArgs e)
    {
        if (_virtualView is null)
        {
            return;
        }

        var state = e.State;

        if (!_virtualView.States.IsStateAllowed(state))
        {
            state = _virtualView.CurrentState;
        }

        _virtualView.CurrentState = state;
        _bottomSheet.SetState(_virtualView.CurrentState);
    }

    /// <summary>
    /// Handles confirm dismiss events and manages navigation when the bottom sheet is dismissed.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    [SuppressMessage("Usage", "VSTHRD100: Avoid async void methods", Justification = "Is okay here.")]
    [SuppressMessage("Design", "CA1031: Do not catch general exception types", Justification = "Catch all exceptions to prevent crash.")]
    private async void BottomSheetOnConfirmDismiss(object? sender, EventArgs e)
    {
        try
        {
            if (_virtualView is null)
            {
                return;
            }

            var parameters = BottomSheetNavigationParameters.Empty();

            if (_mauiContext.Services.GetRequiredService<IBottomSheetNavigationService>().NavigationStack().Contains(_virtualView))
            {
                await _mauiContext.Services.GetRequiredService<IBottomSheetNavigationService>().GoBackAsync(parameters).ConfigureAwait(true);
            }
            else
            {
                if (_virtualView.IsOpen
                    && await MvvmHelpers.ConfirmNavigationAsync(_virtualView, parameters).ConfigureAwait(true))
                {
                    MvvmHelpers.OnNavigatedFrom(_virtualView, parameters);
                    MvvmHelpers.OnNavigatedTo(_virtualView.GetPageParent(), parameters);
                    _virtualView.IsOpen = false;
                }
            }
        }
        catch
        {
            Trace.TraceError("Invoking IConfirmNavigation or IConfirmNavigationAsync failed.");
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

        _virtualView.Frame = e;
    }
}