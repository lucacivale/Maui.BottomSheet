namespace Plugin.Maui.BottomSheet.Platform.MaciOS;

using System.Diagnostics.CodeAnalysis;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Platform;
using UIKit;

/// <summary>
/// BottomSheet controller.
/// </summary>
internal sealed class BottomSheetUIViewController : UINavigationController
{
    /// <summary>
    /// Peek detent Id.
    /// </summary>
    public const string PeekDetentId = "Plugin.Maui.BottomSheet.PeekDetentId";

    private readonly WeakEventManager _eventManager = new();
    private readonly BottomSheetControllerDelegate _bottomSheetControllerDelegate = new();

    private readonly UIViewController _bottomSheetUIViewController;

    private readonly BottomSheetPage _virtualBottomSheet;
    private readonly Grid _virtualBottomSheetLayout;

    private UISheetPresentationControllerDetent? _peekDetent;
    private UISheetPresentationControllerDetent? _mediumDetent;
    private UISheetPresentationControllerDetent? _largeDetent;

    private double _peekDetentHeight;
    private double _peekHeight;

    private View? _virtualBottomSheetContent;

    private BottomSheetHeader? _bottomSheetHeader;

    private bool _isModal;

    private Color _windowBackgroundColor = Colors.Transparent;

    /// <summary>
    /// Initializes a new instance of the <see cref="BottomSheetUIViewController"/> class.
    /// </summary>
    /// <param name="mauiContext">Context.</param>
    public BottomSheetUIViewController(IMauiContext mauiContext)
    {
        _virtualBottomSheetLayout = new Grid()
        {
            RowDefinitions = [
                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Star)],
        };
        _virtualBottomSheet = new BottomSheetPage()
        {
            Content = _virtualBottomSheetLayout,
        };
        _bottomSheetUIViewController = _virtualBottomSheet.ToUIViewController(mauiContext);

        SetViewControllers([_bottomSheetUIViewController], true);
        SetNavigationBarHidden(true, true);

