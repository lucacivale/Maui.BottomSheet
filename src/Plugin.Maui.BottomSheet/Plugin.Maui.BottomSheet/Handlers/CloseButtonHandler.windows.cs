using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml.Controls;

namespace Plugin.Maui.BottomSheet.Handlers;

internal sealed partial class CloseButtonHandler : ViewHandler<CloseButton, Plugin.BottomSheet.Windows.CloseButton>
{
    public static void MapTintColor(CloseButtonHandler handler, CloseButton closeButton)
    {
        handler.PlatformView.Foreground = closeButton.TintColor?.ToPlatform();
    }

    public static void MapHeightRequest(CloseButtonHandler handler, CloseButton closeButton)
    {
        handler.PlatformView.SetHeight(closeButton.HeightRequest);
    }

    public static void MapWidthRequest(CloseButtonHandler handler, CloseButton closeButton)
    {
        handler.PlatformView.SetWidth(closeButton.WidthRequest);
    }

    protected override Plugin.BottomSheet.Windows.CloseButton CreatePlatformView()
    {
        Plugin.BottomSheet.Windows.CloseButton closeButton = new();

        closeButton.UpdateAutomationId(VirtualView);

        return closeButton;
    }

    protected override void ConnectHandler(Plugin.BottomSheet.Windows.CloseButton platformView)
    {
        base.ConnectHandler(platformView);

        platformView.Click += CloseButtonClicked;
    }

    protected override void DisconnectHandler(Plugin.BottomSheet.Windows.CloseButton platformView)
    {
        base.DisconnectHandler(platformView);

        if (platformView.Parent is Panel panel)
        {
            panel.Children.Remove(platformView);
        }

        platformView.Click -= CloseButtonClicked;
    }

    private void CloseButtonClicked(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        VirtualView.RaiseClicked();
    }
}