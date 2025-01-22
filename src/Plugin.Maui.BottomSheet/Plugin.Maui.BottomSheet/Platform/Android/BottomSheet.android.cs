#pragma warning disable SA1200
using Android.Content;
using Android.Widget;
using Google.Android.Material.BottomSheet;
using AColor = Android.Graphics.Color;
using AColorDrawable = Android.Graphics.Drawables.ColorDrawable;
using AGravityFlags = Android.Views.GravityFlags;
using AView = Android.Views.View;
using AViewGroup = Android.Views.ViewGroup;
#pragma warning disable SA1200

namespace Plugin.Maui.BottomSheet.Platform.Android;

using _Microsoft.Android.Resource.Designer;
using Google.Android.Material.Shape;
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
    private readonly BottomSheetLayoutChangeListener _bottomSheetContentChangeListener;
    private readonly int _handleMargin;
    private readonly int _headerMargin;
    private readonly AColorDrawable _backgroundColorDrawable;

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
    private float _cornerRadius;

    /// <summary>
    /// Initializes a new instance of the <see cref="BottomSheet"/> class.
    /// </summary>
    /// <param name="context"><see cref="Context"/>.</param>
    /// <param name="mauiContext"><see cref="IMauiContext"/>.</param>
    public BottomSheet(Context context, IMauiContext mauiContext)
    {
        _context = context;
        _mauiContext = mauiContext;

        _backgroundColorDrawable = new AColorDrawable();
        _backgroundColorDrawable.Color = AColor.Black;
        _backgroundColorDrawable.SetAlpha(80);

        _bottomSheetCallback = new BottomSheetCallback();
        _bottomSheetCallback.StateChanged += BottomSheetCallbackOnStateChanged;

        _bottomSheetDialog = new BottomSheetDialog(context, Resource.Style.ThemeOverlay_App_BottomSheetDialog);
        _bottomSheetDialog.Window?.SetBackgroundDrawable(_backgroundColorDrawable);
        _bottomSheetDialog.Behavior.MaxHeight = MaxHeight();
        _bottomSheetDialog.DismissEvent += BottomSheetDialogOnDismissEvent;

        _bottomSheetHandle = new BottomSheetHandle(context);

        _bottomSheetContentChangeListener = new BottomSheetLayoutChangeListener();
        _bottomSheetContentChangeListener.LayoutChange += BottomSheetContentChanged;

        _sheetContainer = new GridLayout(_context)
        {
            Orientation = GridOrientation.Vertical,
            RowCount = 4,
            LayoutParameters = new AViewGroup.MarginLayoutParams(AViewGroup.LayoutParams.MatchParent, AViewGroup.LayoutParams.WrapContent)
            {
                LeftMargin = Convert.ToInt32(_context.ToPixels(_padding.Left)),
                RightMargin = Convert.ToInt32(_context.ToPixels(_padding.Right)),
            },
        };
        _sheetContainer.SetMinimumHeight(_bottomSheetDialog.Behavior.MaxHeight);

        _handleMargin = Convert.ToInt32(_context.ToPixels(5));
        _headerMargin = Convert.ToInt32(_context.ToPixels(5));
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

        _bottomSheetDialog.Behavior.GestureInsetBottomIgnored = true;
        _bottomSheetDialog.Behavior.FitToContents = false;
        _bottomSheetDialog.Behavior.HalfExpandedRatio = 0.5f;
        _bottomSheetDialog.Behavior.HalfExpandedRatio = 0.5f;
        _bottomSheetDialog.Behavior.AddBottomSheetCallback(_bottomSheetCallback);

        SetState(bottomSheet.CurrentState);
        SetIsCancelable(bottomSheet.IsCancelable);
        SetIsDraggable(bottomSheet.IsDraggable);
        SetCornerRadius(bottomSheet.CornerRadius);

        _bottomSheetDialog.ShowEvent += BottomSheetDialogOnShowEvent;
        _bottomSheetDialog.DismissEvent += BottomSheetDialogOnDismissEvent;
        _bottomSheetDialog.SetContentView(_sheetContainer);
        _bottomSheetDialog.Show();
    }

    /// <summary>
    /// Close the bottom sheet.
    /// </summary>
    public void Close()
    {
        _bottomSheetHandle.Handle.RemoveOnLayoutChangeListener(_bottomSheetContentChangeListener);

        if (_bottomSheetHeader is not null)
        {
            _bottomSheetHeader.LayoutChanged -= BottomSheetContentChanged;
        }

        _platformBottomSheetPeek?.RemoveOnLayoutChangeListener(_bottomSheetContentChangeListener);

        _bottomSheetDialog.Behavior.RemoveBottomSheetCallback(_bottomSheetCallback);

        _bottomSheetDialog.Dismiss();
        _bottomSheetDialog.DismissEvent -= BottomSheetDialogOnDismissEvent;
        _bottomSheetDialog.ShowEvent -= BottomSheetDialogOnShowEvent;

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
        _bottomSheetHeader.LayoutChanged += BottomSheetContentChanged;
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
    /// Set the corner radius.
    /// </summary>
    /// <param name="radius">Corner radius.</param>
    public void SetCornerRadius(float radius)
    {
        _cornerRadius = radius;

        ApplyCornerRadius();
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
        _bottomSheetHandle.Handle.AddOnLayoutChangeListener(_bottomSheetContentChangeListener);
        _sheetContainer.AddView(_bottomSheetHandle.Handle, _handleLayoutParams);
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

        _peekLayoutParams = new GridLayout.LayoutParams()
        {
            RowSpec = GridLayout.InvokeSpec(PeekRow),
        };
        _peekLayoutParams.SetGravity(AGravityFlags.Fill);

        _platformBottomSheetPeek.AddOnLayoutChangeListener(_bottomSheetContentChangeListener);
        _sheetContainer.AddView(_platformBottomSheetPeek, _peekLayoutParams);

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
        _bottomSheetHandle.Handle.RemoveOnLayoutChangeListener(_bottomSheetContentChangeListener);
        _bottomSheetHandle.Remove();
        _handleLayoutParams?.Dispose();
    }

    /// <summary>
    /// Hide the header.
    /// </summary>
    public void HideHeader()
    {
        if (_bottomSheetHeader is not null)
        {
            _bottomSheetHeader.LayoutChanged -= BottomSheetContentChanged;
        }

        _bottomSheetHeader?.Remove();
        _headerLayoutParams?.Dispose();
    }

    /// <summary>
    /// Hide the peek.
    /// </summary>
    public void HidePeek()
    {
        if (_bottomSheetPeek is not null)
        {
            _bottomSheetPeek.PropertyChanged -= BottomSheetPeekOnPropertyChanged;
        }

        _platformBottomSheetPeek?.RemoveOnLayoutChangeListener(_bottomSheetContentChangeListener);
        _bottomSheetDialog.Behavior.SkipCollapsed = true;

        _platformBottomSheetPeek?.RemoveFromParent();
        _peekLayoutParams?.Dispose();
#if NET9_0_OR_GREATER
        _virtualBottomSheetPeek?.DisconnectHandlers();
#else
        _virtualBottomSheetPeek?.Handler?.DisconnectHandler();
#endif
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

    private void ApplyCornerRadius()
    {
        if (_sheetContainer.Parent is AView parent
            && parent.Background is MaterialShapeDrawable shapeDrawable)
        {
            shapeDrawable.SetCornerSize(_context.ToPixels(_cornerRadius));
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1422:Validate platform compatibility", Justification = "Validated.")]
    private void BottomSheetContentChanged(object? sender, EventArgs e)
    {
        var height = _bottomSheetHandle.Handle.Height
            + (_bottomSheetHeader?.HeaderView.Height ?? 0)
            + _handleMargin
            + _headerMargin
            + PeekHeight();

        _bottomSheetDialog.Behavior.PeekHeight = height > _bottomSheetDialog.Behavior.MaxHeight ? _bottomSheetDialog.Behavior.MaxHeight : height;
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

    private void BottomSheetPeekOnPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (_bottomSheetPeek is null)
        {
            return;
        }

        if (e.PropertyName == nameof(BottomSheetPeek.PeekHeight))
        {
            _bottomSheetDialog.Behavior.PeekHeight = PeekHeight() > _bottomSheetDialog.Behavior.MaxHeight ? _bottomSheetDialog.Behavior.PeekHeight : PeekHeight();
        }
    }

    private void BottomSheetCallbackOnStateChanged(object? sender, BottomSheetStateChangedEventArgs e)
    {
        _eventManager.HandleEvent(
            this,
            new BottomSheetStateChangedEventArgs(e.State),
            nameof(StateChanged));
    }

    private void BottomSheetDialogOnShowEvent(object? sender, EventArgs e)
    {
        ApplyCornerRadius();
    }

    private void BottomSheetDialogOnDismissEvent(object? sender, EventArgs e)
    {
        _eventManager.HandleEvent(this, EventArgs.Empty, nameof(Closed));
    }

    private int MaxHeight()
    {
        return _context.Resources?.DisplayMetrics?.HeightPixels ?? 0;
    }

    private void Dispose(bool disposing)
    {
        if (!disposing)
        {
            return;
        }

        _handleLayoutParams?.Dispose();
        _handleLayoutParams = null;
        _headerLayoutParams?.Dispose();
        _headerLayoutParams = null;
        _peekLayoutParams?.Dispose();
        _peekLayoutParams = null;
        _contentLayoutParams?.Dispose();
        _contentLayoutParams = null;

        _bottomSheetHandle.Dispose();
        _bottomSheetHeader?.Dispose();
        _bottomSheetHeader = null;
        _platformBottomSheetPeek?.Dispose();
        _platformBottomSheetPeek = null;
        _platformBottomSheetContent?.Dispose();
        _platformBottomSheetContent = null;

        _sheetContainer.Dispose();

        _bottomSheetContentChangeListener.Dispose();

        _backgroundColorDrawable.Dispose();

        _bottomSheetCallback.StateChanged -= BottomSheetCallbackOnStateChanged;
        _bottomSheetCallback.Dispose();

        _bottomSheetDialog.DismissEvent -= BottomSheetDialogOnDismissEvent;
        _bottomSheetDialog.ShowEvent -= BottomSheetDialogOnShowEvent;
        _bottomSheetDialog.Dispose();
    }
}