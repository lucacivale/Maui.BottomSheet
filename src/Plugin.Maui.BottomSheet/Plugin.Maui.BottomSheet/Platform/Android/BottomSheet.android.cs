using Android.Content;
using Android.Widget;
using Google.Android.Material.BottomSheet;
using AGravityFlags = Android.Views.GravityFlags;
using AMeasureSpecMode = Android.Views.MeasureSpecMode;
using AView = Android.Views.View;
using AViewGroup = Android.Views.ViewGroup;

namespace Plugin.Maui.BottomSheet.Platform.Android;

using Microsoft.Maui.Platform;

// ReSharper disable once RedundantNameQualifier
using View = Microsoft.Maui.Controls.View;

/// <summary>
/// BottomSheet container view including Handle, Header and Content.
/// </summary>
internal sealed class BottomSheet : IDisposable
{
    private const int HandleRow = 0;
    private const int HeaderRow = 1;
    private const int PeekRow = 2;
    private const int ContentRow = 3;

    private readonly WeakEventManager _eventManager = new();
    private readonly IMauiContext _mauiContext;
    private readonly Context _context;
    private readonly GridLayout _sheetContainer;
    private readonly BottomSheetDialog _bottomSheetDialog;
    private readonly BottomSheetHandle _bottomSheetHandle;
    private readonly BottomSheetCallback _bottomSheetCallback;
    private readonly int _handleMargin;
    private readonly int _headerMargin;

    private BottomSheetHeader? _bottomSheetHeader;
    private BottomSheetPeek? _bottomSheetPeek;
    private BottomSheetContent? _bottomSheetContent;

    private GridLayout.LayoutParams? _handleLayoutParams;
    private GridLayout.LayoutParams? _headerLayoutParams;
    private GridLayout.LayoutParams? _peekLayoutParams;
    private GridLayout.LayoutParams? _contentLayoutParams;

    private View? _virtualBottomSheetPeek;
    private AView? _platformBottomSheetPeek;

    private View? _virtualBottomSheetContent;
    private AView? _platformBottomSheetContent;

    private Color? _backgroundColor;
    private Thickness _padding;

    /// <summary>
    /// Initializes a new instance of the <see cref="BottomSheet"/> class.
    /// </summary>
    /// <param name="context"><see cref="Context"/>.</param>
    /// <param name="mauiContext"><see cref="IMauiContext"/>.</param>
    public BottomSheet(Context context, IMauiContext mauiContext)
    {
        _context = context;
        _mauiContext = mauiContext;
        _bottomSheetHandle = new BottomSheetHandle(context);

        _bottomSheetCallback = new BottomSheetCallback();
        _bottomSheetCallback.StateChanged += BottomSheetCallbackOnStateChanged;

        _handleMargin = Convert.ToInt32(_context.ToPixels(5));
        _headerMargin = Convert.ToInt32(_context.ToPixels(5));

        _sheetContainer = new GridLayout(context)
        {
            Orientation = GridOrientation.Vertical,
            RowCount = 4,
            LayoutParameters = new AViewGroup.MarginLayoutParams(AViewGroup.LayoutParams.MatchParent, AViewGroup.LayoutParams.WrapContent)
            {
                LeftMargin = Convert.ToInt32(_context.ToPixels(_padding.Left)),
                RightMargin = Convert.ToInt32(_context.ToPixels(_padding.Right)),
            },
        };
        _sheetContainer.SetMinimumHeight(_context.Resources?.DisplayMetrics?.HeightPixels ?? 0);

        _bottomSheetDialog = new BottomSheetDialog(context);
        _bottomSheetDialog.Behavior.AddBottomSheetCallback(_bottomSheetCallback);
        _bottomSheetDialog.DismissEvent += BottomSheetDialogOnDismissEvent;
    }

    /// <summary>
    /// Finalizes an instance of the <see cref="BottomSheet"/> class.
    /// </summary>
    ~BottomSheet()
    {
        Dispose(false);
    }

    /// <summary>
    /// Header layout changed
    /// </summary>
    public event EventHandler Closed
    {
        add => _eventManager.AddEventHandler(value);
        remove => _eventManager.RemoveEventHandler(value);
    }

    /// <summary>
    /// BottomSheet state changed.
    /// </summary>
    public event EventHandler<BottomSheetStateChangedEventArgs> StateChanged
    {
        add => _eventManager.AddEventHandler(value);
        remove => _eventManager.RemoveEventHandler(value);
    }

    /// <summary>
    /// Gets or sets the padding for the view.
    /// </summary>
    public Thickness Padding
    {
        get => _padding;
        set
        {
            _padding = value;

            if (_sheetContainer.LayoutParameters is AViewGroup.MarginLayoutParams layoutParams)
            {
                layoutParams.LeftMargin = Convert.ToInt32(_context.ToPixels(_padding.Left));
                layoutParams.RightMargin = Convert.ToInt32(_context.ToPixels(_padding.Right));
            }
        }
    }

