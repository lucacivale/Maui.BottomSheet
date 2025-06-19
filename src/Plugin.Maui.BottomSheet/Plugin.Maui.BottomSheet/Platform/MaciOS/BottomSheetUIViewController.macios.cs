using CoreGraphics;
using Microsoft.Maui.Graphics.Platform;

namespace Plugin.Maui.BottomSheet.Platform.MaciOS;

using System.Diagnostics.CodeAnalysis;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Platform;
using UIKit;

/// <summary>
/// UIViewController implementation for managing bottom sheet presentation on macOS and iOS platforms.
/// </summary>
internal sealed class BottomSheetUIViewController : UINavigationController
{
    /// <summary>
    /// Identifier for custom peek detent used in iOS 16+ and macOS.
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
    /// <param name="mauiContext">The MAUI context for platform services.</param>
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
    /// Occurs when the bottom sheet state changes.
    /// </summary>
    public event EventHandler<BottomSheetStateChangedEventArgs> StateChanged
    {
        add => _eventManager.AddEventHandler(value);
        remove => _eventManager.RemoveEventHandler(value);
    }

    /// <summary>
    /// Occurs when the bottom sheet dismissal needs confirmation.
    /// </summary>
    public event EventHandler ConfirmDismiss
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
    /// Gets the peek detent for the bottom sheet, creating a custom detent on supported platforms.
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
    /// Gets the medium detent for the bottom sheet.
    /// </summary>
    public UISheetPresentationControllerDetent MediumDetent => _mediumDetent ??= UISheetPresentationControllerDetent.CreateMediumDetent();

    /// <summary>
    /// Gets the large detent for the bottom sheet.
    /// </summary>
    public UISheetPresentationControllerDetent LargeDetent => _largeDetent ??= UISheetPresentationControllerDetent.CreateLargeDetent();

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
    /// Gets or sets the available detents for the bottom sheet.
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
    /// Opens the bottom sheet with the specified configuration.
    /// </summary>
    /// <param name="bottomSheet">The bottom sheet configuration to open.</param>
    /// <returns>A task that returns true if the bottom sheet opened successfully.</returns>
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
    /// Closes the bottom sheet and cleans up resources.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task CloseAsync()
    {
        await DismissViewControllerAsync(true).ConfigureAwait(true);
        _virtualBottomSheetLayout.Clear();

        _virtualBottomSheetContent?.DisconnectHandlers();
        _virtualBottomSheetContent = null;
    }

    /// <summary>
    /// Called when the view is appearing, applies window background color.
    /// </summary>
    /// <param name="animated">Whether the appearance is animated.</param>
    public override void ViewIsAppearing(bool animated)
    {
        base.ViewIsAppearing(animated);

        ApplyWindowBackgroundColor();
    }

    /// <summary>
    /// Called when the view will disappear, handles modal background animation.
    /// </summary>
    /// <param name="animated">Whether the disappearance is animated.</param>
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
    /// Attaches the presentation delegate to handle sheet dismissal and state changes.
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
    /// Shows the header in the bottom sheet layout.
    /// </summary>
    public void ShowHeader()
    {
        if (_bottomSheetHeader is null)
        {
            return;
        }

        _virtualBottomSheetLayout.Add(_bottomSheetHeader.View);
        _bottomSheetHeader.SizeChanged += BottomSheetHeaderOnSizeChanged;
        _bottomSheetHeader.CloseButtonClicked += BottomSheetControllerDelegateOnConfirmDismiss;
    }

    /// <summary>
    /// Hides the header from the bottom sheet layout.
    /// </summary>
    public void HideHeader()
    {
        if (_bottomSheetHeader is null)
        {
            return;
        }

        _bottomSheetHeader.Hide();
        _bottomSheetHeader.SizeChanged -= BottomSheetHeaderOnSizeChanged;
        _bottomSheetHeader.CloseButtonClicked -= BottomSheetControllerDelegateOnConfirmDismiss;
        ApplyPeekHeight();
    }

    /// <summary>
    /// Adds a header to the bottom sheet with the specified configuration and style.
    /// </summary>
    /// <param name="header">The header configuration.</param>
    /// <param name="style">The header style settings.</param>
    public void AddHeader(Plugin.Maui.BottomSheet.BottomSheetHeader? header, BottomSheetHeaderStyle style)
    {
        if (header is null)
        {
            return;
        }

        _bottomSheetHeader = new BottomSheetHeader(header, style);
    }

    /// <summary>
    /// Sets the header style for the existing header.
    /// </summary>
    /// <param name="style">The header style settings to apply.</param>
    public void SetHeaderStyle(BottomSheetHeaderStyle style)
    {
        if (_bottomSheetHeader is null)
        {
            return;
        }

        _bottomSheetHeader.SetStyle(style);
    }

