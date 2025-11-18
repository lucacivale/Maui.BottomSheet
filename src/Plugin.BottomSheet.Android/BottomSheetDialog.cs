using AndroidX.CoordinatorLayout.Widget;
using AndroidX.Core.View;
using AsyncAwaitBestPractices;
using Google.Android.Material.Shape;

namespace Plugin.BottomSheet.Android;

/// <summary>
/// Custom bottom sheet dialog with enhanced event handling and back press support.
/// </summary>
internal sealed class BottomSheetDialog : Google.Android.Material.BottomSheet.BottomSheetDialog
{
    private readonly Context _context;
    private readonly WeakEventManager _eventManager = new();

    private readonly BottomSheetDialogTouchOutsideListener _touchOutsideListener;
    private readonly BottomSheetLayoutChangeListener _layoutChangeListener;
    private readonly BottomSheetCallback _bottomSheetCallback;
    private readonly BottomSheetDialogOnBackPressedCallback _onBackPressedCallback;

    private readonly ColorDrawable _backgroundColorDrawable;
    private readonly Color _nonModalWindowBackgroundColor = Color.Transparent;

    private View? _content;

    private List<BottomSheetState> _bottomSheetStates = [BottomSheetState.Medium, BottomSheetState.Large];
    private bool _isModal;
    private bool _isCancelable;
    private Color _windowBackgroundColor;