        _bottomSheetControllerDelegate.Dismissed += RaiseDismissed;
        _bottomSheetControllerDelegate.StateChanged += BottomSheetControllerDelegateOnStateChanged;
        _bottomSheetControllerDelegate.ConfirmDismiss += BottomSheetControllerDelegateOnConfirmDismiss;
    }

    /// <summary>
    /// Finalizes an instance of the <see cref="BottomSheetUIViewController"/> class.
    /// </summary>
    ~BottomSheetUIViewController()
    {
        Dispose(false);
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
    /// Confirm dismiss.
    /// </summary>
    public event EventHandler ConfirmDismiss
    {
        add => _eventManager.AddEventHandler(value);
        remove => _eventManager.RemoveEventHandler(value);
    }

    /// <summary>
    /// BottomSheet dismissed.
    /// </summary>
    public event EventHandler Dismissed
    {
        add => _eventManager.AddEventHandler(value);
        remove => _eventManager.RemoveEventHandler(value);
    }

    /// <summary>
    /// Gets peek detent.
    /// </summary>
    public UISheetPresentationControllerDetent PeekDetent
    {
        get
        {
            if (OperatingSystem.IsMacCatalyst()
                || (OperatingSystem.IsIOS()
                    && OperatingSystem.IsIOSVersionAtLeast(16)))
            {
#pragma warning disable CA1416
                _peekDetent = UISheetPresentationControllerDetent.Create(PeekDetentId, PeekHeight);
#pragma warning restore CA1416
            }
            else
            {
                _peekDetent = UISheetPresentationControllerDetent.CreateMediumDetent();
            }

            return _peekDetent;
        }
    }

    /// <summary>
    /// Gets medium detent.
    /// </summary>
    public UISheetPresentationControllerDetent MediumDetent => _mediumDetent ??= UISheetPresentationControllerDetent.CreateMediumDetent();

    /// <summary>
    /// Gets large detent.
    /// </summary>
    public UISheetPresentationControllerDetent LargeDetent => _largeDetent ??= UISheetPresentationControllerDetent.CreateLargeDetent();

    /// <summary>
    /// Gets or sets a value indicating whether sheet is draggable.
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
    /// Gets or sets a value indicating whether sheet is modal.
    /// </summary>
    public bool IsModal
    {
        get => _isModal;
        set
        {
            _isModal = value;

            if (SheetPresentationController is not null)
            {
                // Actually bug in UIKit. Custom detent can't be undimmed. Submit a bug?.
                SheetPresentationController.LargestUndimmedDetentIdentifier = _isModal ? UISheetPresentationControllerDetentIdentifier.Unknown : Detents.LargestDetentIdentifier();
            }

            ApplyWindowBackgroundColor();
        }
    }

    /// <summary>
    /// Gets or sets available detents.
    /// </summary>
    public UISheetPresentationControllerDetent[] Detents
    {
        get => SheetPresentationController is not null ? SheetPresentationController.Detents : [];
        set
        {
            if (SheetPresentationController is not null)
            {
                SheetPresentationController.AnimateChanges(() =>
                {
                    SheetPresentationController.Detents = value;
                });
            }
        }
    }

    /// <summary>
    /// Open the bottom sheet.
    /// </summary>
    /// <param name="bottomSheet"><see cref="IBottomSheet"/> to open.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task<bool> OpenAsync(IBottomSheet bottomSheet)
    {
        _virtualBottomSheet.Parent = bottomSheet.GetPageParent();
        _virtualBottomSheet.BottomSheet = bottomSheet;

        if (SheetPresentationController is not null)
        {
            SheetPresentationController.PrefersEdgeAttachedInCompactHeight = true;
        }

        if (WindowStateManager.Default.GetCurrentUIViewController() is UIViewController parent)
        {
            await parent.PresentViewControllerAsync(this, true).ConfigureAwait(true);
        }

        return WindowStateManager.Default.GetCurrentUIViewController() is BottomSheetUIViewController;
    }

    /// <summary>
    /// Close the bottom sheet.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task CloseAsync()
    {
        await DismissViewControllerAsync(true).ConfigureAwait(true);
        _virtualBottomSheetLayout.Clear();

#if NET9_0_OR_GREATER
        _virtualBottomSheetContent?.DisconnectHandlers();
        _virtualBottomSheetContent = null;
#else
        _virtualBottomSheetContent?.Handler?.DisconnectHandler();
        _virtualBottomSheetContent = null;
#endif
    }

    /// <inheritdoc/>
    public override void ViewIsAppearing(bool animated)
    {
        base.ViewIsAppearing(animated);

        ApplyWindowBackgroundColor();
    }

    [SuppressMessage("Usage", "ConditionalAccessQualifierIsNonNullableAccordingToAPIContract: ReSharper disable once ConditionalAccessQualifierIsNonNullableAccordingToAPIContract", Justification = "Can actually be null.")]
    public override void ViewWillDisappear(bool animated)
    {
        if (IsModal
            && PresentationController?.ContainerView?.Subviews.Length > 0)
        {
            UIView.Animate(0.5, () => PresentationController.ContainerView.Subviews[0].BackgroundColor = UIColor.Clear);
        }

        base.ViewWillDisappear(animated);
    }

    /// <summary>
    /// Attach presentation delegate to notify sheet dismiss and state changes.
    /// </summary>
    public void AttachPresentationDelegate()
    {
        if (SheetPresentationController is not null
            && !ReferenceEquals(SheetPresentationController.Delegate, _bottomSheetControllerDelegate))
        {
            SheetPresentationController.Delegate = _bottomSheetControllerDelegate;
        }
    }

    /// <summary>
    /// Show header.
    /// </summary>
    public void ShowHeader()
    {
        if (_bottomSheetHeader is null)
        {
            return;
        }

        _virtualBottomSheetLayout.Add(_bottomSheetHeader.View);
        _bottomSheetHeader.SizeChanged += BottomSheetHeaderOnSizeChanged;
        _bottomSheetHeader.CloseButtonClicked += RaiseDismissed;
    }

    /// <summary>
    /// Hide header.
    /// </summary>
    public void HideHeader()
    {
        if (_bottomSheetHeader is null)
        {
            return;
        }

        _bottomSheetHeader.Hide();
        _bottomSheetHeader.SizeChanged -= BottomSheetHeaderOnSizeChanged;
        _bottomSheetHeader.CloseButtonClicked -= RaiseDismissed;
        ApplyPeekHeight();
    }

    /// <summary>
    /// Adds the header view.
    /// </summary>
    /// <param name="header">Header.</param>
    /// <param name="style">Style.</param>
    public void AddHeader(Plugin.Maui.BottomSheet.BottomSheetHeader? header, BottomSheetHeaderStyle style)
    {
        if (header is null)
        {
            return;
        }

        _bottomSheetHeader = new BottomSheetHeader(header, style);
    }

    /// <summary>
    /// Set header style.
    /// </summary>
    /// <param name="style">Style.</param>
    public void SetHeaderStyle(BottomSheetHeaderStyle style)
    {
        if (_bottomSheetHeader is null)
        {
            return;
        }

        _bottomSheetHeader.SetStyle(style);
    }

    /// <summary>
    /// Adds the content view.
    /// </summary>
    /// <param name="content">Content.</param>
    public void AddContent(BottomSheetContent? content)
    {
        if (content is null
            || _bottomSheetUIViewController.View is null)
        {
            return;
        }

        _virtualBottomSheetContent = content.CreateContent();

        _virtualBottomSheetLayout.Add(
            _virtualBottomSheetContent,
            0,
            1);
    }

    /// <summary>
    /// Set current detent identifier.
    /// </summary>
    /// <param name="identifier">Identifier.</param>
    public void SetSelectedDetentIdentifier(UISheetPresentationControllerDetentIdentifier identifier)
    {
        if (SheetPresentationController is not null)
        {
            SheetPresentationController.AnimateChanges(() =>
            {
                SheetPresentationController.SelectedDetentIdentifier = identifier;
            });
        }
    }

    /// <summary>
    /// Sets whether grabber is visible.
    /// </summary>
    /// <param name="visible">Is visible.</param>
    public void SetPrefersGrabberVisible(bool visible)
    {
        if (SheetPresentationController is not null)
        {
            SheetPresentationController.PrefersGrabberVisible = visible;
        }
    }

    /// <summary>
    /// Sets corner radius.
    /// </summary>
    /// <param name="cornerRadius">Corner radius.</param>
    public void SetCornerRadius(float cornerRadius)
    {
        if (SheetPresentationController is not null)
        {
            SheetPresentationController.PreferredCornerRadius = cornerRadius;
        }
    }

    /// <summary>
    /// Sets window background color.
    /// </summary>
    /// <param name="color">Color.</param>
    public void SetWindowBackgroundColor(Color color)
    {
        _windowBackgroundColor = color;
        ApplyWindowBackgroundColor();
    }

    /// <summary>
    /// Sets the background color.
    /// </summary>
    /// <param name="color">Color.</param>
    public void SetBackgroundColor(Color color)
    {
        _virtualBottomSheet.BackgroundColor = color;
    }

    /// <summary>
    /// Sets padding.
    /// </summary>
    /// <param name="padding">Thickness.</param>
    public void SetPadding(Thickness padding)
    {
        _virtualBottomSheet.Padding = padding;
    }

    /// <summary>
    /// Set ignore safe area.
    /// </summary>
    /// <param name="ignoreSafeArea">Ignore safe area.</param>
    public void SetIgnoreSafeArea(bool ignoreSafeArea)
    {
        _virtualBottomSheetLayout.IgnoreSafeArea = ignoreSafeArea;
    }

    /// <summary>
    /// Set peek detent height.
    /// </summary>
    /// <param name="peekHeight">Peek height.</param>
    public void SetPeekHeight(double peekHeight)
    {
        _peekHeight = peekHeight;

        ApplyPeekHeight();
    }

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        if (!disposing)
        {
            return;
        }
#if NET9_0_OR_GREATER
        _virtualBottomSheet.DisconnectHandlers();
        _virtualBottomSheetContent?.DisconnectHandlers();
#else
        _virtualBottomSheet.Handler?.DisconnectHandler();
        _virtualBottomSheetContent?.Handler?.DisconnectHandler();
#endif
        _virtualBottomSheet.BottomSheet = null;

        HideHeader();

        _bottomSheetControllerDelegate.Dismissed -= RaiseDismissed;
        _bottomSheetControllerDelegate.StateChanged -= BottomSheetControllerDelegateOnStateChanged;
        _bottomSheetControllerDelegate.ConfirmDismiss -= BottomSheetControllerDelegateOnConfirmDismiss;
        _bottomSheetControllerDelegate.Dispose();
        _bottomSheetUIViewController.Dispose();

        _peekDetent?.Dispose();
        _mediumDetent?.Dispose();
        _largeDetent?.Dispose();

        _bottomSheetHeader?.Dispose();

        base.Dispose(disposing);
    }

    private void BottomSheetHeaderOnSizeChanged(object? sender, EventArgs e)
    {
        ApplyPeekHeight();
    }

    [SuppressMessage("Major Bug", "S1244:Floating point numbers should not be tested for equality", Justification = "False positive")]
    private nfloat PeekHeight(IUISheetPresentationControllerDetentResolutionContext arg)
    {
        if (OperatingSystem.IsMacCatalyst()
            || (OperatingSystem.IsIOS()
                && OperatingSystem.IsIOSVersionAtLeast(16)))
        {
#pragma warning disable CA1416
            return _peekDetentHeight <= arg.MaximumDetentValue ? new nfloat(_peekDetentHeight) : arg.MaximumDetentValue;
#pragma warning restore CA1416
        }
        else
        {
            return new nfloat(_peekDetentHeight);
        }
    }

    private void ApplyPeekHeight()
    {
        var peekHeight = (_bottomSheetHeader?.View.Height ?? 0)
            + _peekHeight;

        if (_virtualBottomSheetLayout.IgnoreSafeArea)
        {
            peekHeight -= WindowUtils.CurrentSafeAreaInsets().Bottom;
        }

        _peekDetentHeight = peekHeight;

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

    private void ApplyWindowBackgroundColor()
    {
        // ReSharper disable once ConditionalAccessQualifierIsNonNullableAccordingToAPIContract
        if (PresentationController?.ContainerView?.Subviews.Length > 0)
        {
            UIView.Animate(0.5, () => PresentationController.ContainerView.Subviews[0].BackgroundColor = IsModal ? _windowBackgroundColor.ToPlatform() : UIColor.Clear);
        }
    }

    private void BottomSheetControllerDelegateOnStateChanged(object? sender, BottomSheetStateChangedEventArgs e)
    {
        _eventManager.HandleEvent(
            this,
            e,
            nameof(StateChanged));
    }

    private void BottomSheetControllerDelegateOnConfirmDismiss(object? sender, EventArgs e)
    {
        _eventManager.HandleEvent(
            this,
            e,
            nameof(ConfirmDismiss));
    }

    private void RaiseDismissed(object? sender, EventArgs e)
    {
        _eventManager.HandleEvent(
            this,
            e,
            nameof(Dismissed));
    }
}