using AndroidX.CoordinatorLayout.Widget;
using AndroidX.Core.View;
using AsyncAwaitBestPractices;
using Google.Android.Material.Shape;

namespace Plugin.BottomSheet.Android;

/// <summary>
/// Defines a bottom sheet dialog designed to display content in an overlay that slides up from the bottom of the screen.
/// </summary>
public sealed class BottomSheetDialog : Google.Android.Material.BottomSheet.BottomSheetDialog
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
    /// Provides a customizable modal bottom sheet dialog that overlays content on top of the application's main interface.
    /// </summary>
    /// <param name="context">The context the view is running in, through which it can access the current theme, resources, etc.</param>
    /// <param name="theme">Theme id.</param>
    /// <exception cref="NotSupportedException">Thrown when the <paramref name="context"/> is not of type <see cref="AndroidX.AppCompat.App.AppCompatActivity"/>.</exception>
    public BottomSheetDialog(Context context, int theme)
        : base(context, theme)
    {
        _context = context;
        _bottomSheetCallback = new BottomSheetCallback();
        _bottomSheetCallback.StateChanged += BottomSheetCallback_StateChanged;
        Behavior.AddBottomSheetCallback(_bottomSheetCallback);

        Behavior.GestureInsetBottomIgnored = true;

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
    /// Indicates whether the layout has been modified or updated, typically as a result of user interaction or dynamic changes.
    /// </summary>
    public event EventHandler LayoutChanged
    {
        add => _eventManager.AddEventHandler(value);
        remove => _eventManager.RemoveEventHandler(value);
    }

    /// <summary>
    /// Indicates whether the back button has been pressed in the current context or operation.
    /// </summary>
    public event EventHandler BackPressed
    {
        add => _eventManager.AddEventHandler(value);
        remove => _eventManager.RemoveEventHandler(value);
    }

    /// <summary>
    /// Indicates whether the state of an object, system, or process has been modified.
    /// </summary>
    public event EventHandler<BottomSheetStateChangedEventArgs> StateChanged
    {
        add => _eventManager.AddEventHandler(value);
        remove => _eventManager.RemoveEventHandler(value);
    }

    /// <summary>
    /// Indicates whether the current operation or process has been canceled.
    /// </summary>
    public event EventHandler Canceled
    {
        add => _eventManager.AddEventHandler(value);
        remove => _eventManager.RemoveEventHandler(value);
    }

    /// <summary>
    /// Gets or sets the space between the content of an element and its boundary.
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

    /// <summary>
    /// Gets or sets the radius of the corners of a rectangular element, defining the degree of rounding applied to its edges.
    /// </summary>
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
    /// Gets or sets the background color used for rendering or display purposes.
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

    /// <summary>
    /// Gets or sets the background color of the window, defining the visual appearance of its background area.
    /// </summary>
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
    /// Gets a structural or logical boundary used to encapsulate or organize elements within a system or application.
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

    /// <summary>
    /// Gets or sets a value indicating whether the current window or dialog is displayed as a modal,
    /// preventing interaction with other windows until closed.
    /// </summary>
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

    /// <summary>
    /// Gets or sets the current status or condition of an object, process, or system.
    /// </summary>
    public BottomSheetState State
    {
        get => Behavior.State.ToBottomSheetState();
        set => Behavior.State = value.ToPlatformState();
    }

    /// <summary>
    /// Gets or sets the collection of states associated with the application, process, or workflow.
    /// </summary>
    public IList<BottomSheetState> States
    {
        get => _bottomSheetStates;
        set
        {
            _bottomSheetStates = new List<BottomSheetState>(value);

            if (_bottomSheetStates.Contains(BottomSheetState.Peek) == false)
            {
                Behavior.SkipCollapsed = true;
            }
        }
    }

    /// <summary>
    /// Gets or sets the ratio between the fully expanded and collapsed states
    /// of a component or view when it is partially expanded.
    /// </summary>
    public float HalfExpandedRatio
    {
        get => Behavior.HalfExpandedRatio;
        set => Behavior.HalfExpandedRatio = value;
    }

    /// <summary>
    /// Gets or sets the maximum allowable height for a given element or component.
    /// </summary>
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

    /// <summary>
    /// Gets or sets the maximum allowable width for an element or component.
    /// </summary>
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

    /// <summary>
    /// Gets or sets a value indicating whether the element can be dragged interactively using a pointer device.
    /// </summary>
    public bool Draggable
    {
        get => Behavior.Draggable;
        set => Behavior.Draggable = value;
    }

    /// <summary>
    /// Gets or sets the space surrounding a content area, often used for layout and spacing purposes.
    /// </summary>
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

    /// <summary>
    /// Gets or sets the height at which the component is partially visible or "peeked" when it is in its collapsed state.
    /// </summary>
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

            if (Window is not null)
            {
                WindowInsetsCompat? insets = ViewCompat.GetRootWindowInsets(Window.DecorView);
                AndroidX.Core.Graphics.Insets? systemBarInsets = insets?.GetInsets(WindowInsetsCompat.Type.SystemBars());

                if (systemBarInsets is not null)
                {
                    height += systemBarInsets.Bottom;
                }

                if (height > Window.DecorView.Height)
                {
                    height = Window.DecorView.Height;

                    if (systemBarInsets is not null)
                    {
                        height -= systemBarInsets.Top;
                    }
                }
            }

            Behavior.PeekHeight = Convert.ToInt32(height);
        }
    }

    public BottomSheetSizeMode SizeMode
    {
        get => Behavior.FitToContents == true ? BottomSheetSizeMode.FitToContent : BottomSheetSizeMode.States;
        set
        {
            Behavior.FitToContents = value == BottomSheetSizeMode.FitToContent ? true : false;

            if (_content?.Parent is View parent
                && parent.LayoutParameters is not null)
            {
                parent.LayoutParameters.Height = Behavior.FitToContents ? ViewGroup.LayoutParams.WrapContent : ViewGroup.LayoutParams.MatchParent;
            }
        }
    }

    /// <summary>
    /// Displays the bottom sheet or to the user.
    /// </summary>
    public override void Show()
    {
        ShowEvent += BottomSheetDialog_ShowEvent;

        Window?.SetBackgroundDrawable(_backgroundColorDrawable);

        base.Show();
    }

    /// <summary>
    /// Asynchronously displays the bottom sheet or to the user.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation, containing the result of the display action.</returns>
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
    /// Cancels the bottom sheet.
    /// </summary>
    public override void Cancel()
    {
        _eventManager.RaiseEvent(this, EventArgs.Empty, nameof(Canceled));
    }

    /// <summary>
    /// Dismisses the currently displayed modal or dialog, removing it from view.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation, containing the result of the dismiss action.</returns>
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

    /// <summary>
    /// Sets the view to display as the content of the bottom sheet.
    /// </summary>
    /// <param name="view">The view to be used as the content of the bottom sheet.</param>
    public override void SetContentView(View view)
    {
        _content = view;

        base.SetContentView(view);
    }

    /// <summary>
    /// Indicates whether the user can cancel the dialog.
    /// </summary>
    /// <param name="flag">A boolean value indicating whether the user can cancel the dialog.</param>
    public override void SetCancelable(bool flag)
    {
        _isCancelable = flag;

        base.SetCancelable(flag);

        SetCanceledOnTouchOutside(_isCancelable);
    }

    /// <summary>
    /// Called when the view is attached to a window, allowing for initialization logic or resource setup specific to the window context.
    /// </summary>
    public override void OnAttachedToWindow()
    {
        if (_context is AndroidX.AppCompat.App.AppCompatActivity activity
            && activity.Window is not null
            && Window is not null
            && WindowCompat.GetInsetsController(activity.Window, activity.Window.DecorView) is WindowInsetsControllerCompat parentInsetsController
            && WindowCompat.GetInsetsController(Window, Window.DecorView) is WindowInsetsControllerCompat insetsController)
        {
            insetsController.AppearanceLightStatusBars = parentInsetsController.AppearanceLightStatusBars;
        }

        base.OnAttachedToWindow();
    }

    /// <summary>
    /// Releases all resources used by the current instance of the class.
    /// </summary>
    /// <param name="disposing">A boolean value indicating whether managed resources should be disposed. If true, both managed and unmanaged resources are released; otherwise, only unmanaged resources are released.</param>
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

    /// <summary>
    /// Handles the event triggered when the BottomSheet dialog is shown.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">An EventArgs object containing the event data.</param>
    private void BottomSheetDialog_ShowEvent(object? sender, EventArgs e)
    {
        Window?.ClearFlags(WindowManagerFlags.DimBehind);

        if (_content?.Parent is View parent
            && parent.LayoutParameters is not null
            && SizeMode == BottomSheetSizeMode.States)
        {
            parent.LayoutParameters.Height = ViewGroup.LayoutParams.MatchParent;
        }

        _content?.AddOnLayoutChangeListener(_layoutChangeListener);
        OnBackPressedDispatcher.AddCallback(_onBackPressedCallback);
    }

    /// <summary>
    /// Handles the event triggered when the back button is pressed while the bottom sheet dialog is active.
    /// </summary>
    /// <param name="sender">The object that raised the event.</param>
    /// <param name="e">An object that contains the event data.</param>
    private void OnBackPressedCallbackOnBackPressed(object? sender, EventArgs e)
    {
        if (_isCancelable)
        {
            Cancel();
        }

        _eventManager.RaiseEvent(this, EventArgs.Empty, nameof(BackPressed));
    }

    /// <summary>
    /// Handles the layout change events for the bottom sheet container.
    /// </summary>
    /// <param name="sender">The source of the event triggering the layout change.</param>
    /// <param name="e">The event data containing details about the layout change.</param>
    private void LayoutChangeListener_LayoutChange(object? sender, EventArgs e)
    {
        _eventManager.RaiseEvent(this, EventArgs.Empty, nameof(LayoutChanged));
    }

    /// <summary>
    /// Handles the event triggered when the state of the bottom sheet changes.
    /// </summary>
    /// <param name="sender">The source of the event, typically the bottom sheet callback.</param>
    /// <param name="e">An instance of <see cref="BottomSheetStateChangedEventArgs"/> containing details about the new state of the bottom sheet.</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S6608:Prefer indexing instead of \"Enumerable\" methods on types implementing \"IList\"", Justification = "Improve readability and no performance impact")]
    private void BottomSheetCallback_StateChanged(object? sender, BottomSheetStateChangedEventArgs e)
    {
        if (States.IsStateAllowed(e.NewState) == false)
        {
            State = States.First();
        }
        else
        {
            _eventManager.RaiseEvent(this, e, nameof(StateChanged));
        }
    }
}