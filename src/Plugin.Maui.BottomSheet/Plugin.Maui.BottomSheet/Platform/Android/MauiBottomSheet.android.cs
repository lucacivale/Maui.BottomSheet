using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using AsyncAwaitBestPractices;
using Microsoft.Maui.LifecycleEvents;
using Plugin.Maui.BottomSheet.Navigation;
using AActivity = Android.App.Activity;
using AndroidLifecycle = Plugin.Maui.BottomSheet.LifecycleEvents.AndroidLifecycle;
#pragma warning disable SA1200
using Android.Content;
using AndroidView = Android.Views.View;
#pragma warning restore SA1200

namespace Plugin.Maui.BottomSheet.Platform.Android;

using Microsoft.Maui.Platform;

/// <summary>
/// Android platform implementation of the MAUI bottom sheet view handler.
/// </summary>
internal sealed class MauiBottomSheet : AndroidView
{
    private readonly IMauiContext _mauiContext;
    private readonly BottomSheet _bottomSheet;
    private readonly Context _context;

    private IBottomSheet? _virtualView;

    /// <summary>
    /// Initializes a new instance of the <see cref="MauiBottomSheet"/> class.
    /// </summary>
    /// <param name="mauiContext">The MAUI context for platform services.</param>
    /// <param name="context">The Android context.</param>
    public MauiBottomSheet(IMauiContext mauiContext, Context context)
        : base(context)
    {
        _mauiContext = mauiContext;
        _context = context;
        _bottomSheet = new BottomSheet(_context, _mauiContext);
        _bottomSheet.Opened += BottomSheetOnOpened;
        _bottomSheet.Closed += BottomSheetOnClosed;
        _bottomSheet.StateChanged += BottomSheetOnStateChanged;
        _bottomSheet.BackPressed += BottomSheetOnBackPressed;
        _bottomSheet.LayoutChanged += BottomSheetOnLayoutChanged;
    }

    /// <summary>
    /// Gets a value indicating whether the bottom sheet is currently open.
    /// </summary>
    public bool IsOpen => _bottomSheet.IsShowing;

    /// <summary>
    /// Sets the allowed bottom sheet states. This method is intentionally empty on Android.
    /// </summary>
    public static void SetStates()
    {
        // Method intentionally left empty.
        // On iOS and mac states must be set.
    }

    /// <summary>
    /// Sets the virtual view associated with this bottom sheet.
    /// </summary>
    /// <param name="virtualView">The virtual view to associate.</param>
    public void SetView(IBottomSheet virtualView)
    {
        _virtualView = virtualView;
    }

    /// <summary>
    /// Cleans up resources and event handlers.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task CleanupAsync()
    {
        await _bottomSheet.DisposeAsync().ConfigureAwait(true);
    }

    /// <summary>
    /// Sets whether the bottom sheet is cancelable.
    /// </summary>
    public void SetIsCancelable()
    {
        _bottomSheet.SetIsCancelable(_virtualView?.IsCancelable ?? false);
    }

    /// <summary>
    /// Sets whether the bottom sheet should show a drag handle.
    /// </summary>
    public void SetHasHandle()
    {
        if (_virtualView?.HasHandle == false)
        {
            _bottomSheet.HideHandle();
        }
        else
        {
            _bottomSheet.AddHandle();
        }
    }

    /// <summary>
    /// Sets the header for the bottom sheet.
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
    /// Sets whether the header should be shown.
    /// </summary>
    public void SetShowHeader()
    {
        if (_virtualView?.ShowHeader == false)
        {
            _bottomSheet.HideHeader();
        }
        else
        {
            _bottomSheet.AddHeader();
        }
    }

