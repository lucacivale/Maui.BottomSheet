using AsyncAwaitBestPractices;
using System.Diagnostics.CodeAnalysis;

namespace Plugin.BottomSheet.IOSMacCatalyst;

/// <summary>
/// Container for bottom sheet functionality including handle, header and content on macOS and iOS platforms.
/// </summary>
internal sealed class BottomSheet : UINavigationController
{
    private const string PeekDetentId = "Plugin.Maui.BottomSheet.PeekDetentId";

    private readonly WeakEventManager _eventManager = new();

    private readonly UISheetPresentationControllerDetent _peekDetent;
    private readonly UISheetPresentationControllerDetent _mediumDetent = UISheetPresentationControllerDetent.CreateMediumDetent();
    private readonly UISheetPresentationControllerDetent _largeDetent = UISheetPresentationControllerDetent.CreateLargeDetent();

    private readonly BottomSheetDelegate _bottomSheetDelegate = new();

    private double _peekHeight;
    private bool _isModal;

    private UIColor? _windowBackgroundColor;
    private UIColor? _backgroundColor;

    private BottomSheetContainerViewController? _containerViewController;

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
#pragma warning disable CA1416
            _peekDetent = UISheetPresentationControllerDetent.Create(PeekDetentId, PeekHeightValue);
#pragma warning restore CA1416
        }
        else
        {
            _peekDetent = UISheetPresentationControllerDetent.CreateMediumDetent();
        }
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
    /// Occurs when the bottom sheet frame changes.
    /// </summary>
    public event EventHandler<Rect> FrameChanged
    {
        add => _eventManager.AddEventHandler(value);
        remove => _eventManager.RemoveEventHandler(value);
    }

    /// <summary>
    /// Occurs when the bottom sheet frame changes.
    /// </summary>
    public event EventHandler LayoutChanged
    {
        add => _eventManager.AddEventHandler(value);
        remove => _eventManager.RemoveEventHandler(value);
    }

    public double PeekHeight
    {
        get => _peekHeight;
        set
        {
            _peekHeight = value;

            if ((OperatingSystem.IsMacCatalyst()
                || (OperatingSystem.IsIOS()
                    && OperatingSystem.IsIOSVersionAtLeast(16)))
#pragma warning disable CA1416
#pragma warning disable S6605
                && SheetPresentationController?.Detents.Any(x => x.Identifier == PeekDetentId) == true)
#pragma warning restore S6605
            {
                SheetPresentationController?.InvalidateDetents();
#pragma warning restore CA1416
            }
        }
    }

    public UIColor? WindowBackgroundColor
    {
        get => _windowBackgroundColor;
        set
        {
            _windowBackgroundColor = value;
            ApplyWindowBackgroundColor();
        }
    }

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
    /// Gets or sets a value indicating whether the bottom sheet is draggable.
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
    /// Gets or sets a value indicating whether the bottom sheet is modal.
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

    public BottomSheetState State
    {
        get => SheetPresentationController?.SelectedDetentIdentifier.ToBottomSheetState() ?? BottomSheetState.Peek;
        set
        {
            if (SheetPresentationController is not null)
            {
                SheetPresentationController.AnimateChanges(() =>
                {
                    SheetPresentationController.SelectedDetentIdentifier = value.ToPlatformState();
                });
            }
        }
    }

    /// <summary>
    /// Gets or sets the available detents for the bottom sheet.
    /// </summary>
    public ICollection<BottomSheetState> States
    {
        get
        {
            if (SheetPresentationController is null)
            {
                return [];
            }

            return SheetPresentationController.Detents.ToBottomSheetStates();
        }

        set
        {
            if (SheetPresentationController is null)
            {
                return;
            }

            SheetPresentationController.AnimateChanges(() =>
            {
                UISheetPresentationControllerDetent[] detents = value
                    .Select(x =>
                    {
                        UISheetPresentationControllerDetent detent = _largeDetent;

                        if (x == BottomSheetState.Peek)
                        {
                            detent = _peekDetent;
                        }
                        else if (x == BottomSheetState.Medium)
                        {
                            detent = _mediumDetent;
                        }

                        return detent;
                    })
                    .ToArray();

                SheetPresentationController.Detents = detents;
            });
        }
    }

    /// <summary>
    /// Gets a value indicating whether the bottom sheet is currently being presented.
    /// </summary>
    public bool IsOpen { get; private set; }

    /// <summary>
    /// Gets the frame rectangle of the bottom sheet in pixels.
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

    public override void ViewWillAppear(bool animated)
    {
        base.ViewWillAppear(animated);

        ApplyBackgroundColor();
        ApplyWindowBackgroundColor();
    }

    public override void ViewWillDisappear(bool animated)
    {
        base.ViewWillDisappear(animated);

        WindowBackgroundColor = UIColor.Clear;
        ApplyWindowBackgroundColor();
    }

    public override void ViewDidLayoutSubviews()
    {
        base.ViewDidLayoutSubviews();

        _eventManager.RaiseEvent(
            this,
            EventArgs.Empty,
            nameof(LayoutChanged));

        _eventManager.RaiseEvent(
            this,
            new Rect(
                Frame.X,
                Frame.Y,
                Frame.Width,
                Frame.Height),
            nameof(FrameChanged));
    }

    public void SetContentView(UIView view)
    {
        _containerViewController = new BottomSheetContainerViewController(view);

        SetViewControllers([_containerViewController], false);
    }

    /// <summary>
    /// Opens the bottom sheet with the specified configuration.
    /// </summary>
    /// <param name="bottomSheet">The virtual view containing bottom sheet configuration.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
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
    /// Cancels the bottom sheet dialog.
    /// </summary>
    public void Cancel()
    {
        _eventManager.RaiseEvent(this, EventArgs.Empty, nameof(Canceled));
    }

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

        _bottomSheetDelegate.StateChanged -= BottomSheetDelegateOnStateChanged;
        _bottomSheetDelegate.ConfirmDismiss -= BottomSheetDelegateOnConfirmDismiss;
        _bottomSheetDelegate.Dispose();

        if (IsOpen)
        {
            _containerViewController?.Dispose();
        }
    }

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

    private void ApplyWindowBackgroundColor()
    {
        // ReSharper disable once ConditionalAccessQualifierIsNonNullableAccordingToAPIContract
        if (PresentationController?.ContainerView?.Subviews.Length > 0)
        {
            UIView.Animate(0.25, () => PresentationController.ContainerView.Subviews[0].BackgroundColor = IsModal ? WindowBackgroundColor : UIColor.Clear);
        }
    }

    private void ApplyBackgroundColor()
    {
        if (View is not null)
        {
            View.BackgroundColor = BackgroundColor;
        }
    }

    /// <summary>
    /// Calculates the peek height based on container context and constraints.
    /// </summary>
    /// <param name="arg">The detent resolution context.</param>
    /// <returns>The calculated peek height as a native float.</returns>
    [SuppressMessage("Major Bug", "S1244:Floating point numbers should not be tested for equality", Justification = "False positive")]
    private nfloat PeekHeightValue(IUISheetPresentationControllerDetentResolutionContext arg)
    {
        nfloat peekHeight = new(_peekHeight);

        if (View?.Window is not null
            && peekHeight > 0)
        {
            peekHeight += View.Window.SafeAreaInsets.Bottom;
        }

        if (OperatingSystem.IsMacCatalyst()
            || (OperatingSystem.IsIOS()
                && OperatingSystem.IsIOSVersionAtLeast(16)))
        {
#pragma warning disable CA1416
            return peekHeight <= arg.MaximumDetentValue ? new nfloat(peekHeight) : arg.MaximumDetentValue;
#pragma warning restore CA1416
        }
        else
        {
            return new nfloat(peekHeight);
        }
    }

    /// <summary>
    /// Handles state change events from the delegate and forwards them.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The state change event arguments.</param>
    private void BottomSheetDelegateOnStateChanged(object? sender, BottomSheetStateChangedEventArgs e)
    {
        _eventManager.RaiseEvent(
            this,
            e,
            nameof(StateChanged));
    }

    /// <summary>
    /// Handles confirm dismiss events from the delegate and forwards them.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    private void BottomSheetDelegateOnConfirmDismiss(object? sender, EventArgs e)
    {
        _eventManager.RaiseEvent(
            this,
            e,
            nameof(Canceled));
    }
}