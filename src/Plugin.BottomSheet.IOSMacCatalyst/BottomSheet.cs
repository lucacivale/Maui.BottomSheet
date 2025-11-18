using AsyncAwaitBestPractices;
using System.Diagnostics.CodeAnalysis;

namespace Plugin.BottomSheet.IOSMacCatalyst;

/// <summary>
/// Container for bottom sheet functionality including handle, header and content on macOS and iOS platforms.
/// </summary>
internal sealed class BottomSheet : UIViewController, IUISheetPresentationControllerDelegate
{
    private const string PeekDetentId = "Plugin.Maui.BottomSheet.PeekDetentId";

    private readonly WeakEventManager _eventManager = new();

    private readonly UISheetPresentationControllerDetent _peekDetent;
    private readonly UISheetPresentationControllerDetent _mediumDetent = UISheetPresentationControllerDetent.CreateMediumDetent();
    private readonly UISheetPresentationControllerDetent _largeDetent = UISheetPresentationControllerDetent.CreateLargeDetent();

    private readonly BottomSheetDelegate _bottomSheetDelegate = new();

    private double _peekHeight;
    private UIColor? _windowBackgroundColor;
    private bool _isModal;

    public BottomSheet()
    {
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
        get => View?.BackgroundColor;
        set
        {
            if (View is not null)
            {
                View.BackgroundColor = value;
            }
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

    /// <summary>
    /// Called when the view layout is complete, handles frame change notifications.
    /// </summary>
    public override void ViewDidLayoutSubviews()
    {
        base.ViewDidLayoutSubviews();

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
        view.TranslatesAutoresizingMaskIntoConstraints = false;

        if (View is not null)
        {
            View.AddSubview(view);

            NSLayoutConstraint.ActivateConstraints(
            [
                view.TopAnchor.ConstraintEqualTo(View.SafeAreaLayoutGuide.TopAnchor),
                view.LeadingAnchor.ConstraintEqualTo(View.SafeAreaLayoutGuide.LeadingAnchor),
                view.TrailingAnchor.ConstraintEqualTo(View.SafeAreaLayoutGuide.TrailingAnchor),
                view.BottomAnchor.ConstraintEqualTo(View.SafeAreaLayoutGuide.BottomAnchor),
            ]);
        }
    }

    /// <summary>
    /// Opens the bottom sheet with the specified configuration.
    /// </summary>
    /// <param name="bottomSheet">The virtual view containing bottom sheet configuration.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task OpenAsync()
    {
        InitSheetPresentationController();

        if (WindowUtils.GetTopViewController() is UIViewController parent)
        {
            await parent.PresentViewControllerAsync(this, true).ConfigureAwait(true);
        }

        IsOpen = ReferenceEquals(WindowUtils.GetTopViewController(), this);
    }

    /// <summary>
    /// Closes the bottom sheet asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task CloseAsync()
    {
        await DismissViewControllerAsync(true).ConfigureAwait(true);

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

    /// <summary>
    /// Calculates the peek height based on container context and constraints.
    /// </summary>
    /// <param name="arg">The detent resolution context.</param>
    /// <returns>The calculated peek height as a native float.</returns>
    [SuppressMessage("Major Bug", "S1244:Floating point numbers should not be tested for equality", Justification = "False positive")]
    private nfloat PeekHeightValue(IUISheetPresentationControllerDetentResolutionContext arg)
    {
        if (OperatingSystem.IsMacCatalyst()
            || (OperatingSystem.IsIOS()
                && OperatingSystem.IsIOSVersionAtLeast(16)))
        {
#pragma warning disable CA1416
            return _peekHeight <= arg.MaximumDetentValue ? new nfloat(_peekHeight) : arg.MaximumDetentValue;
#pragma warning restore CA1416
        }
        else
        {
            return new nfloat(_peekHeight);
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