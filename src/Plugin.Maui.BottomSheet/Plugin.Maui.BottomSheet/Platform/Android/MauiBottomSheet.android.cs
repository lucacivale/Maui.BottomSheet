using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Plugin.Maui.BottomSheet.Navigation;
#pragma warning disable SA1200
using Android.Content;
using AndroidView = Android.Views.View;
#pragma warning restore SA1200

namespace Plugin.Maui.BottomSheet.Platform.Android;

using Microsoft.Maui.Platform;

/// <inheritdoc />
public sealed class MauiBottomSheet : AndroidView
{
    private readonly IMauiContext _mauiContext;
    private readonly BottomSheet _bottomSheet;

    private IBottomSheet? _virtualView;

    /// <summary>
    /// Initializes a new instance of the <see cref="MauiBottomSheet"/> class.
    /// </summary>
    /// <param name="mauiContext">Maui context.</param>
    /// <param name="context">Android context.</param>
    public MauiBottomSheet(IMauiContext mauiContext, Context context)
        : base(context)
    {
        _mauiContext = mauiContext;
        _bottomSheet = new BottomSheet(context, _mauiContext);
        _bottomSheet.Closed += BottomSheetOnClosed;
        _bottomSheet.StateChanged += BottomSheetOnStateChanged;
    }

    /// <summary>
    /// Gets a value indicating whether the bottom sheet is open.
    /// </summary>
    public bool IsOpen => _bottomSheet.IsShowing;

    /// <summary>
    /// Gets a reference to the Android BottomSheetDialog if the bottom sheet is open.
    /// </summary>
    public AndroidX.AppCompat.App.AppCompatDialog? Dialog => _bottomSheet.Dialog;

    /// <summary>
    /// Set allowed bottom sheet states.
    /// </summary>
    public static void SetStates()
    {
        // Method intentionally left empty.
        // On iOS and mac states must be set.
    }

    /// <summary>
    /// Set virtual view.
    /// </summary>
    /// <param name="virtualView">View.</param>
    public void SetView(IBottomSheet virtualView)
    {
        _virtualView = virtualView;
    }

    /// <summary>
    /// Cleanup resources.
    /// </summary>
    public void Cleanup()
    {
        _bottomSheet.Closed -= BottomSheetOnClosed;
        _bottomSheet.StateChanged -= BottomSheetOnStateChanged;
        _bottomSheet.Dispose();
    }

    /// <summary>
    /// Set whether sheet is cancelable.
    /// </summary>
    public void SetIsCancelable()
    {
        _bottomSheet.SetIsCancelable(_virtualView?.IsCancelable ?? false);
    }

    /// <summary>
    /// Set whether show handle.
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
    /// Set bottom sheet header.
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
    /// Show header.
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
    /// Open bottom sheet.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
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
    /// Close bottom sheet.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task CloseAsync()
    {
        if (_virtualView?.IsOpen == true)
        {
            _virtualView.OnClosingBottomSheet();
            _bottomSheet.Close(false);
            _virtualView.OnClosedBottomSheet();
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Open bottom sheet.
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
                _bottomSheet.Close();
            }

            _virtualView?.OnClosedBottomSheet();
        }
    }

    /// <summary>
    /// Set whether bottom sheet is draggable.
    /// </summary>
    public void SetIsDraggable()
    {
        _bottomSheet.SetIsDraggable(_virtualView?.IsDraggable ?? false);
    }

    /// <summary>
    /// Set current bottom sheet state.
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
    /// Set peek.
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
    /// Set content.
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
    /// Set modal.
    /// </summary>
    public void SetIsModal()
    {
        _bottomSheet.SetIsModal(_virtualView?.IsModal ?? true);
    }

    /// <summary>
    /// Set padding.
    /// </summary>
    public void SetPadding()
    {
        if (_virtualView is not null)
        {
            _bottomSheet.Padding = _virtualView.Padding;
        }
    }

    /// <summary>
    /// set background color.
    /// </summary>
    public void SetBottomSheetBackgroundColor()
    {
        if (_virtualView?.BackgroundColor is not null)
        {
            _bottomSheet.BackgroundColor = _virtualView.BackgroundColor;
        }
    }

    /// <summary>
    /// Set corner radius.
    /// </summary>
    public void SetCornerRadius()
    {
        _bottomSheet.SetCornerRadius(_virtualView?.CornerRadius ?? 0);
    }

    /// <summary>
    /// Set window background color.
    /// </summary>
    public void SetWindowBackgroundColor()
    {
        _bottomSheet.SetWindowBackgroundColor(_virtualView?.WindowBackgroundColor ?? Colors.Transparent);
    }

    /// <summary>
    /// Sets bottom sheet style.
    /// </summary>
    public void SetBottomSheetStyle()
    {
        if (_virtualView?.BottomSheetStyle is not null)
        {
            _bottomSheet.SetHeaderStyle(_virtualView.BottomSheetStyle.HeaderStyle);
        }
    }

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (!disposing)
        {
            return;
        }

        Cleanup();
    }

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
                await _mauiContext.Services.GetRequiredService<IBottomSheetNavigationService>().GoBackAsync(parameters).ConfigureAwait(true);

                if (_mauiContext.Services.GetRequiredService<IBottomSheetNavigationService>().NavigationStack().Contains(_virtualView))
                {
                    _bottomSheet.SetState(_virtualView.CurrentState);
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
            }
        }
        catch
        {
            Trace.TraceError("Invoking IConfirmNavigation or IConfirmNavigationAsync failed.");
        }
    }

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
            _bottomSheet.SetState(state);
        }

        _virtualView.CurrentState = state;
    }
}