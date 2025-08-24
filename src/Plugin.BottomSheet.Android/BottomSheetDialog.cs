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
    private const int HandleRow = 0;
    private const int HeaderRow = 1;
    private const int ContentRow = 2;

    private const int HandleMargin = 5;
    private const int HeaderMargin = 5;

    private readonly WeakEventManager _eventManager = new();

    private readonly BottomSheetDialogTouchOutsideListener _touchOutsideListener;
    private readonly BottomSheetLayoutChangeListener _layoutChangeListener;
    private readonly BottomSheetCallback _bottomSheetCallback;
    private readonly BottomSheetDialogOnBackPressedCallback _onBackPressedCallback;

    private readonly GridLayout _sheetContainer;

    private readonly ColorDrawable _backgroundColorDrawable;

    private GridLayout.LayoutParams? _handleLayoutParams;
    private GridLayout.LayoutParams? _headerLayoutParams;
    private GridLayout.LayoutParams? _contentLayoutParams;

    private BottomSheetHandle? _bottomSheetHandle;

    private List<BottomSheetState> _bottomSheetStates = [BottomSheetState.Medium, BottomSheetState.Large];
    private bool _isModal;
    private bool _isCancelable;

    /// <summary>
    /// Initializes a new instance of the <see cref="BottomSheetDialog"/> class.
    /// </summary>
    /// <param name="context">The Android context.</param>
    /// <param name="theme">The theme resource ID.</param>
    public BottomSheetDialog(Context context, int theme)
        : base(context, theme)
    {
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

        _sheetContainer = new GridLayout(Context)
        {
            Orientation = GridOrientation.Vertical,
            RowCount = 3,
            LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent),
        };

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
        get => new(
            Convert.ToDouble(Context.FromPixels(_sheetContainer.PaddingLeft)),
            Convert.ToDouble(Context.FromPixels(_sheetContainer.PaddingTop)),
            Convert.ToDouble(Context.FromPixels(_sheetContainer.PaddingRight)),
            Convert.ToDouble(Context.FromPixels(_sheetContainer.PaddingBottom)));
        set =>
            _sheetContainer.SetPadding(
                Convert.ToInt32(Context.ToPixels(value.Left)),
                Convert.ToInt32(Context.ToPixels(value.Top)),
                Convert.ToInt32(Context.ToPixels(value.Right)),
                Convert.ToInt32(Context.ToPixels(value.Bottom)));
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0046:Convert to conditional expression", Justification = "Decreases readability.")]
    public double CornerRadius
    {
        get
        {
            if (_sheetContainer.Parent is View parent
                && parent.Background is MaterialShapeDrawable shapeDrawable)
            {
                return Context.FromPixels(shapeDrawable.TopLeftCornerResolvedSize);
            }

            return 0;
        }

        set
        {
            if (_sheetContainer.Parent is View parent
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
            if (_sheetContainer.Parent is View bottomSheetFrame
                && bottomSheetFrame.BackgroundTintList is not null)
            {
                return new Color(bottomSheetFrame.BackgroundTintList.DefaultColor);
            }

            return Color.Transparent;
        }

        set
        {
            if (_sheetContainer.Parent is View bottomSheetFrame)
            {
                bottomSheetFrame.BackgroundTintList = ColorStateList.ValueOf(value);
            }
        }
    }

    public Color WindowBackgroundColor
    {
        get => _backgroundColorDrawable.Color;
        set
        {
            if (IsModal)
            {
                _backgroundColorDrawable.AnimateChangeAsync(_backgroundColorDrawable.Color.ToArgb(), value).SafeFireAndForget();
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
            ((View?)_sheetContainer.Parent)?.GetLocationOnScreen(location);

            int height = Window?.DecorView.Height - location[1] ?? -1;
            int width = _sheetContainer.Width;

            return new Rect(location[0], location[1] - 144, width, height);
        }
    }

    public bool IsModal
    {
        get => _isModal;
        set
        {
            _isModal = value;

            if (_sheetContainer.Parent?.Parent is CoordinatorLayout bottomSheetLayout
                && bottomSheetLayout.FindViewById(_Microsoft.Android.Resource.Designer.Resource.Id.touch_outside) is View touchOutside)
            {
                if (_isModal == false)
                {
                    SetCanceledOnTouchOutside(false);
                    touchOutside.SetOnTouchListener(_touchOutsideListener);

                    if (IsShowing)
                    {
                        _backgroundColorDrawable.AnimateChangeAsync(_backgroundColorDrawable.Color.ToArgb(), Color.Transparent.ToArgb()).SafeFireAndForget();
                    }
                }
                else
                {
                    SetCanceledOnTouchOutside(_isCancelable);
                    touchOutside.SetOnTouchListener(null);

                    if (IsShowing)
                    {
                        _backgroundColorDrawable.AnimateChangeAsync(_backgroundColorDrawable.Color.ToArgb(), WindowBackgroundColor).SafeFireAndForget();
                    }
                }
            }
        }
    }

    public bool HasHandle
    {
        get => _sheetContainer.GetChildAt(HandleRow) is not null;
        set
        {
            if (value)
            {
                _bottomSheetHandle = new BottomSheetHandle(Context);

                _handleLayoutParams = new GridLayout.LayoutParams
                {
                    TopMargin = Convert.ToInt32(Context.ToPixels(HandleMargin)),
                    Width = Convert.ToInt32(Context.ToPixels(30)),
                    Height = Convert.ToInt32(Context.ToPixels(5)),
                    RowSpec = GridLayout.InvokeSpec(HandleRow),
                };
                _handleLayoutParams.SetGravity(GravityFlags.CenterHorizontal);

                _sheetContainer.AddView(_bottomSheetHandle, _handleLayoutParams);
            }
            else
            {
                if (_bottomSheetHandle is not null)
                {
                    _bottomSheetHandle.LayoutParameters?.Dispose();
                    _bottomSheetHandle.LayoutParameters = null;

                    _bottomSheetHandle.RemoveFromParent();
                    _bottomSheetHandle.Dispose();
                    _bottomSheetHandle = null;
                }
            }
        }
    }

    public BottomSheetState State { get => Behavior.State.ToBottomSheetState(); set => Behavior.State = value.ToPlatformState(); }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S6608:Prefer indexing instead of \"Enumerable\" methods on types implementing \"IList\"", Justification = "Improced readability.")]
    public List<BottomSheetState> States
    {
        get => _bottomSheetStates;
        set
        {
            _bottomSheetStates = value;

            if (_bottomSheetStates.IsStateAllowed(State) == false)
            {
                State = _bottomSheetStates.First();
            }

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
            if (_sheetContainer.Parent is not View bottomSheetFrame
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
            if (_sheetContainer.Parent is View bottomSheetFrame
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
        get
        {
            double height = (_sheetContainer.GetChildAt(HeaderRow)?.Height ?? 0)
                + (_sheetContainer.GetChildAt(HeaderRow)?.Height ?? 0)
                + Convert.ToInt32(
                    Context.ToPixels(
                        HandleMargin
                        + HeaderMargin));

            return Context.FromPixels(Behavior.PeekHeight - height);
        }

        set
        {
            double height = (_sheetContainer.GetChildAt(HeaderRow)?.Height ?? 0)
                + (_sheetContainer.GetChildAt(HeaderRow)?.Height ?? 0)
                + Convert.ToInt32(
                    Context.ToPixels(
                        HandleMargin
                        + HeaderMargin
                        + value));

            Behavior.PeekHeight = Convert.ToInt32(height);
        }
    }

    /// <summary>
    /// Shows the bottom sheet dialog.
    /// </summary>
    public override void Show()
    {
        ShowEvent += BottomSheetDialog_ShowEvent;

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
        _sheetContainer.RemoveOnLayoutChangeListener(_layoutChangeListener);

        _onBackPressedCallback.BackPressed -= OnBackPressedCallbackOnBackPressed;
        _onBackPressedCallback.Remove();

        await _backgroundColorDrawable.AnimateChangeAsync(_backgroundColorDrawable.Color.ToArgb(), Color.Transparent.ToArgb(), 100).ConfigureAwait(true);

        TaskCompletionSource taskCompletionSource = new();

        EventHandler @event = null!;
        @event = (s, e) =>
        {
            _sheetContainer.RemoveFromParent();
            _sheetContainer.RemoveAllViews();

            ((BottomSheetDialog)s!).DismissEvent -= @event;

            _ = taskCompletionSource.TrySetResult();
        };
        DismissEvent += @event;

        base.Dismiss();

        await taskCompletionSource.Task.ConfigureAwait(true);
    }

    public override void SetContentView(View view)
    {
        _sheetContainer.RemoveFromParent();

        RemoveRow(ContentRow);

        _contentLayoutParams = new GridLayout.LayoutParams()
        {
            RowSpec = GridLayout.InvokeSpec(ContentRow, 1f),
            Height = 0,
        };
        _contentLayoutParams.SetGravity(GravityFlags.Fill);

        view.Tag = ContentRow;

        _sheetContainer.AddView(view, _contentLayoutParams);

        base.SetContentView(_sheetContainer);
    }

    public void SetHeaderView(View view)
    {
        RemoveHeader();

        _headerLayoutParams = new GridLayout.LayoutParams()
        {
            TopMargin = Convert.ToInt32(Context.ToPixels(5)),
            RowSpec = GridLayout.InvokeSpec(HeaderRow),
        };
        _headerLayoutParams.SetGravity(GravityFlags.Fill);

        view.Tag = HeaderRow;

        _sheetContainer.AddView(view, _headerLayoutParams);
    }

    public void RemoveHeader()
    {
        RemoveRow(HeaderRow);
    }

    public override void SetCancelable(bool flag)
    {
        _isCancelable = flag;

        base.SetCancelable(flag);

        SetCanceledOnTouchOutside(_isCancelable);
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
        _handleLayoutParams?.Dispose();
        _headerLayoutParams?.Dispose();
        _contentLayoutParams?.Dispose();
        _touchOutsideListener.Dispose();
        _backgroundColorDrawable.Dispose();
        _bottomSheetHandle?.Dispose();
        _sheetContainer.Dispose();
    }

    private void RemoveRow(int row)
    {
        for (int i = 0; i < _sheetContainer.ChildCount; i++)
        {
            View? child = _sheetContainer.GetChildAt(i);

            if (child is not null
                && child.Tag is Java.Lang.Integer intTag
                && intTag.IntValue() == row)
            {
                child.RemoveFromParent();
                child.LayoutParameters?.Dispose();

                if (child is ViewGroup viewGroup)
                {
                    viewGroup.RemoveAllViews();
                }
            }
        }
    }

    private void BottomSheetDialog_ShowEvent(object? sender, EventArgs e)
    {
        if (_sheetContainer.Parent is View parent
            && parent.LayoutParameters is not null)
        {
            parent.LayoutParameters.Height = ViewGroup.LayoutParams.MatchParent;
        }

        _sheetContainer.AddOnLayoutChangeListener(_layoutChangeListener);
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
            State = e.State;
        }
        else
        {
            _eventManager.RaiseEvent(this, e, nameof(StateChanged));
        }
    }
}