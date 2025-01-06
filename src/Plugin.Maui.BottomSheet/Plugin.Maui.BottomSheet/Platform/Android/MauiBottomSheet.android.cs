#pragma warning disable SA1200
using Android.Content;
using AndroidView = Android.Views.View;
#pragma warning restore SA1200

namespace Plugin.Maui.BottomSheet.Platform.Android;

/// <inheritdoc />
internal sealed class MauiBottomSheet : AndroidView
{
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
        _bottomSheet = new BottomSheet(context, mauiContext);
        _bottomSheet.Closed += BottomSheetOnClosed;
        _bottomSheet.StateChanged += BottomSheetOnStateChanged;
    }

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
        if (_virtualView?.IsOpen == false
            || !_bottomSheet.IsShowing)
        {
            return;
        }

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
        if (_virtualView?.Header is not null)
        {
            _bottomSheet.SetHeader(_virtualView.Header);
        }

        SetShowHeader();
    }

    /// <summary>
    /// Show header.
    /// </summary>
    public void SetShowHeader()
    {
        if (_virtualView?.IsOpen == false
            || !_bottomSheet.IsShowing)
        {
            return;
        }

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
    public void SetIsOpen()
    {
        if (_virtualView?.IsOpen == true)
        {
            _virtualView.OnOpeningBottomSheet();
            _bottomSheet.Open(_virtualView);
            _virtualView.OnOpenedBottomSheet();
        }
        else
        {
            _virtualView?.OnClosingBottomSheet();
            _bottomSheet.Close();
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
    public void SetPeek()
    {
        if (_virtualView?.Peek is not null)
        {
            _bottomSheet.SetPeek(_virtualView.Peek);
        }

        if (_virtualView?.IsOpen == true
            && _bottomSheet.IsShowing)
        {
            if (_virtualView.Peek is not null)
            {
                _bottomSheet.AddPeek();
            }
            else
            {
                _bottomSheet.HidePeek();
            }
        }
    }

    /// <summary>
    /// Set content.
    /// </summary>
    public void SetContent()
    {
        if (_virtualView?.Content is not null)
        {
            _bottomSheet.SetContent(_virtualView.Content);
        }

        if (_virtualView?.IsOpen == true
            && _bottomSheet.IsShowing)
        {
            if (_virtualView.Content is not null)
            {
                _bottomSheet.AddContent();
            }
            else
            {
                _bottomSheet.HideContent();
            }
        }
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

    private void BottomSheetOnClosed(object? sender, EventArgs e)
    {
        if (_virtualView is not null)
        {
            _virtualView.IsOpen = false;
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