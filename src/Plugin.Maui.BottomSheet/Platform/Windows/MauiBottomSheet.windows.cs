using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml;
using Plugin.Maui.BottomSheet.Navigation;
using Plugin.Maui.BottomSheet.PlatformConfiguration.WindowsSpecific;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using WWindow = Microsoft.UI.Xaml.Window;

namespace Plugin.Maui.BottomSheet.Platform.Windows;

/// <summary>
/// Represents a platform-specific implementation of a bottom sheet for Windows, integrated into the .NET MAUI framework.
/// </summary>
public sealed partial class MauiBottomSheet : FrameworkElement
{
    private readonly IMauiContext _mauiContext;
    private readonly TaskCompletionSource _isAttachedToWindowTcs;

    private Plugin.BottomSheet.Windows.BottomSheet? _bottomSheet;

    private IBottomSheet? _virtualView;

    private bool _isAttachedToWindow;

    /// <summary>
    /// Initializes a new instance of the <see cref="MauiBottomSheet"/> class.
    /// MAUI implementation of a bottom sheet for windows.
    /// </summary>
    /// <param name="mauiContext">The MAUI context associated with the bottom sheet.</param>
    public MauiBottomSheet(IMauiContext mauiContext)
    {
        _mauiContext = mauiContext;
        _isAttachedToWindowTcs = new TaskCompletionSource();

        Loaded += MauiBottomSheet_Loaded;
    }

    /// <summary>
    /// Gets a value indicating whether the bottom sheet is currently open.
    /// </summary>
    public bool IsOpen => _bottomSheet?.IsOpen == true;

    /// <summary>
    /// Gets the underlying ContentDialog instance associated with the MAUI bottom sheet.
    /// </summary>
    public Plugin.BottomSheet.Windows.BottomSheet? BottomSheet => _bottomSheet;

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
        Loaded -= MauiBottomSheet_Loaded;
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

        WWindow window = (((View)_virtualView).Window.Handler.PlatformView as WWindow) ?? throw new NotSupportedException("Window can not be null.");

        _bottomSheet = new Plugin.BottomSheet.Windows.BottomSheet(window);
        _bottomSheet.Canceled += BottomSheetOnCanceled;
        _bottomSheet.SizeChanged += BottomSheetOnFrameChanged;

        SetWindowBackgroundColor();
        SetBottomSheetBackgroundColor();
        SetCornerRadius();
        SetMinWidth();
        SetMinHeight();
        SetMaxWidth();
        SetMaxHeight();
        SetSizeMode();

        FrameworkElement view = _virtualView.ContainerView.ToPlatform(_mauiContext);
        view.UpdateAutomationId(_virtualView);

        _bottomSheet.SetContentView(view);

        _virtualView.OnOpeningBottomSheet();

        if (XamlRoot is null)
        {
            XamlRoot = window.Content.XamlRoot;
        }

        await _bottomSheet.OpenAsync(XamlRoot).ConfigureAwait(true);

        SetFrame();

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

        _bottomSheet.Canceled -= BottomSheetOnCanceled;
        _bottomSheet.SizeChanged -= BottomSheetOnFrameChanged;

        await _bottomSheet.CloseAsync().ConfigureAwait(true);

        SetFrame(true);
        _virtualView.OnClosedBottomSheet();

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
    /// Sets the background color of the bottom sheet based on the current virtual view's background color.
    /// </summary>
    public void SetBottomSheetBackgroundColor()
    {
        if (_bottomSheet is null
            || _virtualView?.BackgroundColor is null)
        {
            return;
        }

        _bottomSheet.Background = _virtualView.BackgroundColor.ToPlatform();
    }

    /// <summary>
    /// Sets the corner radius for the bottom sheet to align with the desired appearance.
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
    /// Sets the background color of the window associated with the bottom sheet.
    /// </summary>
    public void SetWindowBackgroundColor()
    {
        if (_bottomSheet is null
            || _virtualView?.WindowBackgroundColor is null)
        {
            return;
        }

        // ReSharper disable once ConditionalAccessQualifierIsNonNullableAccordingToAPIContract
        _bottomSheet.WindowBackground = _virtualView.WindowBackgroundColor.ToPlatform();
    }

    /// <summary>
    /// Sets the minimal width.
    /// </summary>
    public void SetMinWidth()
    {
        if (_bottomSheet is null
            || _virtualView is null)
        {
            return;
        }

        _bottomSheet.MinWidth = _virtualView.GetMinWidth();
    }

    /// <summary>
    /// Sets the minimal height.
    /// </summary>
    public void SetMinHeight()
    {
        if (_bottomSheet is null
            || _virtualView is null)
        {
            return;
        }

        _bottomSheet.MinHeight = _virtualView.GetMinHeight();
    }

    /// <summary>
    /// Sets the maximal width.
    /// </summary>
    public void SetMaxWidth()
    {
        if (_bottomSheet is null
            || _virtualView is null)
        {
            return;
        }

        _bottomSheet.MaxWidth = _virtualView.GetMaxWidth();
    }

    /// <summary>
    /// Sets the maximal height.
    /// </summary>
    public void SetMaxHeight()
    {
        if (_bottomSheet is null
            || _virtualView is null)
        {
            return;
        }

        _bottomSheet.MaxHeight = _virtualView.GetMaxHeight();
    }

    /// <summary>
    /// Configures the size mode of the bottom sheet based on the associated virtual view's size mode.
    /// </summary>
    public void SetSizeMode()
    {
        if (_virtualView is null
            || _bottomSheet is null)
        {
            return;
        }

        _bottomSheet.SizeMode = _virtualView.SizeMode;
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
    private void BottomSheetOnFrameChanged(object? sender, SizeChangedEventArgs e)
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

        _virtualView.Frame = isClosed ? Rect.Zero : new Rect(
            _bottomSheet.Frame.X,
            _bottomSheet.Frame.Y,
            _bottomSheet.Frame.Width,
            _bottomSheet.Frame.Height);
    }

    private void MauiBottomSheet_Loaded(object sender, RoutedEventArgs e)
    {
        _isAttachedToWindow = true;
        _isAttachedToWindowTcs.TrySetResult();
    }
}
