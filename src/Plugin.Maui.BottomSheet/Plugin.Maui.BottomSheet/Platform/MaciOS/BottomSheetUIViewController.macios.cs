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

    private readonly ContentPage _virtualBottomSheet;
    private readonly Grid _virtualBottomSheetLayout;

    private UISheetPresentationControllerDetent? _peekDetent;
    private UISheetPresentationControllerDetent? _mediumDetent;
    private UISheetPresentationControllerDetent? _largeDetent;

    private double _peekDetentHeight;

    private View? _virtualBottomSheetPeek;
    private View? _virtualBottomSheetContent;

    private UISheetPresentationControllerDetentIdentifier _previousIdentifier = UISheetPresentationControllerDetentIdentifier.Unknown;

    private BottomSheetHeader? _bottomSheetHeader;
    private BottomSheetPeek? _bottomSheetPeek;

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
                new RowDefinition(0),
                new RowDefinition(GridLength.Star)],
        };
        _virtualBottomSheet = new ContentPage()
        {
            Content = _virtualBottomSheetLayout,
        };
        _bottomSheetUIViewController = _virtualBottomSheet.ToUIViewController(mauiContext);

        SetViewControllers([_bottomSheetUIViewController], true);
        SetNavigationBarHidden(true, true);

        if (SheetPresentationController is not null)
        {
            _bottomSheetControllerDelegate.Dismissed += BottomSheetControllerDelegateOnDismissed;
            _bottomSheetControllerDelegate.StateChanged += BottomSheetControllerDelegateOnStateChanged;
        }
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
    public UISheetPresentationControllerDetent MediumDetent
    {
        get => _mediumDetent ??= UISheetPresentationControllerDetent.CreateMediumDetent();
    }

    /// <summary>
    /// Gets large detent.
    /// </summary>
    public UISheetPresentationControllerDetent LargeDetent
    {
        get => _largeDetent ??= UISheetPresentationControllerDetent.CreateLargeDetent();
    }

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
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task OpenAsync()
    {
        if (SheetPresentationController is not null)
        {
            _previousIdentifier = SheetPresentationController.SelectedDetentIdentifier;
        }

        if (WindowStateManager.Default.GetCurrentUIViewController() is UIViewController parent)
        {
            await parent.PresentViewControllerAsync(this, true).ConfigureAwait(true);
        }
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

        _virtualBottomSheetPeek?.DisconnectHandlers();
        _virtualBottomSheetPeek = null;
#else
        _virtualBottomSheetContent?.Handler?.DisconnectHandler();
        _virtualBottomSheetContent = null;

        _virtualBottomSheetPeek?.Handler?.DisconnectHandler();
        _virtualBottomSheetPeek = null;
#endif
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
    }

    /// <summary>
    /// Adds the header view.
    /// </summary>
    /// <param name="header">Header.</param>
    public void AddHeader(Plugin.Maui.BottomSheet.BottomSheetHeader? header)
    {
        if (header is null)
        {
            return;
        }

        _bottomSheetHeader = new BottomSheetHeader(header);
    }

    /// <summary>
    /// Adds the peek view.
    /// </summary>
    /// <param name="peek">Peek.</param>
    public void AddPeek(BottomSheetPeek? peek)
    {
        if (peek is null)
        {
            return;
        }

        _bottomSheetPeek = peek;

        if (_virtualBottomSheetPeek is not null)
        {
            _virtualBottomSheetPeek.SizeChanged -= BottomSheetPeekOnSizeChanged;
        }

        _virtualBottomSheetPeek = _bottomSheetPeek.PeekViewDataTemplate?.CreateContent() as View;

        if (_virtualBottomSheetPeek is null)
        {
            return;
        }

        _virtualBottomSheetPeek.BindingContext = _bottomSheetPeek.BindingContext;
        _virtualBottomSheetPeek.Parent = _bottomSheetPeek.Parent;
        _virtualBottomSheetPeek.SizeChanged += BottomSheetPeekOnSizeChanged;

        _virtualBottomSheetLayout.Add(
            _virtualBottomSheetPeek,
            0,
            1);

        LayoutPeek(true);
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

        _virtualBottomSheetContent = content.ContentTemplate?.CreateContent() as View;

        if (_virtualBottomSheetContent is null)
        {
            return;
        }

        _virtualBottomSheetContent.BindingContext = content.BindingContext;
        _virtualBottomSheetContent.Parent = content.Parent;

        _virtualBottomSheetLayout.Add(
            _virtualBottomSheetContent,
            0,
            2);
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
                LayoutPeek();
                _previousIdentifier = identifier;
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

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
#if NET9_0_OR_GREATER
            _virtualBottomSheet.DisconnectHandlers();
            _virtualBottomSheetContent?.DisconnectHandlers();
            _virtualBottomSheetPeek?.DisconnectHandlers();
#else
            _virtualBottomSheet.Handler?.DisconnectHandler();
            _virtualBottomSheetContent?.Handler?.DisconnectHandler();
            _virtualBottomSheetPeek?.Handler?.DisconnectHandler();
#endif
            HideHeader();

            _bottomSheetControllerDelegate.Dispose();
            _bottomSheetUIViewController.Dispose();

            _peekDetent?.Dispose();
            _mediumDetent?.Dispose();
            _largeDetent?.Dispose();

            _bottomSheetHeader?.Dispose();
        }

        base.Dispose(disposing);
    }

    private void LayoutPeek(bool force = false)
    {
        if (SheetPresentationController is null
            || (SheetPresentationController.SelectedDetentIdentifier == _previousIdentifier
                && !force)
            || _virtualBottomSheetPeek is null)
        {
            return;
        }

        var insets = WindowUtils.CurrentSafeAreaInsets();
        var safeAreaBottom = 0.00;

        if (!_virtualBottomSheetLayout.IgnoreSafeArea)
        {
            safeAreaBottom = insets.Bottom;
        }

        if ((_previousIdentifier == UISheetPresentationControllerDetentIdentifier.Unknown
                && SheetPresentationController.SelectedDetentIdentifier != UISheetPresentationControllerDetentIdentifier.Unknown)
            || !ExistPeekDetent())
        {
#if NET9_0_OR_GREATER
            var peekViewHeight = _virtualBottomSheetPeek.Measure(double.PositiveInfinity, double.PositiveInfinity).Height;
#else
            var peekViewHeight = _virtualBottomSheetPeek.Measure(double.PositiveInfinity, double.PositiveInfinity).Request.Height;
#endif
            _virtualBottomSheetLayout.RowDefinitions[1].Height = new GridLength(peekViewHeight);
        }
        else if (SheetPresentationController.SelectedDetentIdentifier == UISheetPresentationControllerDetentIdentifier.Unknown)
        {
#if NET9_0_OR_GREATER
            var peekViewHeight = _virtualBottomSheetPeek.Measure(double.PositiveInfinity, double.PositiveInfinity).Height;
#else
            var peekViewHeight = _virtualBottomSheetPeek.Measure(double.PositiveInfinity, double.PositiveInfinity).Request.Height;
#endif
            _virtualBottomSheetLayout.RowDefinitions[1].Height = new GridLength(peekViewHeight + safeAreaBottom);
        }
    }

    [SuppressMessage("Major Bug", "S6605", Justification = "False positive")]
    private bool ExistPeekDetent()
    {
        if (OperatingSystem.IsMacCatalyst()
            || (OperatingSystem.IsIOS()
                && OperatingSystem.IsIOSVersionAtLeast(16)))
        {
#pragma warning disable CA1416
            return SheetPresentationController?.Detents.Any(x => x.Identifier == PeekDetentId) == true;
#pragma warning restore CA1416
        }
        else
        {
            return false;
        }
    }

    private void BottomSheetHeaderOnSizeChanged(object? sender, EventArgs e)
    {
        _peekDetentHeight = (_bottomSheetHeader?.View.Height ?? 0)
            + PeekHeight();

        if (OperatingSystem.IsMacCatalyst()
            || (OperatingSystem.IsIOS()
                && OperatingSystem.IsIOSVersionAtLeast(16)))
        {
#pragma warning disable CA1416
            SheetPresentationController?.InvalidateDetents();
#pragma warning restore CA1416
        }
    }

    private void BottomSheetPeekOnSizeChanged(object? sender, EventArgs e)
    {
        LayoutPeek(true);
#if NET9_0_OR_GREATER
        var peekViewHeight = _virtualBottomSheetPeek?.Measure(double.PositiveInfinity, double.PositiveInfinity).Height ?? 0;
#else
        var peekViewHeight = _virtualBottomSheetPeek?.Measure(double.PositiveInfinity, double.PositiveInfinity).Request.Height ?? 0;
#endif

        _peekDetentHeight = (_bottomSheetHeader?.View.Height ?? 0)
            + (DeviceInfo.Idiom == DeviceIdiom.Tablet ? PeekHeight() : peekViewHeight);

        if (OperatingSystem.IsMacCatalyst()
            || (OperatingSystem.IsIOS()
                && OperatingSystem.IsIOSVersionAtLeast(16)))
        {
#pragma warning disable CA1416
            SheetPresentationController?.InvalidateDetents();
#pragma warning restore CA1416
        }
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

    private double PeekHeight()
    {
        if (_bottomSheetPeek is not null
            && !double.IsNaN(_bottomSheetPeek.PeekHeight))
        {
            return _bottomSheetPeek.PeekHeight;
        }
        else
        {
            return DeviceInfo.Idiom == DeviceIdiom.Tablet ?
                _virtualBottomSheetLayout.RowDefinitions[1].Height.Value
                : _virtualBottomSheetPeek?.Height ?? 0;
        }
    }

    private void BottomSheetControllerDelegateOnStateChanged(object? sender, BottomSheetStateChangedEventArgs e)
    {
        _eventManager.HandleEvent(
            this,
            e,
            nameof(StateChanged));
    }

    private void BottomSheetControllerDelegateOnDismissed(object? sender, EventArgs e)
    {
        _eventManager.HandleEvent(
            this,
            e,
            nameof(Dismissed));
    }
}