    /// <summary>
    /// Adds content to the bottom sheet layout.
    /// </summary>
    /// <param name="content">The content to add to the bottom sheet.</param>
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
    /// Sets the current detent identifier for the sheet presentation.
    /// </summary>
    /// <param name="identifier">The detent identifier to set.</param>
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
    /// Sets whether the grabber (drag handle) should be visible.
    /// </summary>
    /// <param name="visible">True to show the grabber, false to hide it.</param>
    public void SetPrefersGrabberVisible(bool visible)
    {
        if (SheetPresentationController is not null)
        {
            SheetPresentationController.PrefersGrabberVisible = visible;
        }
    }

    /// <summary>
    /// Sets the corner radius for the bottom sheet.
    /// </summary>
    /// <param name="cornerRadius">The corner radius value.</param>
    public void SetCornerRadius(float cornerRadius)
    {
        if (SheetPresentationController is not null)
        {
            SheetPresentationController.PreferredCornerRadius = cornerRadius;
        }
    }

    /// <summary>
    /// Sets the window background color for the bottom sheet.
    /// </summary>
    /// <param name="color">The background color to apply.</param>
    public void SetWindowBackgroundColor(Color color)
    {
        _windowBackgroundColor = color;
        ApplyWindowBackgroundColor();
    }

    /// <summary>
    /// Sets the background color for the bottom sheet.
    /// </summary>
    /// <param name="color">The background color to apply.</param>
    public void SetBackgroundColor(Color color)
    {
        _virtualBottomSheet.BackgroundColor = color;
    }

    /// <summary>
    /// Sets the padding for the bottom sheet content.
    /// </summary>
    /// <param name="padding">The padding thickness to apply.</param>
    public void SetPadding(Thickness padding)
    {
        _virtualBottomSheet.Padding = padding;
    }

    /// <summary>
    /// Sets whether the bottom sheet should ignore safe area constraints.
    /// </summary>
    /// <param name="ignoreSafeArea">True to ignore safe area, false to respect it.</param>
    public void SetIgnoreSafeArea(bool ignoreSafeArea)
    {
        _virtualBottomSheetLayout.IgnoreSafeArea = ignoreSafeArea;
    }

    /// <summary>
    /// Sets the peek height for the bottom sheet.
    /// </summary>
    /// <param name="peekHeight">The peek height value.</param>
    public void SetPeekHeight(double peekHeight)
    {
        _peekHeight = peekHeight;

        ApplyPeekHeight();
    }

    /// <summary>
    /// Called when the view layout is complete, handles frame change notifications.
    /// </summary>
    public override void ViewDidLayoutSubviews()
    {
        base.ViewDidLayoutSubviews();

        if (PresentationController is null)
        {
            return;
        }

        CGRect frame = PresentationController.FrameOfPresentedViewInContainerView;
        frame.Y -= new nfloat(Math.Abs(NavigationBar.Frame.Y.Value)) + PresentationController.PresentedView.LayoutMargins.Top;

        _eventManager.HandleEvent(
            this,
            frame.AsRectangle(),
            nameof(FrameChanged));
    }

    /// <summary>
    /// Releases all resources used by the view controller.
    /// </summary>
    /// <param name="disposing">True if disposing managed resources.</param>
    protected override void Dispose(bool disposing)
    {
        if (!disposing)
        {
            return;
        }

        _virtualBottomSheet.DisconnectHandlers();
        _virtualBottomSheetContent?.DisconnectHandlers();
        _virtualBottomSheet.BottomSheet = null;

        HideHeader();

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

    /// <summary>
    /// Handles header size change events and updates peek height.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    private void BottomSheetHeaderOnSizeChanged(object? sender, EventArgs e)
    {
        ApplyPeekHeight();
    }

    /// <summary>
    /// Calculates the peek height based on container context and constraints.
    /// </summary>
    /// <param name="arg">The detent resolution context.</param>
    /// <returns>The calculated peek height as a native float.</returns>
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

    /// <summary>
    /// Applies the calculated peek height and updates detents if necessary.
    /// </summary>
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

    /// <summary>
    /// Applies the window background color based on modal state.
    /// </summary>
    private void ApplyWindowBackgroundColor()
    {
        // ReSharper disable once ConditionalAccessQualifierIsNonNullableAccordingToAPIContract
        if (PresentationController?.ContainerView?.Subviews.Length > 0)
        {
            UIView.Animate(0.25, () => PresentationController.ContainerView.Subviews[0].BackgroundColor = IsModal ? _windowBackgroundColor.ToPlatform() : UIColor.Clear);
        }
    }

    /// <summary>
    /// Handles state change events from the delegate and forwards them.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The state change event arguments.</param>
    private void BottomSheetControllerDelegateOnStateChanged(object? sender, BottomSheetStateChangedEventArgs e)
    {
        _eventManager.HandleEvent(
            this,
            e,
            nameof(StateChanged));
    }

    /// <summary>
    /// Handles confirm dismiss events from the delegate and forwards them.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    private void BottomSheetControllerDelegateOnConfirmDismiss(object? sender, EventArgs e)
    {
        _eventManager.HandleEvent(
            this,
            e,
            nameof(ConfirmDismiss));
    }
}