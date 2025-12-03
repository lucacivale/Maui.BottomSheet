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
/// Represents the Android-specific implementation for a MAUI bottom sheet component.
/// </summary>
public sealed class MauiBottomSheet : AndroidView
{
    private readonly IMauiContext _mauiContext;
    private readonly Context _context;
    private readonly TaskCompletionSource _isAttachedToWindowTcs;

    private IBottomSheet? _virtualView;
    private BottomSheetDialog? _bottomSheet;

    private bool _isAttachedToWindow;

    /// <summary>
    /// Initializes a new instance of the <see cref="MauiBottomSheet"/> class.
    /// Android platform implementation of the MAUI bottom sheet view.
    /// </summary>
    /// <param name="mauiContext">The MAUI context associated with the bottom sheet.</param>
    /// <param name="context">The Android context associated with the bottom sheet.</param>
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
    /// Gets the underlying Android BottomSheetDialog instance associated with the MAUI bottom sheet.
    /// </summary>
    public BottomSheetDialog? BottomSheet => _bottomSheet;

    /// <summary>
    /// Configures the allowed states for the bottom sheet.
    /// Updates the bottom sheet's states based on the associated virtual view.
    /// </summary>
    public void SetStates()
    {
        if (_virtualView is null
            || _bottomSheet is null)
        {
            return;
        }

        _bottomSheet.States = new List<BottomSheetState>(_virtualView.States);
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
    /// Releases resources and detaches event handlers associated with the bottom sheet.
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
    /// Sets whether the user can cancel the bottom sheet.
    /// This method configures the cancelable behavior of the underlying platform-specific bottom sheet.
    /// </summary>
    public void SetIsCancelable()
    {
        _bottomSheet?.SetCancelable(_virtualView?.IsCancelable ?? false);
    }

    /// <summary>
    /// Asynchronously opens the bottom sheet, initializing its properties and event handlers.
    /// Optionally forces the bottom sheet to open immediately, bypassing attachment checks.
    /// </summary>
    /// <param name="force">
    /// A boolean value indicating whether to bypass the attachment check and force the bottom sheet to open.
    /// Setting this parameter to <c>false</c> will cause the method to wait for the bottom sheet to be attached
    /// to the window for up to 20 seconds. The default value is <c>false</c>.
    /// </param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation of opening the bottom sheet.</returns>
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

        _bottomSheet.SetContentView(_virtualView.ContainerView.ToPlatform(_mauiContext));

        SetHalfExpandedRatio();
        SetMargin();
        _bottomSheet.MaxHeight = _virtualView.GetMaxHeight();
        _bottomSheet.MaxWidth = _virtualView.GetMaxWidth();

        _virtualView.OnOpeningBottomSheet();

        await _bottomSheet.ShowAsync().ConfigureAwait(true);

        _bottomSheet.Window?.DecorView.UpdateAutomationId(_virtualView);

        SetWindowBackgroundColor();
        SetBottomSheetBackgroundColor();
        SetPeekHeight();
        SetIsModal();
        SetCornerRadius();
        SetFrame();

        _virtualView.OnOpenedBottomSheet();
    }

    /// <summary>
    /// Closes the bottom sheet asynchronously, ensuring proper cleanup and notifying the virtual view of closure events.
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

    /// <summary>
    /// Cancels the currently displayed bottom sheet, if any, by invoking the cancel method on the underlying <see cref="BottomSheetDialog"/> instance.
    /// </summary>
    public void Cancel()
    {
        _bottomSheet?.Cancel();
    }

    /// <summary>
    /// Asynchronously sets the open state of the bottom sheet based on the current view state.
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

    /// <summary>
    /// Applies the margin settings from the virtual view to the bottom sheet dialog.
    /// </summary>
    public void SetMargin()
    {
        if (_bottomSheet is null
            || _virtualView is null)
        {
            return;
        }

        _bottomSheet.Margin = _virtualView.GetMargin().ToThickness();
    }

    /// <summary>
    /// Sets the ratio at which the bottom sheet is considered half-expanded.
    /// </summary>
    /// <remarks>
    /// This method updates the half-expanded ratio of the underlying bottom sheet dialog based on the value
    /// provided by the virtual view. The half-expanded ratio determines the intermediate state between collapsed
    /// and fully expanded, improving user interactivity.
    /// </remarks>
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
    /// Configures the draggable behavior of the bottom sheet based on its current state or settings.
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
    /// Updates the state of the bottom sheet to reflect the current state of the virtual view.
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
    /// Sets the peek height of the bottom sheet based on the virtual view's configuration.
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
    /// Sets the modal state of the bottom sheet based on the associated virtual view or a default value.
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
    /// Sets the background color of the bottom sheet based on the virtual view's background color.
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
    /// Sets the corner radius of the bottom sheet to the specified value from the virtual view or defaults to zero if not set.
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
    /// Sets the background color of the bottom sheet's window.
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

    /// <summary>
    /// Called when the view is attached to a window. This method overrides the base implementation
    /// to initialize platform-specific logic for the bottom sheet component and sets the internal
    /// state to indicate that the view is attached to the window.
    /// </summary>
    protected override void OnAttachedToWindow()
    {
        base.OnAttachedToWindow();

        _isAttachedToWindow = true;
        _isAttachedToWindowTcs.TrySetResult();
    }

    /// <summary>
    /// Handles the event when the bottom sheet is canceled, including navigation support.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
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
    /// Handles bottom sheet state changes and validates state transitions.
    /// </summary>
    /// <param name="sender">The source of the event, typically the bottom sheet.</param>
    /// <param name="e">The event arguments containing details of the state change.</param>
    private void BottomSheetOnStateChanged(object? sender, BottomSheetStateChangedEventArgs e)
    {
        BottomSheetState state = e.NewState;

        if (_virtualView is null
            || state == _virtualView.CurrentState)
        {
            return;
        }

        SetFrame();
        _virtualView.CurrentState = state;
    }

    /// <summary>
    /// Handles the back button press event for the bottom sheet by invoking registered lifecycle event delegates.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void BottomSheetOnBackPressed(object? sender, EventArgs e)
    {
        IEnumerable<Action<AActivity?>> actions = _mauiContext.Services.GetRequiredService<ILifecycleEventService>()
            .GetEventDelegates<Action<AActivity?>>(AndroidLifecycle.BottomSheetBackPressedEventName);

        foreach (Action<AActivity?> action in actions)
        {
            action(Microsoft.Maui.ApplicationModel.Platform.CurrentActivity);
        }
    }

    /// <summary>
    /// Handles layout change events for the bottom sheet and updates its frame.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data associated with the layout change.</param>
    private void BottomSheetOnLayoutChanged(object? sender, EventArgs e)
    {
        _virtualView?.OnLayoutChanged();
        SetFrame();
    }

    /// <summary>
    /// Updates the frame of the virtual view based on the current frame of the bottom sheet dialog or resets it if the bottom sheet is closed.
    /// </summary>
    /// <param name="isClosed">Indicates whether the bottom sheet is currently closed.</param>
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