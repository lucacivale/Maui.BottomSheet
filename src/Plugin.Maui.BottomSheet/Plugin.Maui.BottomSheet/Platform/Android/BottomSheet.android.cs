#pragma warning disable SA1200
using Android.Content;
using Android.Widget;
using Google.Android.Material.BottomSheet;
using AGravityFlags = Android.Views.GravityFlags;
using AMeasureSpecMode = Android.Views.MeasureSpecMode;
using AView = Android.Views.View;
using AViewGroup = Android.Views.ViewGroup;
#pragma warning restore SA1200

namespace Plugin.Maui.BottomSheet.Platform.Android;

using Microsoft.Maui.Platform;

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

    private BottomSheetHeader? _bottomSheetHeader;
    private BottomSheetPeek? _bottomSheetPeek;
    private BottomSheetContent? _bottomSheetContent;

    private ContainerView? _bottomSheetHeaderContainer;
    private ContainerView? _bottomSheetPeekContainer;
    private ContainerView? _bottomSheetContentContainer;

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

        _sheetContainer = new GridLayout(context)
        {
            Orientation = GridOrientation.Vertical,
            RowCount = 4,
            LayoutParameters = new AViewGroup.LayoutParams(AViewGroup.LayoutParams.MatchParent, AViewGroup.LayoutParams.MatchParent),
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

            var pixelPadding = _context.ToPixels(_padding.HorizontalThickness());
            _bottomSheetHeaderContainer.SetPadding(pixelPadding);
            _bottomSheetPeekContainer.SetPadding(pixelPadding);
            _bottomSheetContentContainer.SetPadding(pixelPadding);
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
            TopMargin = Convert.ToInt32(_context.ToPixels(5)),
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

        _bottomSheetHeaderContainer = new ContainerView(_mauiContext);
        _bottomSheetHeaderContainer.AddView(_bottomSheetHeader.HeaderView);
        _bottomSheetHeaderContainer.SetPadding(_context.ToPixels(_padding.HorizontalThickness()));

        _headerLayoutParams = new GridLayout.LayoutParams()
        {
            TopMargin = Convert.ToInt32(_context.ToPixels(5)),
            RowSpec = GridLayout.InvokeSpec(HeaderRow),
        };
        _headerLayoutParams.SetGravity(AGravityFlags.Fill);

        _sheetContainer.AddView(_bottomSheetHeaderContainer, _headerLayoutParams);
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
        _platformBottomSheetPeek.LayoutParameters = new AViewGroup.LayoutParams(AViewGroup.LayoutParams.MatchParent, AViewGroup.LayoutParams.MatchParent);

        _bottomSheetPeekContainer = new ContainerView(_mauiContext);
        _bottomSheetPeekContainer.AddView(_platformBottomSheetPeek);
        _bottomSheetPeekContainer.SetPadding(_context.ToPixels(_padding.HorizontalThickness()));

        var height = _context.ToPixels(_bottomSheetPeek.PeekHeight);
        if (height <= 0.00)
        {
            _bottomSheetPeekContainer.Measure(AView.MeasureSpec.MakeMeasureSpec(int.MaxValue, AMeasureSpecMode.AtMost), AView.MeasureSpec.MakeMeasureSpec(int.MaxValue, AMeasureSpecMode.AtMost));
            height = _bottomSheetPeekContainer.MeasuredHeight;
        }

        _peekLayoutParams = new GridLayout.LayoutParams()
        {
            Height = Convert.ToInt32(height),
            Width = _sheetContainer.Width,
            RowSpec = GridLayout.InvokeSpec(PeekRow),
        };
        _peekLayoutParams.SetGravity(AGravityFlags.Fill);

        _sheetContainer.AddView(_bottomSheetPeekContainer, _peekLayoutParams);

        if (double.IsNaN(_bottomSheetPeek.PeekHeight))
        {
            _bottomSheetDialog.Behavior.PeekHeight = MeasurePeekHeight();
        }
        else
        {
            _bottomSheetDialog.Behavior.PeekHeight = Convert.ToInt32(_bottomSheetPeek.PeekHeight);
        }

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
        _platformBottomSheetContent.LayoutParameters = new AViewGroup.LayoutParams(AViewGroup.LayoutParams.MatchParent, AViewGroup.LayoutParams.MatchParent);

        _bottomSheetContentContainer = new ContainerView(_mauiContext);
        _bottomSheetContentContainer.AddView(_platformBottomSheetContent);
        _bottomSheetContentContainer.SetPadding(_context.ToPixels(_padding.HorizontalThickness()));

        _contentLayoutParams = new GridLayout.LayoutParams()
        {
            Width = _sheetContainer.Width,
            RowSpec = GridLayout.InvokeSpec(ContentRow),
        };
        _contentLayoutParams.SetGravity(AGravityFlags.Fill);

        _sheetContainer.AddView(_bottomSheetContentContainer, _contentLayoutParams);
    }

    /// <summary>
    /// Hide the handle.
    /// </summary>
    public void HideHandle()
    {
        _bottomSheetHandle.Remove();
        _handleLayoutParams?.Dispose();
        BottomSheetHandleLayoutChanged();
    }

    /// <summary>
    /// Hide the header.
    /// </summary>
    public void HideHeader()
    {
        _bottomSheetHeader?.Remove();
        _headerLayoutParams?.Dispose();
        _bottomSheetHeaderContainer?.RemoveFromParent();
    }

    /// <summary>
    /// Hide the peek.
    /// </summary>
    public void HidePeek()
    {
        _bottomSheetDialog.Behavior.SkipCollapsed = true;

        _platformBottomSheetPeek?.RemoveFromParent();
        _bottomSheetPeekContainer?.RemoveFromParent();
        _peekLayoutParams?.Dispose();
#if NET9_0_OR_GREATER
        _virtualBottomSheetPeek?.DisconnectHandlers();
#else
        _virtualBottomSheetPeek?.Handler?.DisconnectHandler();
#endif
        BottomSheetPeekLayoutChanged();
    }

    /// <summary>
    /// Hide the content.
    /// </summary>
    public void HideContent()
    {
        _platformBottomSheetContent?.RemoveFromParent();
        _bottomSheetContentContainer?.RemoveFromParent();
        _contentLayoutParams?.Dispose();
#if NET9_0_OR_GREATER
        _virtualBottomSheetContent?.DisconnectHandlers();
#else
        _virtualBottomSheetContent?.Handler?.DisconnectHandler();
#endif
    }

    private int MeasurePeekHeight()
    {
        _bottomSheetHandle.Handle.Measure(AView.MeasureSpec.MakeMeasureSpec(int.MaxValue, AMeasureSpecMode.AtMost), AView.MeasureSpec.MakeMeasureSpec(int.MaxValue, AMeasureSpecMode.AtMost));
        _bottomSheetHeaderContainer?.Measure(AView.MeasureSpec.MakeMeasureSpec(int.MaxValue, AMeasureSpecMode.AtMost), AView.MeasureSpec.MakeMeasureSpec(int.MaxValue, AMeasureSpecMode.AtMost));
        _bottomSheetPeekContainer?.Measure(AView.MeasureSpec.MakeMeasureSpec(int.MaxValue, AMeasureSpecMode.AtMost), AView.MeasureSpec.MakeMeasureSpec(int.MaxValue, AMeasureSpecMode.AtMost));

        return _bottomSheetHandle.Handle.MeasuredHeight
               + (_bottomSheetHeaderContainer?.MeasuredHeight ?? 0)
               + PeekHeight();
    }

    private void BottomSheetHeaderLayoutChanged(object? sender, EventArgs e)
    {
        _bottomSheetHeaderContainer?.Measure(AView.MeasureSpec.MakeMeasureSpec(int.MaxValue, AMeasureSpecMode.AtMost), AView.MeasureSpec.MakeMeasureSpec(int.MaxValue, AMeasureSpecMode.AtMost));

        var height = _bottomSheetHandle.Handle.MeasuredHeight
            + (_bottomSheetHeaderContainer?.MeasuredHeight ?? 0)
            + PeekHeight();

        _bottomSheetDialog.Behavior.PeekHeight = height;
    }

    private void BottomSheetHandleLayoutChanged()
    {
        _bottomSheetHandle.Handle.Measure(AView.MeasureSpec.MakeMeasureSpec(int.MaxValue, AMeasureSpecMode.AtMost), AView.MeasureSpec.MakeMeasureSpec(int.MaxValue, AMeasureSpecMode.AtMost));

        var height = _bottomSheetHandle.Handle.MeasuredHeight
             + (_bottomSheetHeaderContainer?.MeasuredHeight ?? 0)
             + PeekHeight();

        _bottomSheetDialog.Behavior.PeekHeight = height;
    }

    private void BottomSheetPeekLayoutChanged()
    {
        _bottomSheetPeekContainer?.Measure(AView.MeasureSpec.MakeMeasureSpec(int.MaxValue, AMeasureSpecMode.AtMost), AView.MeasureSpec.MakeMeasureSpec(int.MaxValue, AMeasureSpecMode.AtMost));

        var height = _bottomSheetHandle.Handle.MeasuredHeight
             + (_bottomSheetHeaderContainer?.MeasuredHeight ?? 0)
             + PeekHeight();

        _bottomSheetDialog.Behavior.PeekHeight = height;
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
            return _bottomSheetPeekContainer?.MeasuredHeight ?? 0;
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
        _bottomSheetDialog.Dispose();
        _bottomSheetHandle.Dispose();
        _bottomSheetHeader?.Dispose();
        _bottomSheetHeader = null;
        _bottomSheetHeaderContainer?.Dispose();
        _bottomSheetHeaderContainer = null;
        _bottomSheetPeekContainer?.Dispose();
        _bottomSheetPeekContainer = null;
        _bottomSheetContentContainer?.Dispose();
        _bottomSheetContentContainer = null;
        _platformBottomSheetPeek?.Dispose();
        _platformBottomSheetPeek = null;
        _platformBottomSheetContent?.Dispose();
        _platformBottomSheetContent = null;

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
            _bottomSheetDialog.Behavior.PeekHeight = Convert.ToInt32(_bottomSheetPeek.PeekHeight);
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