    /// <summary>
    /// Gets a value indicating whether BottomSheet is showing.
    /// </summary>
    public bool IsShowing
    {
        get => _bottomSheetDialog.IsShowing;
    }

    /// <summary>
    /// Gets or sets the Color which will fill the background of an element.
    /// </summary>
    public Color BackgroundColor
    {
        get => _backgroundColor ?? Colors.White;
        set
        {
            _backgroundColor = value;
            _sheetContainer.SetBackgroundColor(value.ToPlatform());
        }
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Open the bottom sheet.
    /// </summary>
    /// <param name="bottomSheet">Virtual view.</param>
    public void Open(IBottomSheet bottomSheet)
    {
        if (bottomSheet.HasHandle)
        {
            AddHandle();
        }

        if (bottomSheet.ShowHeader)
        {
            AddHeader();
        }

        AddPeek();
        AddContent();

        _bottomSheetDialog.Behavior.FitToContents = false;
        _bottomSheetDialog.Behavior.HalfExpandedRatio = 0.5f;

        SetState(bottomSheet.CurrentState);
        SetIsCancelable(bottomSheet.IsCancelable);
        SetIsDraggable(bottomSheet.IsDraggable);

        _bottomSheetDialog.SetContentView(_sheetContainer);
        _bottomSheetDialog.Show();
    }

    /// <summary>
    /// Close the bottom sheet.
    /// </summary>
    public void Close()
    {
        _bottomSheetDialog.DismissEvent += BottomSheetDialogOnDismissEvent;
        _bottomSheetCallback.StateChanged += BottomSheetCallbackOnStateChanged;

        if (_bottomSheetPeek is not null)
        {
            _bottomSheetPeek.PropertyChanged -= BottomSheetPeekOnPropertyChanged;
        }

        if (_bottomSheetHeader is not null)
        {
            _bottomSheetHeader.LayoutChanged += BottomSheetHeaderLayoutChanged;
        }

        _bottomSheetDialog.Dismiss();
        _sheetContainer.RemoveFromParent();

        HideHandle();
        HideHeader();
        HidePeek();
        HideContent();
    }

    /// <summary>
    /// Sets whether this dialog is cancelable.
    /// </summary>
    /// <param name="isCancelable">Whether the dialog should be canceled when touched outside the window or by slide.</param>
    public void SetIsCancelable(bool isCancelable)
    {
        _bottomSheetDialog.SetCancelable(isCancelable);
    }

    /// <summary>
    /// Set current state.
    /// </summary>
    /// <param name="state">State to apply.</param>
    public void SetState(BottomSheetState state)
    {
        _bottomSheetDialog.Behavior.State = state.ToPlatformState();
    }

    /// <summary>
    /// Sets whether this dialog is draggable.
    /// </summary>
    /// <param name="isDraggable">Whether the dialog should is draggable.</param>
    public void SetIsDraggable(bool isDraggable)
    {
        _bottomSheetDialog.Behavior.Draggable = isDraggable;
    }

    /// <summary>
    /// Set the <see cref="Plugin.Maui.BottomSheet.BottomSheetHeader"/>.
    /// </summary>
    /// <param name="header">Header.</param>
    public void SetHeader(Plugin.Maui.BottomSheet.BottomSheetHeader header)
    {
        _bottomSheetHeader = new BottomSheetHeader(
            _context,
            _mauiContext,
            header);
        _bottomSheetHeader.LayoutChanged += BottomSheetHeaderLayoutChanged;
    }

    /// <summary>
    /// Set the <see cref="Plugin.Maui.BottomSheet.BottomSheetPeek"/>.
    /// </summary>
    /// <param name="peek">Peek.</param>
    public void SetPeek(BottomSheetPeek peek)
    {
        if (_bottomSheetPeek is not null)
        {
            _bottomSheetPeek.PropertyChanged -= BottomSheetPeekOnPropertyChanged;
        }

        _bottomSheetPeek = peek;

        if (_bottomSheetPeek is not null)
        {
            _bottomSheetPeek.PropertyChanged += BottomSheetPeekOnPropertyChanged;
        }
    }

    /// <summary>
    /// Set the <see cref="Plugin.Maui.BottomSheet.BottomSheetContent"/>.
    /// </summary>
    /// <param name="content">Content.</param>
    public void SetContent(BottomSheetContent content)
    {
        _bottomSheetContent = content;
    }

    /// <summary>
    /// Add a handle view.
    /// </summary>
    public void AddHandle()
    {
        _handleLayoutParams = new GridLayout.LayoutParams()
        {
            TopMargin = _handleMargin,
            Width = Convert.ToInt32(_context.ToPixels(30)),
            Height = Convert.ToInt32(_context.ToPixels(5)),
            RowSpec = GridLayout.InvokeSpec(HandleRow),
        };
        _handleLayoutParams.SetGravity(AGravityFlags.CenterHorizontal);

        _sheetContainer.AddView(_bottomSheetHandle.Handle, _handleLayoutParams);
        BottomSheetHandleLayoutChanged();
    }

    /// <summary>
    /// Adds the header view.
    /// </summary>
    public void AddHeader()
    {
        if (_bottomSheetHeader is null)
        {
            return;
        }

        _headerLayoutParams = new GridLayout.LayoutParams()
        {
            TopMargin = _headerMargin,
            RowSpec = GridLayout.InvokeSpec(HeaderRow),
        };
        _headerLayoutParams.SetGravity(AGravityFlags.Fill);
        _sheetContainer.AddView(_bottomSheetHeader.CreateHeader(), _headerLayoutParams);
    }

    /// <summary>
    /// Adds the peek view.
    /// </summary>
    public void AddPeek()
    {
        if (_bottomSheetPeek is null)
        {
            return;
        }

        _virtualBottomSheetPeek = _bottomSheetPeek.PeekViewDataTemplate?.CreateContent() as View;

        if (_virtualBottomSheetPeek is null)
        {
            return;
        }

        _virtualBottomSheetPeek.BindingContext = _bottomSheetPeek.BindingContext;
        _virtualBottomSheetPeek.Parent = _bottomSheetPeek.Parent;

        _platformBottomSheetPeek = _virtualBottomSheetPeek.ToPlatform(_mauiContext);

        var height = _context.ToPixels(_bottomSheetPeek.PeekHeight);
        if (double.IsNaN(height))
        {
            _platformBottomSheetPeek.Measure(AView.MeasureSpec.MakeMeasureSpec(int.MaxValue, AMeasureSpecMode.AtMost), AView.MeasureSpec.MakeMeasureSpec(int.MaxValue, AMeasureSpecMode.AtMost));
            height = _platformBottomSheetPeek.MeasuredHeight;
        }

        _peekLayoutParams = new GridLayout.LayoutParams()
        {
            Height = Convert.ToInt32(height),
            RowSpec = GridLayout.InvokeSpec(PeekRow),
        };
        _peekLayoutParams.SetGravity(AGravityFlags.Fill);

        _sheetContainer.AddView(_platformBottomSheetPeek, _peekLayoutParams);

        _bottomSheetDialog.Behavior.PeekHeight = double.IsNaN(_bottomSheetPeek.PeekHeight) ? MeasurePeekHeight() : Convert.ToInt32(_bottomSheetPeek.PeekHeight);
        _bottomSheetDialog.Behavior.SkipCollapsed = false;
    }

    /// <summary>
    /// Adds the content view.
    /// </summary>
    public void AddContent()
    {
        if (_bottomSheetContent is null)
        {
            return;
        }

        _virtualBottomSheetContent = _bottomSheetContent.ContentTemplate?.CreateContent() as View;

        if (_virtualBottomSheetContent is null)
        {
            return;
        }

        _virtualBottomSheetContent.BindingContext = _bottomSheetContent.BindingContext;
        _virtualBottomSheetContent.Parent = _bottomSheetContent.Parent;

        _platformBottomSheetContent = _virtualBottomSheetContent.ToPlatform(_mauiContext);

        _contentLayoutParams = new GridLayout.LayoutParams()
        {
            RowSpec = GridLayout.InvokeSpec(ContentRow),
        };
        _contentLayoutParams.SetGravity(AGravityFlags.Fill);

        _sheetContainer.AddView(_platformBottomSheetContent, _contentLayoutParams);
    }

    /// <summary>
    /// Hide the handle.
    /// </summary>
    public void HideHandle()
    {
        _bottomSheetHandle.Remove();
        _handleLayoutParams?.Dispose();
        if (_bottomSheetDialog.IsShowing)
        {
            BottomSheetHandleLayoutChanged();
        }
    }

    /// <summary>
    /// Hide the header.
    /// </summary>
    public void HideHeader()
    {
        _bottomSheetHeader?.Remove();
        _headerLayoutParams?.Dispose();
    }

    /// <summary>
    /// Hide the peek.
    /// </summary>
    public void HidePeek()
    {
        _bottomSheetDialog.Behavior.SkipCollapsed = true;

        _platformBottomSheetPeek?.RemoveFromParent();
        _peekLayoutParams?.Dispose();
#if NET9_0_OR_GREATER
        _virtualBottomSheetPeek?.DisconnectHandlers();
#else
        _virtualBottomSheetPeek?.Handler?.DisconnectHandler();
#endif
        if (_bottomSheetDialog.IsShowing)
        {
            BottomSheetPeekLayoutChanged();
        }
    }

    /// <summary>
    /// Hide the content.
    /// </summary>
    public void HideContent()
    {
        _platformBottomSheetContent?.RemoveFromParent();
        _contentLayoutParams?.Dispose();
#if NET9_0_OR_GREATER
        _virtualBottomSheetContent?.DisconnectHandlers();
#else
        _virtualBottomSheetContent?.Handler?.DisconnectHandler();
#endif
    }

    private int MeasurePeekHeight()
    {
        return _bottomSheetHandle.Handle.Height
            + (_bottomSheetHeader?.HeaderView.Height ?? 0)
            - _handleMargin
            - _headerMargin
            + PeekHeight();
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1422:Validate platform compatibility", Justification = "Validated.")]
    private void BottomSheetHeaderLayoutChanged(object? sender, EventArgs e)
    {
        var height = _bottomSheetHandle.Handle.Height
            + (_bottomSheetHeader?.HeaderView.Height ?? 0)
            - _handleMargin
            - _headerMargin
            + PeekHeight();

        _bottomSheetDialog.Behavior.PeekHeight = height
            + _bottomSheetDialog.Window?.DecorView.RootView?.RootWindowInsets?.SystemWindowInsetBottom ?? 0;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1422:Validate platform compatibility", Justification = "Validated.")]
    private void BottomSheetHandleLayoutChanged()
    {
        var height = _bottomSheetHandle.Handle.Height
             + (_bottomSheetHeader?.HeaderView.Height ?? 0)
             - _handleMargin
             - _headerMargin
             + PeekHeight();

        _bottomSheetDialog.Behavior.PeekHeight = height
            + _bottomSheetDialog.Window?.DecorView.RootView?.RootWindowInsets?.SystemWindowInsetBottom ?? 0;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1422:Validate platform compatibility", Justification = "Validated.")]
    private void BottomSheetPeekLayoutChanged()
    {
        var height = _bottomSheetHandle.Handle.Height
             + (_bottomSheetHeader?.HeaderView.Height ?? 0)
             - _handleMargin
             - _headerMargin
             + PeekHeight();

        _bottomSheetDialog.Behavior.PeekHeight = height
            + _bottomSheetDialog.Window?.DecorView.RootView?.RootWindowInsets?.SystemWindowInsetBottom ?? 0;
    }

    private int PeekHeight()
    {
        if (_bottomSheetPeek is not null
            && !double.IsNaN(_bottomSheetPeek.PeekHeight))
        {
            return Convert.ToInt32(_bottomSheetPeek.PeekHeight);
        }
        else
        {
            return _platformBottomSheetPeek?.Height ?? 0;
        }
    }

    private void Dispose(bool disposing)
    {
        if (!disposing)
        {
            return;
        }

        _bottomSheetDialog.DismissEvent -= BottomSheetDialogOnDismissEvent;

        _sheetContainer.Dispose();
        _bottomSheetHandle.Dispose();
        _bottomSheetHeader?.Dispose();
        _bottomSheetHeader = null;
        _platformBottomSheetPeek?.Dispose();
        _platformBottomSheetPeek = null;
        _platformBottomSheetContent?.Dispose();
        _platformBottomSheetContent = null;

        _bottomSheetDialog.Dispose();

        _handleLayoutParams?.Dispose();
        _handleLayoutParams = null;
        _headerLayoutParams?.Dispose();
        _headerLayoutParams = null;
        _peekLayoutParams?.Dispose();
        _peekLayoutParams = null;
        _contentLayoutParams?.Dispose();
        _contentLayoutParams = null;

        _bottomSheetCallback.Dispose();
    }

    private void BottomSheetPeekOnPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (_bottomSheetPeek is null)
        {
            return;
        }

        if (e.PropertyName == nameof(BottomSheetPeek.PeekHeight))
        {
            _bottomSheetDialog.Behavior.PeekHeight = Convert.ToInt32(_context.ToPixels(_bottomSheetPeek.PeekHeight));
        }
    }

    private void BottomSheetCallbackOnStateChanged(object? sender, BottomSheetStateChangedEventArgs e)
    {
        _eventManager.HandleEvent(
            this,
            new BottomSheetStateChangedEventArgs(e.State),
            nameof(StateChanged));
    }

    private void BottomSheetDialogOnDismissEvent(object? sender, EventArgs e)
    {
        _eventManager.HandleEvent(this, EventArgs.Empty, nameof(Closed));
    }
}