using AsyncAwaitBestPractices;
using CommunityToolkit.WinUI;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using Windows.Foundation;
using Windows.UI;

namespace Plugin.BottomSheet.Windows;

internal sealed partial class BottomSheet : ContentDialog
{
    private readonly WeakEventManager _eventManager = new();

    public event EventHandler Canceled
    {
        add => _eventManager.AddEventHandler(value);
        remove => _eventManager.RemoveEventHandler(value);
    }

    public bool IsOpen { get; private set; }

    public Brush? WindowBackground
    {
        get
        {
            Brush? windowBackground = null;

            if (this.FindAscendant<Canvas>() is Canvas popupRoot
                && popupRoot.Children.OfType<Rectangle>().FirstOrDefault(x => x.Name is "SmokeLayerBackground") is Rectangle background)
            {
                windowBackground = background.Fill;
            }

            return windowBackground;
        }

        set
        {
            if (this.FindAscendant<Canvas>() is Canvas popupRoot
                && popupRoot.Children.OfType<Rectangle>().FirstOrDefault(x => x.Name is "SmokeLayerBackground") is Rectangle background)
            {
                background.Fill = value;
            }
        }
    }

    public new Brush? Background
    {
        get
        {
            Brush? background = null;

            if (this.FindDescendant("BackgroundElement") is Border layoutRoot)
            {
                background = layoutRoot.Background;
            }

            return background;
        }

        set
        {
            base.Background = value;
            if (this.FindDescendant("BackgroundElement") is Border backgroundElement)
            {
                backgroundElement.Background = value;
            }

            if (this.FindDescendant("Root") is Border root)
            {
                root.Background = value;
            }

            if (this.FindDescendant("DialogSpace") is Grid dialogSpace)
            {
                dialogSpace.Background = value;
            }

            if (this.FindDescendant("ContentScrollViewer") is ScrollViewer contentScrollViewer)
            {
                contentScrollViewer.Background = value;
                var grid = contentScrollViewer.FindDescendants().OfType<Grid>().First();
                grid.Background = value;

                grid.FindDescendant<ContentControl>().Background = value;
                grid.FindDescendant<ContentPresenter>().Background = value;
            }

            var a = this.FindDescendants();

            var b = 10;
        }
    }

    public void SetContentView(UIElement view)
    {
        Content = view;
    }

    public async Task OpenAsync(XamlRoot? root)
    {
        XamlRoot = root;

        TaskCompletionSource taskCompletionSource = new();

        TypedEventHandler<ContentDialog, ContentDialogOpenedEventArgs> @event = null!;
        @event = (s, e) =>
        {
            ((BottomSheet)s!).Opened -= @event;

            ((BottomSheet)s!).IsOpen = true;

            _ = taskCompletionSource.TrySetResult();
        };

        Opened += @event;

        ShowAsync().AsTask().SafeFireAndForget();

        await taskCompletionSource.Task.ConfigureAwait(true);
    }

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

        Hide();

        await taskCompletionSource.Task.ConfigureAwait(true);
    }

    public void Cancel()
    {
        _eventManager.RaiseEvent(this, EventArgs.Empty, nameof(Canceled));
    }
}
