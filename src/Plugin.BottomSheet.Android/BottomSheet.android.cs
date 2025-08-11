#pragma warning disable SA1200
using AColor = Android.Graphics.Color;
using AColorDrawable = Android.Graphics.Drawables.ColorDrawable;
using AColorStateList = Android.Content.Res.ColorStateList;
using AGravityFlags = Android.Views.GravityFlags;
using AView = Android.Views.View;
using AViewGroup = Android.Views.ViewGroup;
using AWindowManagerFlags = Android.Views.WindowManagerFlags;
#pragma warning disable SA1200

namespace Plugin.BottomSheet.Android;

#pragma warning disable SA1135
#pragma warning restore SA1135
using System.Diagnostics.CodeAnalysis;
using AndroidX.CoordinatorLayout.Widget;
using AsyncAwaitBestPractices;
using global::Android.Content;
using Google.Android.Material.BottomSheet;
using Google.Android.Material.Shape;

/// <summary>
/// Android platform implementation of bottom sheet containing handle, header, and content components.
/// </summary>
internal sealed class BottomSheet : IAsyncDisposable, IDisposable
{
    private const int HandleRow = 0;
    private const int HeaderRow = 1;
    private const int ContentRow = 2;

    private readonly WeakEventManager _eventManager = new();
    private readonly Context _context;
    private readonly GridLayout _sheetContainer;
    private readonly BottomSheetHandle _bottomSheetHandle;
    private readonly BottomSheetCallback _bottomSheetCallback;
    private readonly BottomSheetDialogTouchOutsideListener? _touchOutsideListener;
    private readonly BottomSheetLayoutChangeListener _bottomSheetContentChangeListener;
    private readonly BottomSheetLayoutChangeListener _bottomSheetContainerLayoutChangeListener;
    private readonly int _handleMargin;
    private readonly int _headerMargin;
    private readonly AColorDrawable _backgroundColorDrawable;

    private BottomSheetDialog? _bottomSheetDialog;
    private BottomSheetBehavior? _bottomSheetBehavior;

    private GridLayout.LayoutParams? _handleLayoutParams;
    private GridLayout.LayoutParams? _headerLayoutParams;
    private GridLayout.LayoutParams? _contentLayoutParams;

    private double _peekHeight;

    private AView? _platformBottomSheetContent;

    private AColor? _windowBackgroundColor;
    private AColor? _backgroundColor;
    private Thickness _padding;
    private float _cornerRadius;
    private bool _isModal;

