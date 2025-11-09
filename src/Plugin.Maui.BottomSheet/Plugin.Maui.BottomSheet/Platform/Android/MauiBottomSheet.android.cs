using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Maui.LifecycleEvents;
using Plugin.Maui.BottomSheet.Navigation;
using Plugin.BottomSheet;
using Plugin.BottomSheet.Android;
using Microsoft.Maui.Platform;
using Android.Content;
using Plugin.Maui.BottomSheet.PlatformConfiguration.AndroidSpecific;
using AActivity = Android.App.Activity;
using AndroidLifecycle = Plugin.Maui.BottomSheet.LifecycleEvents.AndroidLifecycle;
using AndroidView = Android.Views.View;

namespace Plugin.Maui.BottomSheet.Platform.Android;

/// <summary>
/// Android platform implementation of the MAUI bottom sheet view handler.
/// </summary>
internal sealed class MauiBottomSheet : AndroidView
{
    private readonly IMauiContext _mauiContext;
    private readonly Context _context;
    private readonly TaskCompletionSource _isAttachedToWindowTcs;

    private IBottomSheet? _virtualView;
    private BottomSheetDialog? _bottomSheet;

    private bool _isAttachedToWindow;

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
        _isAttachedToWindowTcs = new TaskCompletionSource();
    }

    /// <summary>
    /// Gets a value indicating whether the bottom sheet is currently open.
    /// </summary>
    public bool IsOpen => _bottomSheet?.IsShowing == true;

    /// <summary>
    /// Sets the allowed bottom sheet states. This method is intentionally empty on Android.
    /// </summary>
    public void SetStates()
    {
        if (_virtualView is null
            || _bottomSheet is null)
        {
            return;
        }

        _bottomSheet.States = new(_virtualView.States);
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
    public void Cleanup()
    {
        if (_bottomSheet is null)
        {
            return;
        }

        _bottomSheet.Canceled -= BottomSheetOnCanceled;
        _bottomSheet.StateChanged -= BottomSheetOnStateChanged;
        _bottomSheet.BackPressed -= BottomSheetOnBackPressed;
        _bottomSheet.LayoutChanged -= BottomSheetOnLayoutChanged;

        _bottomSheet.Dispose();
        _bottomSheet = null;
    }

    /// <summary>
    /// Sets whether the bottom sheet is cancelable.
    /// </summary>
    public void SetIsCancelable()
    {
        _bottomSheet?.SetCancelable(_virtualView?.IsCancelable ?? false);
    }

    /// <summary>
    /// Sets whether the bottom sheet should show a drag handle.
    /// </summary>
    public void SetHasHandle()
    {
        if (_bottomSheet is null)
        {
            return;
        }

        _bottomSheet.HasHandle = _virtualView?.HasHandle ?? false;
    }

    /// <summary>
    /// Sets the header for the bottom sheet.
    /// </summary>
    public void SetHeader()
    {
        SetShowHeader();
    }

    /// <summary>
    /// Sets whether the header should be shown.
    /// </summary>
    public void SetShowHeader()
    {
        if (_bottomSheet is null)
        {
            return;
        }

        if (_virtualView?.ShowHeader == false)
        {
            _bottomSheet.RemoveHeader();
        }
        else
        {
            if (_virtualView?.Header is not null)
            {
                _bottomSheet.SetHeaderView(_virtualView.Header.CreateContent().ToPlatform(_mauiContext));
            }
        }
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

        _bottomSheet = new(_context, _virtualView.GetTheme());
        _bottomSheet.Canceled += BottomSheetOnCanceled;
        _bottomSheet.StateChanged += BottomSheetOnStateChanged;
        _bottomSheet.BackPressed += BottomSheetOnBackPressed;
        _bottomSheet.LayoutChanged += BottomSheetOnLayoutChanged;

        SetStates();
        SetIsCancelable();
        SetIsDraggable();
        SetCurrentState();
        SetShowHeader();
        SetContent();

        SetHalfExpandedRatio();
        SetMargin();
        _bottomSheet.MaxHeight = _virtualView.GetMaxHeight();
        _bottomSheet.MaxWidth = _virtualView.GetMaxWidth();

        _virtualView.OnOpeningBottomSheet();

        await _bottomSheet.ShowAsync().ConfigureAwait(true);

        _bottomSheet.Window?.DecorView.UpdateAutomationId(_virtualView);

        SetWindowBackgroundColor();
        SetBottomSheetBackgroundColor();
        SetHasHandle();
        SetPeekHeight();
        SetIsModal();
        SetPadding();
        SetCornerRadius();
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

        await _bottomSheet.Dismiss().ConfigureAwait(true);

        SetFrame(true);
        _virtualView.OnClosedBottomSheet();

        _bottomSheet.Dispose();
        _bottomSheet = null;
    }

    public void Cancel()
    {
        _bottomSheet?.Cancel();
    }

    /// <summary>
    /// Sets the open state of the bottom sheet.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task SetIsOpenAsync()
    {
        if (_virtualView?.IsOpen == true)
        {
            if (_bottomSheet?.IsShowing == false
                || _bottomSheet is null)
            {
                await OpenAsync().ConfigureAwait(true);
            }
        }
        else
        {
            if (_bottomSheet?.IsShowing == true)
            {
                Cancel();
            }
        }
    }

    public void SetMargin()
    {
        if (_bottomSheet is null
            || _virtualView is null)
        {
            return;
        }

        _bottomSheet.Margin = _virtualView.GetMargin().ToThickness();
    }

    public void SetHalfExpandedRatio()
    {
        if (_bottomSheet is null
            || _virtualView is null)
        {
            return;
        }

        _bottomSheet.HalfExpandedRatio = _virtualView.GetHalfExpandedRatio();
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

        _bottomSheet.Draggable = _virtualView?.IsDraggable ?? false;
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
    /// Sets the peek height of the bottom sheet.
    /// </summary>
    public void SetPeekHeight()
    {
        if (_virtualView is null
            || _bottomSheet is null)
        {
            return;
        }

        _bottomSheet.PeekHeight = Microsoft.Maui.Platform.ContextExtensions.ToPixels(Context, _virtualView.PeekHeight);
    }

    /// <summary>
    /// Sets the content of the bottom sheet.
    /// </summary>
    public void SetContent()
    {
        AndroidView view = _virtualView?.Content?.CreateContent().ToPlatform(_mauiContext) ?? new AndroidView(Context);

        _bottomSheet?.SetContentView(view);
    }

    /// <summary>
    /// Sets whether the bottom sheet is modal.
    /// </summary>
    public void SetIsModal()
    {
        if (_bottomSheet is null)
        {
            return;
        }

        _bottomSheet.IsModal = _virtualView?.IsModal ?? true;
    }

    /// <summary>
    /// Sets the padding of the bottom sheet.
    /// </summary>
    public void SetPadding()
    {
        if (_virtualView is null
            || _bottomSheet is null)
        {
            return;
        }

        _bottomSheet.Padding = new(_virtualView.Padding.Left, _virtualView.Padding.Top, _virtualView.Padding.Right, _virtualView.Padding.Bottom);
    }

    /// <summary>
    /// Sets the background color of the bottom sheet.
    /// </summary>
    public void SetBottomSheetBackgroundColor()
    {
        if (_virtualView?.BackgroundColor is null
            || _bottomSheet is null)
        {
            return;
        }

        _bottomSheet.BackgroundColor = _virtualView.BackgroundColor.ToPlatform();
    }

    /// <summary>
    /// Sets the corner radius of the bottom sheet.
    /// </summary>
    public void SetCornerRadius()
    {
        if (_bottomSheet is null)
        {
            return;
        }

        _bottomSheet.CornerRadius = _virtualView?.CornerRadius ?? 0;
    }

    /// <summary>
    /// Sets the window background color of the bottom sheet.
    /// </summary>
    public void SetWindowBackgroundColor()
    {
        if (_virtualView?.WindowBackgroundColor is null
            || _bottomSheet is null)
        {
            return;
        }

        _bottomSheet.WindowBackgroundColor = _virtualView.WindowBackgroundColor.ToPlatform();
    }

    /// <inheritdoc />
    protected override void OnAttachedToWindow()
    {
        base.OnAttachedToWindow();

        _isAttachedToWindow = true;
        _isAttachedToWindowTcs.TrySetResult();
    }

    /// <summary>
    /// Handles the bottom sheet closed event with navigation support.
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
        catch
        {
            Trace.TraceError("Invoking IConfirmNavigation or IConfirmNavigationAsync failed: {0}", e);
        }
    }

    /// <summary>
    /// Handles bottom sheet state changes and validates state transitions.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The state change event arguments.</param>
    private void BottomSheetOnStateChanged(object? sender, BottomSheetStateChangedEventArgs e)
    {
        BottomSheetState state = e.State;

        if (_virtualView is null
            || state == _virtualView.CurrentState)
        {
            return;
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
        IEnumerable<Action<AActivity?>> actions = _mauiContext.Services.GetRequiredService<ILifecycleEventService>().GetEventDelegates<Action<AActivity?>>(AndroidLifecycle.BottomSheetBackPressedEventName);

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
        SetPeekHeight();
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

        _virtualView.Frame = isClosed ? Microsoft.Maui.Graphics.Rect.Zero : _context.FromPixels(
            new Microsoft.Maui.Graphics.Rect(
                Convert.ToDouble(_bottomSheet.Frame.X),
                Convert.ToDouble(_bottomSheet.Frame.Y),
                Convert.ToDouble(_bottomSheet.Frame.Width),
                Convert.ToDouble(_bottomSheet.Frame.Height)));
    }
}