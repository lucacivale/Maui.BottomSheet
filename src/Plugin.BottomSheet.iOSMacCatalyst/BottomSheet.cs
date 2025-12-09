using AsyncAwaitBestPractices;
using System.Diagnostics.CodeAnalysis;

namespace Plugin.BottomSheet.iOSMacCatalyst;

/// <summary>
/// Represents a UI component that provides bottom sheet functionality, typically used to display additional contextual content or user actions on supported platforms.
/// </summary>
public sealed class BottomSheet : UINavigationController, IEnumerable<UIView>
{
    private const string AccessibilityIdentifier = "Plugin.BottomSheet.iOSMacCatalyst.BottomSheet";
    private const string PeekDetentId = "Plugin.Maui.BottomSheet.PeekDetentId";

    private readonly WeakEventManager _eventManager = new();
    private readonly UISheetPresentationControllerDetent _contentDetent;
    private readonly UISheetPresentationControllerDetent _peekDetent;
    private readonly UISheetPresentationControllerDetent _mediumDetent = UISheetPresentationControllerDetent.CreateMediumDetent();
    private readonly UISheetPresentationControllerDetent _largeDetent = UISheetPresentationControllerDetent.CreateLargeDetent();
    private readonly BottomSheetDelegate _bottomSheetDelegate = new();

    private double _peekHeight;
    private bool _isModal;

    private UIColor? _windowBackgroundColor;
    private UIColor? _backgroundColor;

    private BottomSheetContainerViewController? _containerViewController;

    private BottomSheetSizeMode _sizeMode;

