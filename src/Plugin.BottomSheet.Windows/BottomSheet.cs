using AsyncAwaitBestPractices;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Windows.Foundation;
using WVirtualKey = Windows.System.VirtualKey;
using WVirtualKeyModifiers = Windows.System.VirtualKeyModifiers;

namespace Plugin.BottomSheet.Windows;

/// <summary>
/// Represents a BottomSheet that is a custom content dialog with specialized behaviors such as open, close, and cancel actions.
/// It allows setting custom content and adjusting visual properties such as background and window background.
/// </summary>
public sealed partial class BottomSheet
{
    private const string DefaultBackgroundResourceKey = "SolidBackgroundFillColorBaseBrush";
    private const string ContentDialogBorderWidthResourceKey = "ContentDialogBorderWidth";
    private const string ContentDialogMinWidthResourceKey = "ContentDialogMinWidth";
    private const string ContentDialogMinHeightResourceKey = "ContentDialogMinHeight";
    private const string ContentDialogMaxWidthResourceKey = "ContentDialogMaxWidth";
    private const string ContentDialogMaxHeightResourceKey = "ContentDialogMaxHeight";

    private readonly WeakEventManager _eventManager = new();

    private readonly Brush _defaultWindowBackground;
    private readonly Brush _defaultBackground;

    private readonly Popup _popup;
    private readonly Grid _container;
    private readonly Border _dialogBorder;
    private readonly StackPanel _contentPanel;
    private readonly ContentPresenter _contentPresenter;
    private readonly KeyboardAccelerator _escapeKeyboardAccelerator;

