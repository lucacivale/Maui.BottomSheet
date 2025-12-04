using AsyncAwaitBestPractices;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using Plugin.BottomSheet.Windows.Extensions;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Windows.Foundation;

namespace Plugin.BottomSheet.Windows;

/// <summary>
/// Represents a BottomSheet that is a custom content dialog with specialized behaviors such as open, close, and cancel actions.
/// It allows setting custom content and adjusting visual properties such as background and window background.
/// </summary>
internal sealed partial class BottomSheet : ContentDialog
{
    private const string WindowBackgroundViewName = "SmokeLayerBackground";
    private const string BackgroundViewName = "Content";

    private readonly WeakEventManager _eventManager = new();

    private Brush? _defaultWindowBackground;
    private Brush? _defaultBackground;

    /// <summary>
    /// Occurs when the bottom sheet is canceled.
    /// </summary>
    public event EventHandler Canceled
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
    public Brush? WindowBackground
    {
        get
        {
            Brush? windowBackground = null;

            if (this.FindAscendant<Canvas>() is Canvas popupRoot
                && popupRoot.Children.OfType<Rectangle>().FirstOrDefault(x => x.Name is WindowBackgroundViewName) is Rectangle background)
            {
                windowBackground = background.Fill;
            }

            return windowBackground;
        }

        set
        {
            if (this.FindAscendant<Canvas>() is Canvas popupRoot
                && popupRoot.Children.OfType<Rectangle>().FirstOrDefault(x => x.Name is WindowBackgroundViewName) is Rectangle background)
            {
                background.Fill = value ?? _defaultWindowBackground;
            }
        }
    }

    /// <summary>
    /// Gets the frame (position and size) of the bottom sheet relative to its parent container.
    /// </summary>
    public Rect Frame
    {
        get
        {
            Rect frame = new(0, 0, 0, 0);

            if (this.FindDescendant(BackgroundViewName)?.Parent is Grid container
                && this.FindAscendant<Canvas>() is Canvas popupRoot)
            {
                Point point = container.TransformToVisual(popupRoot).TransformPoint(new(0, 0));
                frame = new Rect(point.X, point.Y, ActualWidth, ActualHeight);
            }

            return frame;
        }
    }

    /// <summary>
    /// Gets or sets the background of the bottom sheet's content area.
    /// </summary>
    public new Brush? Background
    {
        get
        {
            Brush? background = null;

            if (this.FindDescendant(BackgroundViewName)?.Parent is Grid container)
            {
                background = container.Background;
            }

            return background;
        }

        set
        {
            if (this.FindDescendant(BackgroundViewName)?.Parent is Grid container)
            {
                base.Background = value ?? _defaultBackground;
                container.Background = base.Background;
            }
        }
    }

    /// <summary>
    /// Sets the content view of the bottom sheet.
    /// </summary>
    /// <param name="view">The view to set as the content of the bottom sheet.</param>
    public void SetContentView(UIElement view)
    {
        Content = view;
    }

    /// <summary>
    /// Opens the bottom sheet asynchronously and waits for it to open.
    /// </summary>
    /// <param name="root">The XAML root for the bottom sheet (can be null).</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task OpenAsync(XamlRoot? root)
    {
        XamlRoot = root;

        TaskCompletionSource taskCompletionSource = new();

        TypedEventHandler<ContentDialog, ContentDialogOpenedEventArgs> @event = null!;
        @event = (s, e) =>
        {
            ((BottomSheet)s!).Opened -= @event;

            ((BottomSheet)s!).IsOpen = true;

            SetDefaultValues();

            _ = taskCompletionSource.TrySetResult();
        };

        Opened += @event;
        Closing += BottomSheet_Closing;

        ShowAsync().AsTask().SafeFireAndForget();

        await taskCompletionSource.Task.ConfigureAwait(true);
    }

    /// <summary>
    /// Closes the bottom sheet asynchronously and waits for it to close.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task CloseAsync()
    {
        TaskCompletionSource taskCompletionSource = new();

        TypedEventHandler<ContentDialog, ContentDialogClosedEventArgs> @event = null!;
        @event = (s, e) =>
        {
            ((BottomSheet)s!).Closed -= @event;

            _ = taskCompletionSource.TrySetResult();
            ((BottomSheet)s!).IsOpen = false;
        };

        Closed += @event;

        using CloseContext context = new();

        Hide();

        await taskCompletionSource.Task.ConfigureAwait(true);
    }

    /// <summary>
    /// Cancels the current operation of the bottom sheet and raises the <see cref="Canceled"/> event.
    /// </summary>
    public void Cancel()
    {
        _eventManager.RaiseEvent(this, EventArgs.Empty, nameof(Canceled));
    }

    /// <summary>
    /// Sets default values for background and window background when the bottom sheet is opened.
    /// </summary>
    private void SetDefaultValues()
    {
        _defaultBackground = Background;
        _defaultWindowBackground = WindowBackground;

        if (this.FindDescendant(BackgroundViewName)?.Parent is Grid container)
        {
            container.Padding = new(0);
        }
    }

    /// <summary>
    /// Handles the <see cref="ContentDialog.Closing"/> event for the bottom sheet.
    /// Attempts to cancel the closing if certain conditions are met.
    /// </summary>
    /// <param name="sender">The content dialog that is closing.</param>
    /// <param name="args">The event arguments for the closing event.</param>
    [SuppressMessage("Usage", "VSTHRD100: Avoid async void methods", Justification = "Is okay here.")]
    [SuppressMessage("Design", "CA1031: Do not catch general exception types", Justification = "Catch all exceptions to prevent crash.")]
    private async void BottomSheet_Closing(ContentDialog sender, ContentDialogClosingEventArgs args)
    {
        try
        {
            if (CloseContext.Instance() is null)
            {
                args.Cancel = true;
                await Task.Delay(100).ConfigureAwait(true);
            }

            Cancel();
        }
        catch (Exception ex)
        {
            Trace.TraceError("Invoking BottomSheet_Closing: {0}", ex);
        }
    }
}
