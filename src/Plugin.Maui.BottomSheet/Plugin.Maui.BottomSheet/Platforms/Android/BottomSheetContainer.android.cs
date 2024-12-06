#pragma warning disable SA1200
using Android.Content;
using Android.Widget;
using Google.Android.Material.BottomSheet;
using AGravityFlags = Android.Views.GravityFlags;
using AMeasureSpecMode = Android.Views.MeasureSpecMode;
using AView = Android.Views.View;
using AViewGroup = Android.Views.ViewGroup;
#pragma warning restore SA1200

// ReSharper disable once CheckNamespace
namespace Plugin.Maui.BottomSheet.Platforms.Android;

using Microsoft.Maui.Platform;

/// <summary>
/// BottomSheet container view including Handle, Header and Content.
/// </summary>
internal sealed class BottomSheetContainer : IDisposable
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
    /// Initializes a new instance of the <see cref="BottomSheetContainer"/> class.
    /// </summary>
    /// <param name="context"><see cref="Context"/>.</param>
    /// <param name="mauiContext"><see cref="IMauiContext"/>.</param>
    public BottomSheetContainer(Context context, IMauiContext mauiContext)
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
    /// Finalizes an instance of the <see cref="BottomSheetContainer"/> class.
    /// </summary>
    ~BottomSheetContainer()
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

    // ReSharper disable once GrammarMistakeInComment

    /// <summary>
    /// Gets or sets the Color which will fill the background of an element. This is a bindable property.
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

        _bottomSheetDialog.Behavior.PeekHeight = MeasurePeekHeight();
        _bottomSheetDialog.Behavior.SkipCollapsed = _bottomSheetPeek is null;
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
        _bottomSheetPeek = peek;
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
        ArgumentNullException.ThrowIfNull(_bottomSheetHeader);

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
        _virtualBottomSheetPeek?.Handler?.DisconnectHandler();
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
        _virtualBottomSheetContent?.Handler?.DisconnectHandler();
    }

    private void AddPeek()
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

        _bottomSheetDialog.Behavior.SkipCollapsed = false;
    }

    private void AddContent()
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

    private int MeasurePeekHeight()
    {
        _bottomSheetHandle.Handle.Measure(AView.MeasureSpec.MakeMeasureSpec(int.MaxValue, AMeasureSpecMode.AtMost), AView.MeasureSpec.MakeMeasureSpec(int.MaxValue, AMeasureSpecMode.AtMost));
        _bottomSheetHeaderContainer?.Measure(AView.MeasureSpec.MakeMeasureSpec(int.MaxValue, AMeasureSpecMode.AtMost), AView.MeasureSpec.MakeMeasureSpec(int.MaxValue, AMeasureSpecMode.AtMost));
        _bottomSheetPeekContainer?.Measure(AView.MeasureSpec.MakeMeasureSpec(int.MaxValue, AMeasureSpecMode.AtMost), AView.MeasureSpec.MakeMeasureSpec(int.MaxValue, AMeasureSpecMode.AtMost));

        return _bottomSheetHandle.Handle.MeasuredHeight
               + (_bottomSheetHeaderContainer?.MeasuredHeight ?? 0)
               + (_bottomSheetPeekContainer?.MeasuredHeight ?? 0);
    }

    private void BottomSheetHeaderLayoutChanged(object? sender, EventArgs e)
    {
        _bottomSheetHeaderContainer?.Measure(AView.MeasureSpec.MakeMeasureSpec(int.MaxValue, AMeasureSpecMode.AtMost), AView.MeasureSpec.MakeMeasureSpec(int.MaxValue, AMeasureSpecMode.AtMost));

        var height = _bottomSheetHandle.Handle.MeasuredHeight
            + (_bottomSheetHeaderContainer?.MeasuredHeight ?? 0)
            + (_bottomSheetPeekContainer?.MeasuredHeight ?? 0);

        _bottomSheetDialog.Behavior.PeekHeight = height;
    }

    private void BottomSheetHandleLayoutChanged()
    {
        _bottomSheetHandle.Handle.Measure(AView.MeasureSpec.MakeMeasureSpec(int.MaxValue, AMeasureSpecMode.AtMost), AView.MeasureSpec.MakeMeasureSpec(int.MaxValue, AMeasureSpecMode.AtMost));

        var height = _bottomSheetHandle.Handle.MeasuredHeight
             + (_bottomSheetHeaderContainer?.MeasuredHeight ?? 0)
             + (_bottomSheetPeekContainer?.MeasuredHeight ?? 0);

        _bottomSheetDialog.Behavior.PeekHeight = height;
    }

    private void BottomSheetPeekLayoutChanged()
    {
        _bottomSheetPeekContainer?.Measure(AView.MeasureSpec.MakeMeasureSpec(int.MaxValue, AMeasureSpecMode.AtMost), AView.MeasureSpec.MakeMeasureSpec(int.MaxValue, AMeasureSpecMode.AtMost));

        var height = _bottomSheetHandle.Handle.MeasuredHeight
             + (_bottomSheetHeaderContainer?.MeasuredHeight ?? 0)
             + (_bottomSheetPeekContainer?.MeasuredHeight ?? 0);

        _bottomSheetDialog.Behavior.PeekHeight = height;
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