    /// <summary>
    /// Initializes a new instance of the <see cref="BottomSheet"/> class.
    /// </summary>
    /// <param name="window">Window.</param>
    public BottomSheet(Window window)
    {
        _escapeKeyboardAccelerator = new()
        {
            Key = WVirtualKey.Escape,
            Modifiers = WVirtualKeyModifiers.None,
        };

        _defaultWindowBackground = _defaultBackground = new SolidColorBrush(Colors.Black)
        {
            Opacity = 0.5,
        };
        _defaultBackground = (Brush)Application.Current.Resources[DefaultBackgroundResourceKey];

        _popup = new Popup
        {
            IsLightDismissEnabled = false,
        };

        _container = new Grid
        {
            Height = window.Bounds.Height,
            Width = window.Bounds.Width,
            Background = _defaultWindowBackground,
        };

        _dialogBorder = new Border
        {
            Background = _defaultBackground,
            BorderThickness = (Microsoft.UI.Xaml.Thickness)Application.Current.Resources[ContentDialogBorderWidthResourceKey],
            MinWidth = ContentDialogMinWidth,
            MinHeight = ContentDialogMinHeight,
            MaxWidth = ContentDialogMaxWidth,
            MaxHeight = ContentDialogMaxHeight,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
        };

        _contentPanel = new StackPanel();

        _contentPresenter = new ContentPresenter();

        _contentPanel.Children.Add(_contentPresenter);

        _dialogBorder.Child = _contentPanel;
        _container.Children.Add(_dialogBorder);
        _popup.Child = _container;
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
    /// Occurs when the bottom sheet size changed.
    /// </summary>
    public event EventHandler<SizeChangedEventArgs> SizeChanged
    {
        add => _eventManager.AddEventHandler(value);
        remove => _eventManager.RemoveEventHandler(value);
    }

    /// <summary>
    /// Gets a value indicating whether the bottom sheet is currently open.
    /// </summary>
    public bool IsOpen { get; private set; }

    /// <summary>
    /// Gets or sets the background of the bottom sheet's window.
    /// </summary>
    public Brush WindowBackground
    {
        get => _container.Background;
        set => _container.Background = value;
    }

    /// <summary>
    /// Gets or sets the corner radius.
    /// </summary>
    public double CornerRadius
    {
        get => _dialogBorder.CornerRadius.TopRight;
        set => _dialogBorder.CornerRadius = new(value);
    }

    public double MinWidth
    {
        get => _dialogBorder.MinWidth;
        set
        {
            _dialogBorder.MinWidth = double.IsNaN(value) ? (double)Application.Current.Resources[ContentDialogMinWidthResourceKey] : value;
        }
    }

    public double MinHeight
    {
        get => _dialogBorder.MinHeight;
        set
        {
            _dialogBorder.MinHeight = double.IsNaN(value) ? (double)Application.Current.Resources[ContentDialogMinHeightResourceKey] : value;
        }
    }

    public double MaxWidth
    {
        get => _dialogBorder.MaxWidth;
        set
        {
            _dialogBorder.MaxWidth = double.IsNaN(value) ? (double)Application.Current.Resources[ContentDialogMaxWidthResourceKey] : value;
        }
    }

    public double MaxHeight
    {
        get => _dialogBorder.MaxHeight;
        set
        {
            _dialogBorder.MaxHeight = double.IsNaN(value) ? (double)Application.Current.Resources[ContentDialogMaxHeightResourceKey] : value;
        }
    }

    /// <summary>
    /// Gets the frame (position and size) of the bottom sheet relative to its parent container.
    /// </summary>
    public Rect Frame
    {
        get
        {
            Point point = _dialogBorder.TransformToVisual(_popup).TransformPoint(new(0, 0));

            return new Rect(point.X, point.Y, _dialogBorder.ActualWidth, _dialogBorder.ActualHeight);
        }
    }

    /// <summary>
    /// Gets or sets the background of the bottom sheet's content area.
    /// </summary>
    public Brush Background
    {
        get => _dialogBorder.Background;
        set => _dialogBorder.Background = value;
    }

    internal static double ContentDialogMinWidth => (double)Application.Current.Resources[ContentDialogMinWidthResourceKey];

    internal static double ContentDialogMinHeight => (double)Application.Current.Resources[ContentDialogMinHeightResourceKey];

    internal static double ContentDialogMaxWidth => (double)Application.Current.Resources[ContentDialogMaxWidthResourceKey];

    internal static double ContentDialogMaxHeight => (double)Application.Current.Resources[ContentDialogMaxHeightResourceKey];

    /// <summary>
    /// Sets the content view of the bottom sheet.
    /// </summary>
    /// <param name="view">The view to set as the content of the bottom sheet.</param>
    public void SetContentView(UIElement view)
    {
        _contentPresenter.Content = view;
    }

    /// <summary>
    /// Opens the bottom sheet asynchronously and waits for it to open.
    /// </summary>
    /// <param name="root">The XAML root for the bottom sheet (can be null).</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task OpenAsync(XamlRoot? root)
    {
        _popup.XamlRoot = root;

        TaskCompletionSource taskCompletionSource = new();

        EventHandler<object> @event = null!;
        @event = (s, e) =>
        {
            _popup.Opened -= @event;

            IsOpen = true;

            _ = taskCompletionSource.TrySetResult();
        };

        _popup.Opened += @event;

        _dialogBorder.SizeChanged += DialogBorder_SizeChanged;
        _container.Tapped += WindowBackgroundClicked;

        _escapeKeyboardAccelerator.Invoked += EscapeClicked;
        _popup.XamlRoot?.Content.KeyboardAccelerators.Add(_escapeKeyboardAccelerator);

        _popup.IsOpen = true;

        await taskCompletionSource.Task.ConfigureAwait(true);
    }

    /// <summary>
    /// Closes the bottom sheet asynchronously and waits for it to close.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task CloseAsync()
    {
        TaskCompletionSource taskCompletionSource = new();

        EventHandler<object> @event = null!;
        @event = (s, e) =>
        {
            _popup.Closed -= @event;

            _ = taskCompletionSource.TrySetResult();
            IsOpen = false;
        };

        _popup.Closed += @event;

        _dialogBorder.SizeChanged -= DialogBorder_SizeChanged;
        _container.Tapped -= WindowBackgroundClicked;

        _escapeKeyboardAccelerator.Invoked -= EscapeClicked;
        _popup.XamlRoot?.Content.KeyboardAccelerators.Remove(_escapeKeyboardAccelerator);

        _popup.IsOpen = false;

        await taskCompletionSource.Task.ConfigureAwait(true);
    }

    /// <summary>
    /// Cancels the current operation of the bottom sheet and raises the <see cref="Canceled"/> event.
    /// </summary>
    public void Cancel()
    {
        _eventManager.RaiseEvent(this, EventArgs.Empty, nameof(Canceled));
    }

    private void DialogBorder_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        _eventManager.RaiseEvent(this, e, nameof(SizeChanged));
    }

    private void WindowBackgroundClicked(object sender, TappedRoutedEventArgs e)
    {
        if (e.OriginalSource is Grid grid
            && grid == _container)
        {
            _eventManager.RaiseEvent(this, EventArgs.Empty, nameof(Canceled));
        }
    }

    private void EscapeClicked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
    {
        _eventManager.RaiseEvent(this, EventArgs.Empty, nameof(Canceled));
    }
}