    /// <summary>
    /// Opens the bottom sheet asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task OpenAsync()
    {
        if (_virtualView is null)
        {
            return Task.CompletedTask;
        }

        _virtualView.OnOpeningBottomSheet();
        _bottomSheet.Open(_virtualView);
        _virtualView.OnOpenedBottomSheet();

        return Task.CompletedTask;
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
            await _bottomSheet.CloseAsync(false).ConfigureAwait(true);
            SetFrame(true);
            _virtualView.OnClosedBottomSheet();
        }
    }

    /// <summary>
    /// Sets the open state of the bottom sheet.
    /// </summary>
    public void SetIsOpen()
    {
        if (_virtualView?.IsOpen == true)
        {
            if (_bottomSheet.IsShowing == false)
            {
                _virtualView.OnOpeningBottomSheet();
                _bottomSheet.Open(_virtualView);
                _virtualView.OnOpenedBottomSheet();
            }
        }
        else
        {
            _virtualView?.OnClosingBottomSheet();

            if (_bottomSheet.IsShowing)
            {
                _bottomSheet.CloseAsync().SafeFireAndForget();
            }

            SetFrame(true);

            _virtualView?.OnClosedBottomSheet();
        }
    }

    /// <summary>
    /// Sets whether the bottom sheet is draggable.
    /// </summary>
    public void SetIsDraggable()
    {
        _bottomSheet.SetIsDraggable(_virtualView?.IsDraggable ?? false);
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
    /// Sets the peek height of the bottom sheet.
    /// </summary>
    public void SetPeekHeight()
    {
        if (_virtualView is null)
        {
            return;
        }

        _bottomSheet.SetPeekHeight(Context.ToPixels(_virtualView.PeekHeight));
    }

    /// <summary>
    /// Sets the content of the bottom sheet.
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
    /// Sets whether the bottom sheet is modal.
    /// </summary>
    public void SetIsModal()
    {
        _bottomSheet.SetIsModal(_virtualView?.IsModal ?? true);
    }

    /// <summary>
    /// Sets the padding of the bottom sheet.
    /// </summary>
    public void SetPadding()
    {
        if (_virtualView is not null)
        {
            _bottomSheet.Padding = _virtualView.Padding;
        }
    }

    /// <summary>
    /// Sets the background color of the bottom sheet.
    /// </summary>
    public void SetBottomSheetBackgroundColor()
    {
        if (_virtualView?.BackgroundColor is not null)
        {
            _bottomSheet.BackgroundColor = _virtualView.BackgroundColor;
        }
    }

    /// <summary>
    /// Sets the corner radius of the bottom sheet.
    /// </summary>
    public void SetCornerRadius()
    {
        _bottomSheet.SetCornerRadius(_virtualView?.CornerRadius ?? 0);
    }

    /// <summary>
    /// Sets the window background color of the bottom sheet.
    /// </summary>
    public void SetWindowBackgroundColor()
    {
        _bottomSheet.SetWindowBackgroundColor(_virtualView?.WindowBackgroundColor ?? Colors.Transparent, true);
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
    /// Releases managed and unmanaged resources.
    /// </summary>
    /// <param name="disposing">True if disposing managed resources.</param>
    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (!disposing)
        {
            return;
        }

        _bottomSheet.Dispose();
    }

    /// <summary>
    /// Handles the bottom sheet opened event and updates the frame.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    private void BottomSheetOnOpened(object? sender, EventArgs e)
    {
        SetFrame();
    }

    /// <summary>
    /// Handles the bottom sheet closed event with navigation support.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    [SuppressMessage("Usage", "VSTHRD100: Avoid async void methods", Justification = "Is okay here.")]
    [SuppressMessage("Design", "CA1031: Do not catch general exception types", Justification = "Catch all exceptions to prevent crash.")]
    private async void BottomSheetOnClosed(object? sender, EventArgs e)
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
                var result = await _mauiContext.Services.GetRequiredService<IBottomSheetNavigationService>().GoBackAsync(parameters).ConfigureAwait(true);

                if (result.Success == false
                    || result.Cancelled)
                {
                    _bottomSheet.SetState(_virtualView.CurrentState);
                }
                else
                {
                    SetFrame(true);
                }
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

                if (_virtualView.IsOpen)
                {
                    _bottomSheet.SetState(_virtualView.CurrentState);
                }
                else
                {
                    SetFrame(true);
                }
            }
        }
        catch
        {
            Trace.TraceError("Invoking IConfirmNavigation or IConfirmNavigationAsync failed.");
        }
    }

    /// <summary>
    /// Handles bottom sheet state changes and validates state transitions.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The state change event arguments.</param>
    private void BottomSheetOnStateChanged(object? sender, BottomSheetStateChangedEventArgs e)
    {
        var state = e.State;

        if (_virtualView is null
            || state == _virtualView.CurrentState)
        {
            return;
        }

        if (!_virtualView.States.IsStateAllowed(state))
        {
            state = _virtualView.CurrentState;
            _bottomSheet.SetState(state);
        }

        SetFrame();
        _virtualView.CurrentState = state;
    }

    /// <summary>
    /// Handles back button press events by invoking lifecycle event delegates.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    private void BottomSheetOnBackPressed(object? sender, EventArgs e)
    {
        var actions = _mauiContext.Services.GetRequiredService<ILifecycleEventService>().GetEventDelegates<Action<AActivity?>>(AndroidLifecycle.BottomSheetBackPressedEventName);

        foreach (Action<AActivity?> action in actions)
        {
            action(Microsoft.Maui.ApplicationModel.Platform.CurrentActivity);
        }
    }

    /// <summary>
    /// Handles layout change events and updates the frame.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    private void BottomSheetOnLayoutChanged(object? sender, EventArgs e)
    {
        SetFrame();
    }

    /// <summary>
    /// Updates the virtual view's frame based on the bottom sheet's current state.
    /// </summary>
    /// <param name="isClosed">Whether the bottom sheet is closed.</param>
    private void SetFrame(bool isClosed = false)
    {
        if (_virtualView is null)
        {
            return;
        }

        _virtualView.Frame = isClosed ? Rect.Zero : _context.FromPixels(_bottomSheet.Frame);
    }
}