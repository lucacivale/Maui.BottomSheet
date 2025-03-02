namespace Plugin.Maui.BottomSheet.Platform.MaciOS;

using AsyncAwaitBestPractices;
using UIKit;

/// <inheritdoc />
internal sealed class MauiBottomSheet : UIView
{
    private readonly BottomSheet _bottomSheet;

    private IBottomSheet? _virtualView;

    /// <summary>
    /// Initializes a new instance of the <see cref="MauiBottomSheet"/> class.
    /// </summary>
    /// <param name="mauiContext">Maui context.</param>
    public MauiBottomSheet(IMauiContext mauiContext)
    {
        _bottomSheet = new BottomSheet(mauiContext);
        _bottomSheet.Dismissed += BottomSheetOnDismissed;
        _bottomSheet.StateChanged += BottomSheetOnStateChanged;
    }

    /// <inheritdoc/>
    public override void MovedToWindow()
    {
        base.MovedToWindow();

        if (!_bottomSheet.IsBeingPresented)
        {
            SetIsOpenAsync().SafeFireAndForget(continueOnCapturedContext: false);
        }
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
        _bottomSheet.Dispose();
    }

    /// <summary>
    /// Set whether sheet is cancelable.
    /// </summary>
    public void SetIsCancelable()
    {
        _bottomSheet.SetIsCancelable(_virtualView?.IsCancelable == true);
    }

    /// <summary>
    /// Set whether show handle.
    /// </summary>
    public void SetHasHandle()
    {
        _bottomSheet.SetHasHandle(_virtualView?.HasHandle == true);
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
        if (_virtualView?.IsOpen == false
            || _bottomSheet.IsBeingPresented == false)
        {
            return;
        }

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
    /// Open bottom sheet.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task SetIsOpenAsync()
    {
        if (_virtualView?.IsOpen == true)
        {
            _virtualView.OnOpeningBottomSheet();
            await _bottomSheet.OpenAsync(_virtualView).ConfigureAwait(true);
            _virtualView.OnOpenedBottomSheet();
        }
        else
        {
            _virtualView?.OnClosingBottomSheet();
            await _bottomSheet.CloseAsync().ConfigureAwait(true);
            _virtualView?.OnClosedBottomSheet();
        }
    }

    /// <summary>
    /// Set whether bottom sheet is draggable.
    /// </summary>
    public void SetIsDraggable()
    {
        _bottomSheet.SetIsDraggable(_virtualView?.IsDraggable == true);
    }

    /// <summary>
    /// Set allowed bottom sheet states.
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
    /// Set peek height.
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
        _bottomSheet.SetIsModal(_virtualView?.IsModal == true);
    }

    /// <summary>
    /// Set padding.
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
    /// Set background color.
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
    /// Set ignore safe area.
    /// </summary>
    public void SetIgnoreSafeArea()
    {
        _bottomSheet.SetIgnoreSafeArea(_virtualView?.IgnoreSafeArea == true);
    }

    /// <summary>
    /// Sets corner radius.
    /// </summary>
    public void SetCornerRadius()
    {
        _bottomSheet.SetCornerRadius(_virtualView?.CornerRadius ?? 0);
    }

    /// <summary>
    /// Sets window background color.
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
        if (disposing)
        {
            _bottomSheet.Dispose();
        }

        base.Dispose(disposing);
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
        }

        _virtualView.CurrentState = state;
        _bottomSheet.SetState(_virtualView.CurrentState);
    }

    private void BottomSheetOnDismissed(object? sender, EventArgs e)
    {
        if (_virtualView is not null)
        {
            _virtualView.IsOpen = false;
        }
    }
}