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

    private double _peekRowHeight;
    private double _peekDetentHeight;

    private View? _virtualBottomSheetPeek;
    private Thickness _virtualBottomSheetPeekMargin = new(0);
    private View? _virtualBottomSheetContent;

    private UISheetPresentationControllerDetentIdentifier _previousIdentifier = UISheetPresentationControllerDetentIdentifier.Unknown;

    private BottomSheetHeader? _bottomSheetHeader;

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
    /// Gets or sets a value indicating whether peek height should be calculated.
    /// </summary>
    public bool CalculatePeekHeight { get; set; }

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
    public void Close()
    {
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
        _bottomSheetHeader.SizeChanged += BottomSheetHeaderOnSizeChanged;
    }

    /// <summary>
    /// Adds the peek view.
    /// </summary>
    /// <param name="peek">Peek.</param>
    public void AddPeek(BottomSheetPeek? peek)
    {
        if (peek is null
            || _bottomSheetUIViewController.View is null)
        {
            return;
        }

        _virtualBottomSheetPeek = peek.PeekViewDataTemplate?.CreateContent() as View;

        if (_virtualBottomSheetPeek is null)
        {
            return;
        }

        _virtualBottomSheetPeek.BindingContext = peek.BindingContext;
        _virtualBottomSheetPeek.Parent = peek.Parent;

        _virtualBottomSheetLayout.Add(
            _virtualBottomSheetPeek,
            0,
            1);

        _virtualBottomSheetPeekMargin = _virtualBottomSheetPeek.Margin;

        CalculatePeekHeight = double.IsNaN(peek.PeekHeight);

        SetPeekRowHeight(CalculatePeekRowHeight(true));
        SetPeekDetentHeight(CalculatePeekHeight ? _peekRowHeight : peek.PeekHeight);
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

                SetPeekRowHeight(CalculatePeekRowHeight());
                SetPeekDetentHeight(CalculatePeekHeight ? _peekRowHeight : _peekDetentHeight);

                _previousIdentifier = identifier;
            });
        }
    }

    /// <summary>
    /// Sets peek height.
    /// </summary>
    /// <param name="peekHeight">Height.</param>
    public void SetPeekDetentHeight(double peekHeight)
    {
        _peekDetentHeight = peekHeight;
    }

    /// <summary>
    /// Sets peek row height.
    /// </summary>
    /// <param name="peekRowHeight">Row height.</param>
    public void SetPeekRowHeight(double peekRowHeight)
    {
        _virtualBottomSheetLayout.RowDefinitions[1].Height = new GridLength(peekRowHeight);
    }

    /// <summary>
    /// Calculate peek view height.
    /// </summary>
    /// <param name="force">Force calculation. Won't override <see cref="CalculatePeekHeight"/>.</param>
    /// <returns>Calculated peek height.</returns>
    public double CalculatePeekRowHeight(bool force = false)
    {
        var height = _peekRowHeight;

        if (SheetPresentationController is null)
        {
            return height;
        }

        if ((SheetPresentationController.SelectedDetentIdentifier != _previousIdentifier
                || force)
            && _virtualBottomSheetPeek is not null)
        {
            var insets = WindowUtils.CurrentSafeAreaInsets();
            var safeAreaBottom = 0.00;

            if (!_virtualBottomSheetLayout.IgnoreSafeArea
                && CalculatePeekHeight)
            {
                safeAreaBottom = insets.Bottom;
            }

            if (_previousIdentifier == UISheetPresentationControllerDetentIdentifier.Unknown
                && SheetPresentationController.SelectedDetentIdentifier != UISheetPresentationControllerDetentIdentifier.Unknown)
            {
                if (_virtualBottomSheetPeek.Margin.BottomContainsSafeArea(_virtualBottomSheetPeekMargin))
                {
                    _virtualBottomSheetPeek.Margin = new Thickness(
                        _virtualBottomSheetPeek.Margin.Left,
                        _virtualBottomSheetPeek.Margin.Top,
                        _virtualBottomSheetPeek.Margin.Right,
                        _virtualBottomSheetPeek.Margin.Bottom - safeAreaBottom);
                }

#if NET9_0_OR_GREATER
                var peekViewHeight = _virtualBottomSheetPeek.Measure(double.PositiveInfinity, double.PositiveInfinity).Height;
                height = peekViewHeight;
#else
                var peekViewHeight = _virtualBottomSheetPeek.Measure(double.PositiveInfinity, double.PositiveInfinity).Request.Height;
                height = peekViewHeight;
#endif
            }
            else if (SheetPresentationController.SelectedDetentIdentifier == UISheetPresentationControllerDetentIdentifier.Unknown)
            {
                if (_virtualBottomSheetPeek.Margin.Equals(_virtualBottomSheetPeekMargin))
                {
                    _virtualBottomSheetPeek.Margin = new Thickness(
                        _virtualBottomSheetPeek.Margin.Left,
                        _virtualBottomSheetPeek.Margin.Top,
                        _virtualBottomSheetPeek.Margin.Right,
                        _virtualBottomSheetPeek.Margin.Bottom + safeAreaBottom);
                }

#if NET9_0_OR_GREATER
                var peekViewHeight = _virtualBottomSheetPeek.Measure(double.PositiveInfinity, double.PositiveInfinity).Height;
                height = peekViewHeight;
#else
                var peekViewHeight = _virtualBottomSheetPeek.Measure(double.PositiveInfinity, double.PositiveInfinity).Request.Height;
                height = peekViewHeight;
#endif
            }
        }

        _peekRowHeight = height;

        return height;
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
            _bottomSheetControllerDelegate.Dispose();
            _bottomSheetUIViewController.Dispose();

            _peekDetent?.Dispose();
            _mediumDetent?.Dispose();
            _largeDetent?.Dispose();

            _bottomSheetHeader?.Dispose();
        }

        base.Dispose(disposing);
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

    private void BottomSheetHeaderOnSizeChanged(object? sender, EventArgs e)
    {
        if (_bottomSheetHeader is null)
        {
            return;
        }

        _peekDetentHeight += _bottomSheetHeader.View.Height;
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