    /// <summary>
    /// Initializes a new instance of the <see cref="BottomSheetDialog"/> class.
    /// </summary>
    /// <param name="context">The Android context.</param>
    /// <param name="theme">The theme resource ID.</param>
    public BottomSheetDialog(Context context, int theme)
        : base(context, theme)
    {
        _context = context;
        _bottomSheetCallback = new BottomSheetCallback();
        _bottomSheetCallback.StateChanged += BottomSheetCallback_StateChanged;
        Behavior.AddBottomSheetCallback(_bottomSheetCallback);

        Behavior.GestureInsetBottomIgnored = true;
        Behavior.FitToContents = false;

        if (context is not AndroidX.AppCompat.App.AppCompatActivity activity)
        {
            throw new NotSupportedException($"{nameof(Context)} must be of type {nameof(AndroidX.AppCompat.App.AppCompatActivity)}.");
        }

        _touchOutsideListener = new BottomSheetDialogTouchOutsideListener(activity);

        _backgroundColorDrawable = new ColorDrawable();

        _layoutChangeListener = new BottomSheetLayoutChangeListener();
        _layoutChangeListener.LayoutChange += LayoutChangeListener_LayoutChange;
        _onBackPressedCallback = new BottomSheetDialogOnBackPressedCallback(true);
        _onBackPressedCallback.BackPressed += OnBackPressedCallbackOnBackPressed;
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
    /// Occurs when the back button is pressed.
    /// </summary>
    public event EventHandler BackPressed
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
    /// Occurs when the bottom sheet is canceled.
    /// </summary>
    public event EventHandler Canceled
    {
        add => _eventManager.AddEventHandler(value);
        remove => _eventManager.RemoveEventHandler(value);
    }

    /// <summary>
    /// Gets or sets the padding for the bottom sheet.
    /// </summary>
    public Thickness Padding
    {
        get =>
            new(
                Convert.ToDouble(Context.FromPixels(_content?.PaddingLeft ?? 0)),
                Convert.ToDouble(Context.FromPixels(_content?.PaddingTop ?? 0)),
                Convert.ToDouble(Context.FromPixels(_content?.PaddingRight ?? 0)),
                Convert.ToDouble(Context.FromPixels(_content?.PaddingBottom ?? 0)));

        set
        {
            Thickness paddingPx = value.ToPixels(Context);

            _content?.SetPadding(
                Convert.ToInt32(paddingPx.Left),
                Convert.ToInt32(paddingPx.Top),
                Convert.ToInt32(paddingPx.Right),
                Convert.ToInt32(paddingPx.Bottom));
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0046:Convert to conditional expression", Justification = "Decreases readability.")]
    public double CornerRadius
    {
        get
        {
            if (_content?.Parent is View parent
                && parent.Background is MaterialShapeDrawable shapeDrawable)
            {
                return Context.FromPixels(shapeDrawable.TopLeftCornerResolvedSize);
            }

            return 0;
        }

        set
        {
            if (_content?.Parent is View parent
                && parent.Background is MaterialShapeDrawable shapeDrawable)
            {
                shapeDrawable.SetCornerSize(Context.ToPixels(value));
            }
        }
    }

    /// <summary>
    /// Gets or sets the background color of the bottom sheet.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0046:Convert to conditional expression", Justification = "Decreases readability.")]
    public Color BackgroundColor
    {
        get
        {
            if (_content?.Parent is View bottomSheetFrame
                && bottomSheetFrame.BackgroundTintList is not null)
            {
                return new Color(bottomSheetFrame.BackgroundTintList.DefaultColor);
            }

            return Color.Transparent;
        }

        set
        {
            if (_content?.Parent is View bottomSheetFrame)
            {
                bottomSheetFrame.BackgroundTintList = ColorStateList.ValueOf(value);
            }
        }
    }

    public Color WindowBackgroundColor
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Sonar", "S4275: Refactor this getter so that it actually refers to the field '_windowBackgroundColor", Justification = "Return actual background color.")]
        get => _backgroundColorDrawable.Color;
        set
        {
            if (value.Equals(_nonModalWindowBackgroundColor) == false)
            {
                _windowBackgroundColor = value;
            }

            if (IsModal
                || value.Equals(_nonModalWindowBackgroundColor))
            {
                _backgroundColorDrawable.AnimateChangeAsync(_backgroundColorDrawable.Color.ToArgb(), value.ToArgb(), 100).SafeFireAndForget();
            }
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
            ((View?)_content?.Parent)?.GetLocationOnScreen(location);

            int height = Window?.DecorView.Height - location[1] ?? -1;
            int width = _content?.Width ?? 0;

            return new Rect(location[0], location[1], width, height);
        }
    }

    public bool IsModal
    {
        get => _isModal;
        set
        {
            _isModal = value;

            if (_content?.Parent?.Parent is CoordinatorLayout bottomSheetLayout
                && bottomSheetLayout.FindViewById(_Microsoft.Android.Resource.Designer.Resource.Id.touch_outside) is View touchOutside)
            {
                if (_isModal == false)
                {
                    SetCanceledOnTouchOutside(false);
                    touchOutside.SetOnTouchListener(_touchOutsideListener);

                    if (IsShowing)
                    {
                        WindowBackgroundColor = _nonModalWindowBackgroundColor;
                    }
                }
                else
                {
                    SetCanceledOnTouchOutside(_isCancelable);
                    touchOutside.SetOnTouchListener(null);

                    if (IsShowing)
                    {
                        WindowBackgroundColor = _windowBackgroundColor;
                    }
                }
            }
        }
    }

    public BottomSheetState State { get => Behavior.State.ToBottomSheetState(); set => Behavior.State = value.ToPlatformState(); }

    public List<BottomSheetState> States
    {
        get => _bottomSheetStates;
        set
        {
            _bottomSheetStates = value;

            if (_bottomSheetStates.Contains(BottomSheetState.Peek) == false)
            {
                Behavior.SkipCollapsed = true;
            }
        }
    }

    public float HalfExpandedRatio { get => Behavior.HalfExpandedRatio; set => Behavior.HalfExpandedRatio = value; }

    public int MaxHeight
    {
        get => Behavior.MaxHeight;
        set
        {
            if (value != int.MinValue)
            {
                Behavior.MaxHeight = value;
            }
        }
    }

    public int MaxWidth
    {
        get => Behavior.MaxWidth;
        set
        {
            if (value != int.MinValue)
            {
                Behavior.MaxWidth = value;
            }
        }
    }

    public bool Draggable { get => Behavior.Draggable; set => Behavior.Draggable = value; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0046:Convert to conditional expression", Justification = "Decreases readability.")]
    public Thickness Margin
    {
        get
        {
            if (_content?.Parent is not View bottomSheetFrame
                || bottomSheetFrame.LayoutParameters is not ViewGroup.MarginLayoutParams marginLayoutParams)
            {
                return new(0);
            }

            return new Thickness(
                Convert.ToDouble(marginLayoutParams.LeftMargin),
                Convert.ToDouble(marginLayoutParams.RightMargin),
                Convert.ToDouble(marginLayoutParams.TopMargin),
                Convert.ToDouble(marginLayoutParams.BottomMargin)).FromPixels(Context);
        }

        set
        {
            if (_content?.Parent is View bottomSheetFrame
                && bottomSheetFrame.LayoutParameters is ViewGroup.MarginLayoutParams marginLayoutParams)
            {
                Thickness margin = value.ToPixels(Context);

                marginLayoutParams.SetMargins(
                    Convert.ToInt32(margin.Left),
                    0,
                    Convert.ToInt32(margin.Right),
                    0);
            }
        }
    }

    public double PeekHeight
    {
        get => Behavior.PeekHeight;
        set
        {
            if (value < 0)
            {
                return;
            }

            double height = value;

            if (Window is not null
                && height > Window.DecorView.Height)
            {
                height = Window.DecorView.Height;

                if (ViewCompat.GetRootWindowInsets(Window.DecorView) is WindowInsetsCompat insets)
                {
                    AndroidX.Core.Graphics.Insets systemBarInsets = insets.GetInsets(WindowInsetsCompat.Type.SystemBars());
                    height -= systemBarInsets.Top;
                }
            }

            Behavior.PeekHeight = Convert.ToInt32(height);
        }
    }

    /// <summary>
    /// Shows the bottom sheet dialog.
    /// </summary>
    public override void Show()
    {
        ShowEvent += BottomSheetDialog_ShowEvent;

        Window?.SetBackgroundDrawable(_backgroundColorDrawable);

        base.Show();
    }

    public async Task ShowAsync()
    {
        TaskCompletionSource taskCompletionSource = new();

        EventHandler @event = null!;
        @event = (s, e) =>
        {
            _ = taskCompletionSource.TrySetResult();

            ((BottomSheetDialog)s!).ShowEvent -= @event;
        };

        ShowEvent += @event;

        Show();

        await taskCompletionSource.Task.ConfigureAwait(true);
    }

    /// <summary>
    /// Cancels the bottom sheet dialog.
    /// </summary>
    public override void Cancel()
    {
        _eventManager.RaiseEvent(this, EventArgs.Empty, nameof(Canceled));
    }

    /// <summary>
    /// Dismisses the bottom sheet dialog and cleans up resources.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "VSTHRD200:Use \"Async\" suffix for async methods", Justification = "Not applicable here. Hide base member.")]
    public new async Task Dismiss()
    {
        _content?.RemoveOnLayoutChangeListener(_layoutChangeListener);

        _onBackPressedCallback.BackPressed -= OnBackPressedCallbackOnBackPressed;
        _onBackPressedCallback.Remove();

        await _backgroundColorDrawable.AnimateChangeAsync(_backgroundColorDrawable.Color.ToArgb(), Color.Transparent.ToArgb(), 100).ConfigureAwait(true);

        TaskCompletionSource taskCompletionSource = new();

        EventHandler @event = null!;
        @event = (s, e) =>
        {
            _content?.RemoveFromParent();

            ((BottomSheetDialog)s!).DismissEvent -= @event;

            _ = taskCompletionSource.TrySetResult();
        };
        DismissEvent += @event;

        base.Dismiss();

        await taskCompletionSource.Task.ConfigureAwait(true);
    }

    public override void SetContentView(View view)
    {
        _content = view;

        base.SetContentView(view);
    }

    public override void SetCancelable(bool flag)
    {
        _isCancelable = flag;

        base.SetCancelable(flag);

        SetCanceledOnTouchOutside(_isCancelable);
    }

    public override void OnAttachedToWindow()
    {
        if (_context is AndroidX.AppCompat.App.AppCompatActivity activity
            && activity.Window is not null
            && Window is not null)
        {
            WindowInsetsControllerCompat parentInsetsController = WindowCompat.GetInsetsController(activity.Window, activity.Window.DecorView);
            WindowInsetsControllerCompat insetsController = WindowCompat.GetInsetsController(Window, Window.DecorView);

            insetsController.AppearanceLightStatusBars = parentInsetsController.AppearanceLightStatusBars;
        }

        base.OnAttachedToWindow();
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (!disposing)
        {
            return;
        }

        _onBackPressedCallback.Dispose();
        _layoutChangeListener.Dispose();
        _bottomSheetCallback.Dispose();
        _touchOutsideListener.Dispose();
        _backgroundColorDrawable.Dispose();
    }

    private void BottomSheetDialog_ShowEvent(object? sender, EventArgs e)
    {
        Window?.ClearFlags(WindowManagerFlags.DimBehind);

        if (_content?.Parent is View parent
            && parent.LayoutParameters is not null)
        {
            parent.LayoutParameters.Height = ViewGroup.LayoutParams.MatchParent;
        }

        _content?.AddOnLayoutChangeListener(_layoutChangeListener);
        OnBackPressedDispatcher.AddCallback(_onBackPressedCallback);
    }

    /// <summary>
    /// Handles the back pressed callback event.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    private void OnBackPressedCallbackOnBackPressed(object? sender, EventArgs e)
    {
        if (_isCancelable)
        {
            Cancel();
        }

        _eventManager.RaiseEvent(this, EventArgs.Empty, nameof(BackPressed));
    }

    /// <summary>
    /// Handles bottom sheet container layout change events.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    private void LayoutChangeListener_LayoutChange(object? sender, EventArgs e)
    {
        _eventManager.RaiseEvent(this, EventArgs.Empty, nameof(LayoutChanged));
    }

    private void BottomSheetCallback_StateChanged(object? sender, BottomSheetStateChangedEventArgs e)
    {
        if (States.IsStateAllowed(e.State) == false)
        {
            State = States.First();
        }
        else
        {
            _eventManager.RaiseEvent(this, e, nameof(StateChanged));
        }
    }
}