    /// <summary>
    /// Initializes a new instance of the <see cref="BottomSheet"/> class.
    /// Represents a bottom sheet component that provides a configurable and interactive user interface element.
    /// </summary>
    [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "Is validated.")]
    public BottomSheet()
    {
        SetToolbarHidden(true, false);
        SetNavigationBarHidden(true, false);

        _bottomSheetDelegate.StateChanged += BottomSheetDelegateOnStateChanged;
        _bottomSheetDelegate.ConfirmDismiss += BottomSheetDelegateOnConfirmDismiss;

        if (OperatingSystem.IsMacCatalyst()
            || (OperatingSystem.IsIOS()
                && OperatingSystem.IsIOSVersionAtLeast(16)))
        {
            _peekDetent = UISheetPresentationControllerDetent.Create(PeekDetentId, PeekHeightValue);
            _contentDetent = UISheetPresentationControllerDetent.Create(PeekDetentId, ContentHeightValue);
        }
        else
        {
            _peekDetent = UISheetPresentationControllerDetent.CreateMediumDetent();
            _contentDetent = UISheetPresentationControllerDetent.CreateMediumDetent();
        }
    }

    /// <summary>
    /// Triggered when the state of the object undergoes a change.
    /// </summary>
    public event EventHandler<BottomSheetStateChangedEventArgs> StateChanged
    {
        add => _eventManager.AddEventHandler(value);
        remove => _eventManager.RemoveEventHandler(value);
    }

    /// <summary>
    /// Indicates whether an operation or process has been canceled.
    /// </summary>
    public event EventHandler Canceled
    {
        add => _eventManager.AddEventHandler(value);
        remove => _eventManager.RemoveEventHandler(value);
    }

    /// <summary>
    /// Triggered when the frame content is updated or modified.
    /// </summary>
    public event EventHandler FrameChanged
    {
        add => _eventManager.AddEventHandler(value);
        remove => _eventManager.RemoveEventHandler(value);
    }

    /// <summary>
    /// Triggered when the layout of the user interface is modified.
    /// </summary>
    public event EventHandler LayoutChanged
    {
        add => _eventManager.AddEventHandler(value);
        remove => _eventManager.RemoveEventHandler(value);
    }

    /// <summary>
    /// Gets or sets the height at which the bottom sheet is partially expanded.
    /// </summary>
    [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "Is validated.")]
    public double PeekHeight
    {
        get => _peekHeight;
        set
        {
            _peekHeight = value;

            if ((OperatingSystem.IsMacCatalyst()
                || (OperatingSystem.IsIOS()
                    && OperatingSystem.IsIOSVersionAtLeast(16)))
                && SheetPresentationController?.Detents.Any(x => x.Identifier == PeekDetentId) == true)
            {
                SheetPresentationController?.InvalidateDetents();
            }
        }
    }

    /// <summary>
    /// Gets or sets the background color of the window.
    /// </summary>
    public UIColor? WindowBackgroundColor
    {
        get => _windowBackgroundColor;
        set
        {
            _windowBackgroundColor = value;
            ApplyWindowBackgroundColor();
        }
    }

    /// <summary>
    /// Gets or sets the background color of the component.
    /// </summary>
    public UIColor? BackgroundColor
    {
        get => _backgroundColor;
        set
        {
            _backgroundColor = value;
            ApplyBackgroundColor();
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the bottom sheet can be dragged.
    /// </summary>
    public bool Draggable
    {
        // ReSharper disable once ConditionalAccessQualifierIsNonNullableAccordingToAPIContract
        get => PresentationController?.PresentedView?.GestureRecognizers?[0] is not null
            && PresentationController.PresentedView.GestureRecognizers[0].Enabled;
        set
        {
            // ReSharper disable once ConditionalAccessQualifierIsNonNullableAccordingToAPIContract
            if (PresentationController?.PresentedView?.GestureRecognizers?[0] is not null)
            {
                PresentationController.PresentedView.GestureRecognizers[0].Enabled = value;
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the component is displayed as a modal dialog.
    /// </summary>
    [SuppressMessage("Minor Code Smell", "S6608:Prefer indexing instead of \"Enumerable\" methods on types implementing \"IList\"", Justification = "Improves readability and has no performance impact.")]
    public bool IsModal
    {
        get => _isModal;
        set
        {
            _isModal = value;

            if (SheetPresentationController is not null)
            {
                // Actually bug in UIKit. Custom detent can't be undimmed. Submit a bug?.
                SheetPresentationController.LargestUndimmedDetentIdentifier = _isModal ? UISheetPresentationControllerDetentIdentifier.Unknown : States.Last().ToPlatformState();
            }

            ApplyWindowBackgroundColor();
        }
    }

    /// <summary>
    /// Gets or sets the current state of the bottom sheet.
    /// </summary>
    public BottomSheetState State
    {
        get => SheetPresentationController?.SelectedDetentIdentifier.ToBottomSheetState() ?? BottomSheetState.Peek;
        set
        {
            SheetPresentationController?.AnimateChanges(() =>
            {
                SheetPresentationController.SelectedDetentIdentifier = value.ToPlatformState();
            });
        }
    }

    /// <summary>
    /// Gets or sets the collection of states associated with the application, process, or workflow.
    /// </summary>
    public ICollection<BottomSheetState> States
    {
        get
        {
            return SheetPresentationController is null ? [] : SheetPresentationController.Detents.ToBottomSheetStates();
        }

        set
        {
            SheetPresentationController?.AnimateChanges(() =>
            {
                UISheetPresentationControllerDetent[] detents = value
                    .Select(x =>
                    {
                        UISheetPresentationControllerDetent detent = x switch
                        {
                            BottomSheetState.Peek => _peekDetent,
                            BottomSheetState.Medium => _mediumDetent,
                            _ => _largeDetent,
                        };

                        return detent;
                    })
                    .ToArray();

                SheetPresentationController.Detents = detents;
            });
        }
    }

    /// <summary>
    /// Gets a value indicating whether the bottom sheet is currently displayed.
    /// </summary>
    public bool IsOpen { get; private set; }

    /// <summary>
    /// Gets the visible rectangular region or boundary within which UI elements are displayed or interact.
    /// </summary>
    public Rect Frame
    {
        get
        {
            if (PresentationController is null)
            {
                return new(0, 0, 0, 0);
            }

            CGRect frame = PresentationController.FrameOfPresentedViewInContainerView;
            frame.Y -= PresentationController.PresentedView.LayoutMargins.Top;

            return new Rect(frame.X, frame.Y, frame.Width, frame.Height);
        }
    }

    /// <summary>
    /// Gets or sets the sizing behavior of the bottom sheet, determining whether it resizes
    /// to fit its content or adheres to predefined states.
    /// </summary>
    [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "Is validated.")]
    public BottomSheetSizeMode SizeMode
    {
        get => _sizeMode;
        set
        {
            _sizeMode = value;

            UISheetPresentationControllerDetent[] detents = _sizeMode == BottomSheetSizeMode.FitToContent ? [_contentDetent] : [_largeDetent];

            SheetPresentationController?.AnimateChanges(() =>
            {
                SheetPresentationController.Detents = detents;
            });
        }
    }

    /// <summary>
    /// Gets or sets a custom calculation for determining the height of the BottomSheet
    /// when the SizeMode is set to FitToContent. Allows users to provide a function
    /// that calculates the desired content height based on specific requirements.
    /// </summary>
    public Func<double>? FitToContentCalculation { get; set; }

    /// <summary>
    /// Returns an enumerator that iterates through the collection of subviews.
    /// </summary>
    /// <returns>An enumerator for the collection of <see cref="UIView"/> objects.</returns>
    IEnumerator<UIView> IEnumerable<UIView>.GetEnumerator()
        => (IEnumerator<UIView>?)_containerViewController?.GetEnumerator() ?? Enumerable.Empty<UIView>().GetEnumerator();

    /// <summary>
    /// Called after the controller's view is loaded into memory.
    /// Configures the view by setting its accessibility identifier for better UI automation support.
    /// </summary>
    public override void ViewDidLoad()
    {
        base.ViewDidLoad();

        if (View is not null)
        {
            View.AccessibilityIdentifier = AccessibilityIdentifier;
        }
    }

    /// <summary>
    /// Invoked when the view is about to appear on the screen.
    /// </summary>
    /// <param name="animated">A boolean value indicating whether the appearance of the view should be animated.</param>
    public override void ViewWillAppear(bool animated)
    {
        base.ViewWillAppear(animated);

        ApplyBackgroundColor();
        ApplyWindowBackgroundColor();
    }

    /// <summary>
    /// Called to notify that the view is about to disappear from the screen.
    /// </summary>
    /// <param name="animated">Indicates whether the disappearance of the view is being animated.</param>
    public override void ViewWillDisappear(bool animated)
    {
        base.ViewWillDisappear(animated);

        WindowBackgroundColor = UIColor.Clear;
        ApplyWindowBackgroundColor();
    }

    /// <summary>
    /// Notifies the view controller that its view has just laid out its subviews.
    /// </summary>
    [SuppressMessage("Major Code Smell", "S1066:Mergeable \"if\" statements should be combined", Justification = "Improve readability.")]
    [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "Is validated.")]
    public override void ViewDidLayoutSubviews()
    {
        base.ViewDidLayoutSubviews();

        if (_sizeMode == BottomSheetSizeMode.FitToContent)
        {
            if (OperatingSystem.IsMacCatalyst()
                || (OperatingSystem.IsIOS()
                    && OperatingSystem.IsIOSVersionAtLeast(16)))
            {
                SheetPresentationController?.InvalidateDetents();
            }
        }

        _eventManager.RaiseEvent(
            this,
            EventArgs.Empty,
            nameof(LayoutChanged));

        _eventManager.RaiseEvent(
            this,
            EventArgs.Empty,
            nameof(FrameChanged));
    }

    /// <summary>
    /// Sets the content view for the activity to the specified layout resource.
    /// </summary>
    /// <param name="view">The view to be used as the content of the bottom sheet.</param>
    public void SetContentView(UIView view)
    {
        _containerViewController = new BottomSheetContainerViewController(view);

        SetViewControllers([_containerViewController], false);
    }

    /// <summary>
    /// Asynchronously opens the bottom sheet on the specified window.
    /// </summary>
    /// <param name="window">The window to which the bottom sheet is attached.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task OpenAsync(UIWindow? window)
    {
        InitSheetPresentationController();

        if (window?.CurrentViewController() is UIViewController parent)
        {
            await parent.PresentViewControllerAsync(this, true).ConfigureAwait(true);
        }

        IsOpen = ReferenceEquals(View?.Window?.CurrentViewController(), this);
    }

    /// <summary>
    /// Closes the bottom sheet asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task CloseAsync()
    {
        await DismissViewControllerAsync(true).ConfigureAwait(true);

        if (ViewControllers?.First(x => x is BottomSheetContainerViewController) is BottomSheetContainerViewController
            container)
        {
            container.RemoveFromParentViewController();
            container.Dispose();
        }

        IsOpen = false;
    }

    /// <summary>
    /// Cancels the bottom sheet.
    /// </summary>
    public void Cancel()
    {
        _eventManager.RaiseEvent(this, EventArgs.Empty, nameof(Canceled));
    }

    /// <summary>
    /// Releases the resources used by the current instance of the class.
    /// </summary>
    /// <param name="disposing">Indicates whether the method call comes from a Dispose method (its value is true) or from a finalizer (its value is false).</param>
    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (!disposing)
        {
            return;
        }

        _peekDetent.Dispose();
        _mediumDetent.Dispose();
        _largeDetent.Dispose();
        _contentDetent.Dispose();

        _bottomSheetDelegate.StateChanged -= BottomSheetDelegateOnStateChanged;
        _bottomSheetDelegate.ConfirmDismiss -= BottomSheetDelegateOnConfirmDismiss;
        _bottomSheetDelegate.Dispose();

        if (IsOpen)
        {
            _containerViewController?.Dispose();
        }
    }

    /// <summary>
    /// Initializes the sheet presentation controller by setting its delegate and
    /// configuring preferred edge attachment behavior for compact height.
    /// </summary>
    private void InitSheetPresentationController()
    {
        if (SheetPresentationController is null)
        {
            return;
        }

        if (!ReferenceEquals(SheetPresentationController.Delegate, _bottomSheetDelegate))
        {
            SheetPresentationController.Delegate = _bottomSheetDelegate;
        }

        SheetPresentationController.PrefersEdgeAttachedInCompactHeight = true;
    }

    /// <summary>
    /// Applies the specified color as the background color for the application window.
    /// </summary>
    private void ApplyWindowBackgroundColor()
    {
        // ReSharper disable once ConditionalAccessQualifierIsNonNullableAccordingToAPIContract
        if (PresentationController?.ContainerView?.Subviews.Length > 0)
        {
            UIView.Animate(0.25, () => PresentationController.ContainerView.Subviews[0].BackgroundColor = IsModal ? WindowBackgroundColor : UIColor.Clear);
        }
    }

    /// <summary>
    /// Applies the specified background color to the bottom sheet view.
    /// </summary>
    private void ApplyBackgroundColor()
    {
        if (View is not null)
        {
            View.BackgroundColor = BackgroundColor;
        }
    }

    /// <summary>
    /// Calculates the peek height based on the given detent resolution context and the view's constraints.
    /// </summary>
    /// <param name="arg">The context providing detent resolution constraints.</param>
    /// <returns>The calculated peek height as a native float.</returns>
    [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "Is validated.")]
    private nfloat PeekHeightValue(IUISheetPresentationControllerDetentResolutionContext arg)
    {
        double peekHeight = _peekHeight;

        if (View?.Window is not null
            && peekHeight > 0)
        {
            peekHeight += View.Window.SafeAreaInsets.Bottom;
        }

        if (OperatingSystem.IsMacCatalyst()
            || (OperatingSystem.IsIOS()
                && OperatingSystem.IsIOSVersionAtLeast(16)))
        {
            peekHeight = peekHeight <= arg.MaximumDetentValue ? peekHeight : arg.MaximumDetentValue;
        }

        return new(peekHeight);
    }

    /// <summary>
    /// Calculates the content height value for the bottom sheet based on the specified resolution context.
    /// Takes into account the safe area insets, platform compatibility, and maximum detent value constraints.
    /// </summary>
    /// <param name="arg">The context used to resolve detent properties, providing relevant detent configuration information.</param>
    /// <returns>The calculated content height as a <see cref="nfloat"/> value.</returns>
    [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "Is validated.")]
    private nfloat ContentHeightValue(IUISheetPresentationControllerDetentResolutionContext arg)
    {
        double height = FitToContentCalculation?.Invoke() ?? 0;

        if (View?.Window is not null
            && height > 0)
        {
            height += View.Window.SafeAreaInsets.Bottom;
        }

        if (OperatingSystem.IsMacCatalyst()
            || (OperatingSystem.IsIOS()
                && OperatingSystem.IsIOSVersionAtLeast(16)))
        {
            height = height <= arg.MaximumDetentValue ? height : arg.MaximumDetentValue;
        }

        return new nfloat(height);
    }

    /// <summary>
    /// Called when the state of the bottom sheet changes.
    /// </summary>
    /// <param name="sender">The object that raised the event.</param>
    /// <param name="e">The event arguments.</param>
    private void BottomSheetDelegateOnStateChanged(object? sender, BottomSheetStateChangedEventArgs e)
    {
        _eventManager.RaiseEvent(
            this,
            e,
            nameof(StateChanged));
    }

    /// <summary>
    /// Handles the confirmation logic when a bottom sheet is about to be dismissed.
    /// </summary>
    /// <param name="sender">The object that raised the event.</param>
    /// <param name="e">The event arguments.</param>
    private void BottomSheetDelegateOnConfirmDismiss(object? sender, EventArgs e)
    {
        _eventManager.RaiseEvent(
            this,
            e,
            nameof(Canceled));
    }
}