    /// <summary>
    /// Initializes a new instance of the <see cref="BottomSheet"/> class.
    /// </summary>
    /// <param name="context">The Android context.</param>
    public BottomSheet(Context context)
    {
        _context = context;

        if (_context is AndroidX.AppCompat.App.AppCompatActivity activity)
        {
            _touchOutsideListener = new BottomSheetDialogTouchOutsideListener(activity);
        }

        _backgroundColorDrawable = new AColorDrawable();

        _bottomSheetCallback = new BottomSheetCallback();
        _bottomSheetCallback.StateChanged += BottomSheetCallbackOnStateChanged;

        _bottomSheetHandle = new BottomSheetHandle(context);

        _bottomSheetContentChangeListener = new BottomSheetLayoutChangeListener();
        _bottomSheetContentChangeListener.LayoutChange += BottomSheetContentChanged;

        _bottomSheetContainerLayoutChangeListener = new BottomSheetLayoutChangeListener();
        _bottomSheetContainerLayoutChangeListener.LayoutChange += BottomSheetContainerLayoutChangeListenerOnLayoutChange;

        _sheetContainer = new GridLayout(_context)
        {
            Orientation = GridOrientation.Vertical,
            RowCount = 4,
            LayoutParameters = new AViewGroup.LayoutParams(AViewGroup.LayoutParams.MatchParent, AViewGroup.LayoutParams.MatchParent),
        };

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
    /// Occurs when the bottom sheet is opened.
    /// </summary>
    public event EventHandler Opened
    {
        add => _eventManager.AddEventHandler(value);
        remove => _eventManager.RemoveEventHandler(value);
    }

    /// <summary>
    /// Occurs when the bottom sheet is closed.
    /// </summary>
    public event EventHandler Closed
    {
        add => _eventManager.AddEventHandler(value);
        remove => _eventManager.RemoveEventHandler(value);
    }

    /// <summary>
    /// Occurs when the bottom sheet state changes.
    /// </summary>
    public event EventHandler<BottomSheetStateChangedEventArgs> StateChanged
    {
        add => _eventManager.AddEventHandler(value);
        remove => _eventManager.RemoveEventHandler(value);
    }

    /// <summary>
    /// Occurs when the back button is pressed.
    /// </summary>
    public event EventHandler BackPressed
    {
        add => _eventManager.AddEventHandler(value);
        remove => _eventManager.RemoveEventHandler(value);
    }

    /// <summary>
    /// Occurs when the bottom sheet layout changes.
    /// </summary>
    public event EventHandler LayoutChanged
    {
        add => _eventManager.AddEventHandler(value);
        remove => _eventManager.RemoveEventHandler(value);
    }

    /// <summary>
    /// Gets or sets the padding for the bottom sheet.
    /// </summary>
    public Thickness Padding
    {
        get => _padding;
        set
        {
            _padding = value;
            ApplyPadding();
        }
    }

    /// <summary>
    /// Gets a value indicating whether the bottom sheet is currently showing.
    /// </summary>
    public bool IsShowing => _bottomSheetDialog?.IsShowing == true;

    /// <summary>
    /// Gets or sets the background color of the bottom sheet.
    /// </summary>
    public AColor? BackgroundColor
    {
        get => _backgroundColor;
        set
        {
            _backgroundColor = value;
            ApplyBackgroundColor();
        }
    }

    /// <summary>
    /// Gets the frame rectangle of the bottom sheet in pixels.
    /// </summary>
    public Rect Frame
    {
        get
        {
            int[] location = new int[2];
            ((AView?)_sheetContainer.Parent)?.GetLocationOnScreen(location);

            int height = _bottomSheetDialog?.Window?.DecorView.Height - location[1] ?? -1;
            int width = _sheetContainer.Width;

            return new Rect(location[0], location[1] - 144, width, height);
        }
    }

    /// <summary>
    /// Releases all resources used by the bottom sheet asynchronous.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    [SuppressMessage("Usage", "CA1816: Dispose methods should call SuppressFinalize", Justification = "This is handled by Dispose.")]
    public async ValueTask DisposeAsync()
    {
        if (IsShowing)
        {
            await CloseAsync().ConfigureAwait(true);
        }

        Dispose();
    }

    /// <summary>
    /// Releases all resources used by the bottom sheet.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Opens the bottom sheet with the specified configuration.
    /// </summary>
    /// <param name="bottomSheet">The bottom sheet configuration to display.</param>
    public void Open(IBottomSheet bottomSheet)
    {
        _bottomSheetDialog = new BottomSheetDialog(
            _context,
            bottomSheet.GetTheme(),
            _bottomSheetCallback);
        _bottomSheetDialog.Window?.ClearFlags(AWindowManagerFlags.DimBehind);
        _bottomSheetDialog.Window?.SetBackgroundDrawable(_backgroundColorDrawable);
        _bottomSheetDialog.ShowEvent += BottomSheetShowed;
        _bottomSheetDialog.Canceled += BottomSheetDialogOnCanceled;
        _bottomSheetDialog.BackPressed += BottomSheetDialogOnBackPressed;

        _bottomSheetBehavior = _bottomSheetDialog.Behavior;

        Padding = bottomSheet.Padding;
        BackgroundColor = bottomSheet.BackgroundColor;

        SetHeader(bottomSheet.Header, bottomSheet.BottomSheetStyle.HeaderStyle);
        SetContent(bottomSheet.Content);

        if (bottomSheet.HasHandle)
        {
            AddHandle();
        }

        if (bottomSheet.ShowHeader)
        {
            AddHeader();
        }

        AddContent();

        _bottomSheetBehavior.GestureInsetBottomIgnored = true;
        _bottomSheetBehavior.FitToContents = false;
        _bottomSheetBehavior.HalfExpandedRatio = bottomSheet.GetHalfExpandedRatio();

        _sheetContainer.AddOnLayoutChangeListener(_bottomSheetContainerLayoutChangeListener);

        _bottomSheetDialog.SetContentView(_sheetContainer);

        SetHeaderStyle(bottomSheet.BottomSheetStyle.HeaderStyle);
        SetIsModal(bottomSheet.IsModal);
        SetState(bottomSheet.CurrentState);
        SetIsCancelable(bottomSheet.IsCancelable);
        SetIsDraggable(bottomSheet.IsDraggable);
        SetCornerRadius(bottomSheet.CornerRadius);
        SetWindowBackgroundColor(bottomSheet.WindowBackgroundColor);
        SetPeekHeight(_context.ToPixels(bottomSheet.PeekHeight));
        SetMaxHeight(bottomSheet.GetMaxHeight());
        SetMaxWidth(bottomSheet.GetMaxWidth());
        SetMargin(bottomSheet.GetMargin());

        _bottomSheetDialog.Show();
    }

    /// <summary>
    /// Closes the bottom sheet asynchronously.
    /// </summary>
    /// <param name="handleClose">Whether to fire the close event.</param>
    /// <returns>A task representing the asynchronous close operation.</returns>
    public async Task CloseAsync(bool handleClose = true)
    {
        _backgroundColorDrawable.AnimateChange(_backgroundColorDrawable.Color.ToArgb(), Colors.Transparent.ToPlatform().ToArgb(), 100);

        _bottomSheetDialog?.Window?.DecorView.PostDelayed(
            () =>
            {
                _sheetContainer.RemoveOnLayoutChangeListener(_bottomSheetContainerLayoutChangeListener);

                _bottomSheetHandle.Handle.RemoveOnLayoutChangeListener(_bottomSheetContentChangeListener);

                if (_bottomSheetHeader is not null)
                {
                    _bottomSheetHeader.LayoutChanged -= BottomSheetContentChanged;
                    _bottomSheetHeader.CloseButtonClicked -= BottomSheetHeaderOnCloseButtonClicked;
                }

                if (_bottomSheetDialog is not null)
                {
                    _bottomSheetDialog.ShowEvent -= BottomSheetShowed;
                    _bottomSheetDialog.Canceled -= BottomSheetDialogOnCanceled;
                    _bottomSheetDialog.BackPressed -= BottomSheetDialogOnBackPressed;
                }

                _sheetContainer.RemoveFromParent();

                HideHandle();
                HideHeader();
                HideContent();

                _bottomSheetBehavior?.Dispose();
                _bottomSheetBehavior = null;
                _bottomSheetDialog?.Dismiss();
                _bottomSheetDialog = null;

                if (handleClose)
                {
                    _eventManager.HandleEvent(this, EventArgs.Empty, nameof(Closed));
                }
            },
            110);

        await Task.Delay(120).ConfigureAwait(true);
    }

    /// <summary>
    /// Sets whether the bottom sheet is modal.
    /// </summary>
    /// <param name="isModal">True if the bottom sheet should be modal.</param>
    public void SetIsModal(bool isModal)
    {
        _isModal = isModal;

        if (_bottomSheetDialog is null)
        {
            return;
        }

        if (_touchOutsideListener is not null
            && _sheetContainer.Parent?.Parent is CoordinatorLayout bottomSheetLayout
            && bottomSheetLayout.FindViewById(_Microsoft.Android.Resource.Designer.Resource.Id.touch_outside) is AView touchOutside)
        {
            if (_isModal == false)
            {
                _bottomSheetDialog.SetCanceledOnTouchOutside(false);
                touchOutside.SetOnTouchListener(_touchOutsideListener);

                if (IsShowing)
                {
                    _backgroundColorDrawable.AnimateChange(_backgroundColorDrawable.Color.ToArgb(), Colors.Transparent.ToPlatform().ToArgb());
                }
            }
            else
            {
                _bottomSheetDialog.SetCanceledOnTouchOutside(true);
                touchOutside.SetOnTouchListener(null);

                if (IsShowing)
                {
                    ApplyWindowBackgroundColor();
                }
            }
        }
    }

    /// <summary>
    /// Sets whether the bottom sheet is cancelable.
    /// </summary>
    /// <param name="isCancelable">True if the bottom sheet can be canceled by touch outside or slide.</param>
    public void SetIsCancelable(bool isCancelable)
    {
        _bottomSheetDialog?.SetCancelable(isCancelable);
    }

    /// <summary>
    /// Sets the maximum height of the bottom sheet.
    /// </summary>
    /// <param name="height">The maximum height in density-independent pixels.</param>
    public void SetMaxHeight(int height)
    {
        if (_bottomSheetBehavior is null
            || height == int.MinValue)
        {
            return;
        }

        _bottomSheetBehavior.MaxHeight = Convert.ToInt32(_context.ToPixels(height));
    }

    /// <summary>
    /// Sets the maximum width of the bottom sheet.
    /// </summary>
    /// <param name="width">The maximum width in density-independent pixels.</param>
    public void SetMaxWidth(int width)
    {
        if (_bottomSheetBehavior is null
            || width == int.MinValue)
        {
            return;
        }

        _bottomSheetBehavior.MaxWidth = Convert.ToInt32(_context.ToPixels(width));
    }

    /// <summary>
    /// Sets the margin around the bottom sheet.
    /// </summary>
    /// <param name="margin">The margin thickness in density-independent pixels.</param>
    public void SetMargin(Thickness margin)
    {
        if (_sheetContainer.Parent is not AView bottomSheetFrame
            || bottomSheetFrame.LayoutParameters is not AViewGroup.MarginLayoutParams marginLayoutParams)
        {
            return;
        }

        var pixelMargin = _context.ToPixels(margin);

        marginLayoutParams.SetMargins(
            Convert.ToInt32(pixelMargin.Left),
            0,
            Convert.ToInt32(pixelMargin.Right),
            0);
    }

    /// <summary>
    /// Sets the current state of the bottom sheet.
    /// </summary>
    /// <param name="state">The desired bottom sheet state.</param>
    public void SetState(BottomSheetState state)
    {
        if (_bottomSheetBehavior is null)
        {
            return;
        }

        _bottomSheetBehavior.State = state.ToPlatformState();
    }

    /// <summary>
    /// Sets whether the bottom sheet is draggable.
    /// </summary>
    /// <param name="isDraggable">True if user can drag the bottom sheet.</param>
    public void SetIsDraggable(bool isDraggable)
    {
        if (_bottomSheetBehavior is null)
        {
            return;
        }

        _bottomSheetBehavior.Draggable = isDraggable;
    }

    /// <summary>
    /// Sets the header for the bottom sheet.
    /// </summary>
    /// <param name="header">The header configuration.</param>
    /// <param name="style">The header style.</param>
    public void SetHeader(Plugin.Maui.BottomSheet.BottomSheetHeader? header, BottomSheetHeaderStyle style)
    {
        if (header is null)
        {
            return;
        }

        if (IsShowing)
        {
            HideHeader();
            _bottomSheetHeader?.Dispose();
        }

        _bottomSheetHeader = new BottomSheetHeader(
            _context,
            _mauiContext,
            header,
            style);

        if (IsShowing)
        {
            AddHeader();
        }
    }

    /// <summary>
    /// Sets the header style for the bottom sheet.
    /// </summary>
    /// <param name="style">The header style to apply.</param>
    public void SetHeaderStyle(BottomSheetHeaderStyle style)
    {
        _bottomSheetHeader?.SetStyle(style);
    }

    /// <summary>
    /// Sets the content for the bottom sheet.
    /// </summary>
    /// <param name="content">The content to display.</param>
    public void SetContent(BottomSheetContent? content)
    {
        if (content is null)
        {
            return;
        }

        if (IsShowing)
        {
            HideContent();
        }

        _bottomSheetContent = content;

        if (IsShowing)
        {
            AddContent();
        }
    }

    /// <summary>
    /// Sets the corner radius of the bottom sheet.
    /// </summary>
    /// <param name="radius">The corner radius in density-independent pixels.</param>
    public void SetCornerRadius(float radius)
    {
        _cornerRadius = radius;

        ApplyCornerRadius();
    }

    /// <summary>
    /// Sets the window background color.
    /// </summary>
    /// <param name="color">The background color.</param>
    /// <param name="apply">Whether to immediately apply the color.</param>
    public void SetWindowBackgroundColor(Color? color, bool apply = false)
    {
        _windowBackgroundColor = color;

        if (apply)
        {
            ApplyWindowBackgroundColor();
        }
    }

    /// <summary>
    /// Sets the peek height of the bottom sheet.
    /// </summary>
    /// <param name="peekHeight">The peek height in pixels.</param>
    public void SetPeekHeight(double peekHeight)
    {
        if (_bottomSheetBehavior is null)
        {
            return;
        }

        _peekHeight = peekHeight;

        if (_peekHeight <= 0.00)
        {
            _bottomSheetBehavior.PeekHeight = 0;
            _bottomSheetBehavior.SkipCollapsed = true;
        }
        else
        {
            var height = _bottomSheetHandle.Handle.Height
                + (_bottomSheetHeader?.Height ?? 0)
                + _handleMargin
                + _headerMargin
                + _peekHeight;

            _bottomSheetBehavior.PeekHeight = Convert.ToInt32(height);
            _bottomSheetBehavior.SkipCollapsed = false;
        }
    }

    /// <summary>
    /// Adds the handle view to the bottom sheet.
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
    /// Adds the header view to the bottom sheet.
    /// </summary>
    public void AddHeader()
    {
        if (_bottomSheetHeader is null)
        {
            return;
        }

        _bottomSheetHeader.LayoutChanged += BottomSheetContentChanged;
        _bottomSheetHeader.CloseButtonClicked += BottomSheetHeaderOnCloseButtonClicked;

        _headerLayoutParams = new GridLayout.LayoutParams()
        {
            TopMargin = _headerMargin,
            RowSpec = GridLayout.InvokeSpec(HeaderRow),
        };
        _headerLayoutParams.SetGravity(AGravityFlags.Fill);
        _sheetContainer.AddView(_bottomSheetHeader.CreateHeader(), _headerLayoutParams);
    }

    /// <summary>
    /// Adds the content view to the bottom sheet.
    /// </summary>
    public void AddContent()
    {
        if (_bottomSheetContent is null)
        {
            return;
        }

        _virtualBottomSheetContent = _bottomSheetContent.CreateContent();
        _platformBottomSheetContent = _virtualBottomSheetContent.ToPlatform(_mauiContext);

        var fillAvailableSpace = _mauiContext.Services.GetRequiredService<Configuration>().FeatureFlags.ContentFillsAvailableSpace;

        _contentLayoutParams = new GridLayout.LayoutParams()
        {
            RowSpec = fillAvailableSpace ? GridLayout.InvokeSpec(ContentRow, 1f) : GridLayout.InvokeSpec(ContentRow),
        };

        if (fillAvailableSpace)
        {
            _contentLayoutParams.Height = 0;
        }

        _contentLayoutParams.SetGravity(AGravityFlags.Fill);

        _sheetContainer.AddView(_platformBottomSheetContent, _contentLayoutParams);
    }

    /// <summary>
    /// Hides the handle view from the bottom sheet.
    /// </summary>
    public void HideHandle()
    {
        _bottomSheetHandle.Handle.RemoveOnLayoutChangeListener(_bottomSheetContentChangeListener);
        _bottomSheetHandle.Remove();
        _handleLayoutParams?.Dispose();
        SetPeekHeight(_peekHeight);
    }

    /// <summary>
    /// Hides the header view from the bottom sheet.
    /// </summary>
    public void HideHeader()
    {
        if (_bottomSheetHeader is not null)
        {
            _bottomSheetHeader.LayoutChanged -= BottomSheetContentChanged;
            _bottomSheetHeader.CloseButtonClicked -= BottomSheetHeaderOnCloseButtonClicked;
        }

        _bottomSheetHeader?.Remove();
        _headerLayoutParams?.Dispose();
        SetPeekHeight(_peekHeight);
    }

    /// <summary>
    /// Hides the content view from the bottom sheet.
    /// </summary>
    public void HideContent()
    {
        _platformBottomSheetContent?.RemoveFromParent();
        _contentLayoutParams?.Dispose();
        _virtualBottomSheetContent?.DisconnectHandlers();
    }

    /// <summary>
    /// Applies the corner radius to the bottom sheet background.
    /// </summary>
    private void ApplyCornerRadius()
    {
        if (_sheetContainer.Parent is AView parent
            && parent.Background is MaterialShapeDrawable shapeDrawable)
        {
            shapeDrawable.SetCornerSize(_context.ToPixels(_cornerRadius));
        }
    }

    /// <summary>
    /// Applies the padding to the sheet container.
    /// </summary>
    private void ApplyPadding()
    {
        _sheetContainer.SetPadding(
            Convert.ToInt32(_context.ToPixels(_padding.Left)),
            0,
            Convert.ToInt32(_context.ToPixels(_padding.Right)),
            0);
    }

    /// <summary>
    /// Applies the background color to the bottom sheet frame.
    /// </summary>
    private void ApplyBackgroundColor()
    {
        if (_sheetContainer.Parent is AView bottomSheetFrame
            && BackgroundColor is not null)
        {
            bottomSheetFrame.BackgroundTintList = AColorStateList.ValueOf(BackgroundColor.ToPlatform());
        }
    }

    /// <summary>
    /// Applies the window background color with animation.
    /// </summary>
    private void ApplyWindowBackgroundColor()
    {
        if (_isModal
            && _windowBackgroundColor is not null)
        {
            _backgroundColorDrawable.AnimateChange(_backgroundColorDrawable.Color.ToArgb(), _windowBackgroundColor.ToPlatform());
        }
    }

    /// <summary>
    /// Handles bottom sheet content change events.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    private void BottomSheetContentChanged(object? sender, EventArgs e)
    {
        SetPeekHeight(_peekHeight);
    }

    /// <summary>
    /// Handles bottom sheet state change events.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The state change event arguments.</param>
    private void BottomSheetCallbackOnStateChanged(object? sender, BottomSheetStateChangedEventArgs e)
    {
        _eventManager.HandleEvent(
            this,
            new BottomSheetStateChangedEventArgs(e.State),
            nameof(StateChanged));
    }

    /// <summary>
    /// Handles the bottom sheet show event.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    private void BottomSheetShowed(object? sender, EventArgs e)
    {
        _eventManager.HandleEvent(this, EventArgs.Empty, nameof(Opened));

        if (_sheetContainer.Parent is not AView parent)
        {
            return;
        }

        if (parent.LayoutParameters is not null)
        {
            parent.LayoutParameters.Height = AViewGroup.LayoutParams.MatchParent;
        }

        ApplyCornerRadius();
        ApplyBackgroundColor();

        _bottomSheetDialog?.Window?.DecorView.PostDelayed(ApplyWindowBackgroundColor, 50);
    }

    /// <summary>
    /// Handles header close button click events.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    private void BottomSheetHeaderOnCloseButtonClicked(object? sender, EventArgs e)
    {
        _eventManager.HandleEvent(this, EventArgs.Empty, nameof(Closed));
    }

    /// <summary>
    /// Handles bottom sheet dialog canceled events.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    private void BottomSheetDialogOnCanceled(object? sender, EventArgs e)
    {
        _eventManager.HandleEvent(this, EventArgs.Empty, nameof(Closed));
    }

    /// <summary>
    /// Handles bottom sheet dialog back pressed events.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    private void BottomSheetDialogOnBackPressed(object? sender, EventArgs e)
    {
        _eventManager.HandleEvent(this, EventArgs.Empty, nameof(BackPressed));
    }

    /// <summary>
    /// Handles bottom sheet container layout change events.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    private void BottomSheetContainerLayoutChangeListenerOnLayoutChange(object? sender, EventArgs e)
    {
        _eventManager.HandleEvent(this, EventArgs.Empty, nameof(LayoutChanged));
    }

    /// <summary>
    /// Releases managed and unmanaged resources.
    /// </summary>
    /// <param name="disposing">True if disposing managed resources.</param>
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
        _platformBottomSheetContent?.Dispose();
        _platformBottomSheetContent = null;

        _bottomSheetContainerLayoutChangeListener.LayoutChange -= BottomSheetContentChanged;
        _bottomSheetContainerLayoutChangeListener.Dispose();

        _sheetContainer.Dispose();

        _touchOutsideListener?.Dispose();
        _bottomSheetContentChangeListener.Dispose();

        _backgroundColorDrawable.Dispose();

        _bottomSheetCallback.StateChanged -= BottomSheetCallbackOnStateChanged;
        _bottomSheetCallback.Dispose();

        _bottomSheetBehavior?.Dispose();

        if (_bottomSheetDialog is not null)
        {
            _bottomSheetDialog.ShowEvent -= BottomSheetShowed;
            _bottomSheetDialog.Canceled -= BottomSheetDialogOnCanceled;
        }

        _bottomSheetDialog?.Dispose();
    }
}