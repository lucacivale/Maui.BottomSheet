#pragma warning disable SA1200
using Android.Content;
using AndroidView = Android.Views.View;
#pragma warning restore SA1200

// ReSharper disable once CheckNamespace
namespace Plugin.Maui.BottomSheet.Platforms.Android;

/// <inheritdoc />
public sealed class MauiBottomSheet : AndroidView
{
    private readonly BottomSheetContainer _bottomSheetContainer;

    private IBottomSheet? _virtualView;

    /// <summary>
    /// Initializes a new instance of the <see cref="MauiBottomSheet"/> class.
    /// </summary>
    /// <param name="mauiContext">Maui context.</param>
    /// <param name="context">Android context.</param>
    public MauiBottomSheet(IMauiContext mauiContext, Context context)
        : base(context)
    {
        _bottomSheetContainer = new BottomSheetContainer(context, mauiContext);
        _bottomSheetContainer.Closed += BottomSheetContainerOnClosed;
        _bottomSheetContainer.StateChanged += BottomSheetContainerOnStateChanged;
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
        _bottomSheetContainer.Closed -= BottomSheetContainerOnClosed;
        _bottomSheetContainer.StateChanged -= BottomSheetContainerOnStateChanged;
        _bottomSheetContainer.Dispose();
    }

    /// <summary>
    /// Set whether sheet is cancelable.
    /// </summary>
    public void SetIsCancelable()
    {
        _bottomSheetContainer.SetIsCancelable(_virtualView?.IsCancelable ?? false);
    }

    /// <summary>
    /// Set whether show handle.
    /// </summary>
    public void SetHasHandle()
    {
        if (_virtualView?.IsOpen == false
            || _bottomSheetContainer.IsShowing == false)
        {
            return;
        }

        if (_virtualView?.HasHandle == false)
        {
            _bottomSheetContainer.HideHandle();
        }
        else
        {
            _bottomSheetContainer.AddHandle();
        }
    }

    /// <summary>
    /// Set bottom sheet header.
    /// </summary>
    public void SetHeader()
    {
        if (_virtualView?.Header is not null)
        {
            _bottomSheetContainer.SetHeader(_virtualView.Header);
        }

        SetShowHeader();
    }

    /// <summary>
    /// Show header.
    /// </summary>
    public void SetShowHeader()
    {
        if (_virtualView?.IsOpen == false
            || _bottomSheetContainer.IsShowing == false)
        {
            return;
        }

        if (_virtualView?.ShowHeader == false)
        {
            _bottomSheetContainer.HideHeader();
        }
        else
        {
            _bottomSheetContainer.AddHeader();
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
            _bottomSheetContainer.Open(_virtualView);
            _virtualView.OnOpenedBottomSheet();
        }
        else
        {
            _virtualView?.OnClosingBottomSheet();
            _bottomSheetContainer.Close();
            _virtualView?.OnClosedBottomSheet();
        }
    }

    /// <summary>
    /// Set whether bottom sheet is draggable.
    /// </summary>
    public void SetIsDraggable()
    {
        _bottomSheetContainer.SetIsDraggable(_virtualView?.IsDraggable ?? false);
    }

    /// <summary>
    /// Set allowed bottom sheet states.
    /// </summary>
    public void SetStates()
    {
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

        _bottomSheetContainer.SetState(_virtualView.CurrentState);
    }

    /// <summary>
    /// Set peek.
    /// </summary>
    public void SetPeek()
    {
        if (_virtualView?.Peek is not null)
        {
            _bottomSheetContainer.SetPeek(_virtualView.Peek);
        }
        
        if (_virtualView?.IsOpen == true
            && _bottomSheetContainer.IsShowing == true)
        {
            if (_virtualView?.Peek is not null)
            {
                _bottomSheetContainer.AddPeek();
            }
            else
            {
                _bottomSheetContainer.HidePeek();                
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
            _bottomSheetContainer.SetContent(_virtualView.Content);
        }
        
        if (_virtualView?.IsOpen == true
            && _bottomSheetContainer.IsShowing == true)
        {
            if (_virtualView?.Content is not null)
            {
                _bottomSheetContainer.AddContent();
            }
            else
            {
                _bottomSheetContainer.HideContent();                
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
            _bottomSheetContainer.Padding = _virtualView.Padding;
        }
    }

    /// <summary>
    /// set background color.
    /// </summary>
    public void SetBottomSheetBackgroundColor()
    {
        if (_virtualView?.BackgroundColor is not null)
        {
            _bottomSheetContainer.BackgroundColor = _virtualView.BackgroundColor;
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

    private void BottomSheetContainerOnClosed(object? sender, EventArgs e)
    {
        if (_virtualView is not null)
        {
            _virtualView.IsOpen = false;
        }
    }

    private void BottomSheetContainerOnStateChanged(object? sender, BottomSheetStateChangedEventArgs e)
    {
        if (_virtualView is null)
        {
            return;
        }

        var state = e.State;

        if (!_virtualView.States.IsStateAllowed(state))
        {
            state = _virtualView.CurrentState;
            _bottomSheetContainer.SetState(state);
        }

        _virtualView.CurrentState = state;
    }
}