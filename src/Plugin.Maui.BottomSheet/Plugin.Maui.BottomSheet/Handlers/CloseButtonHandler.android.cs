using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using ACloseButton = Plugin.BottomSheet.Android.CloseButton;

namespace Plugin.Maui.BottomSheet.Handlers;

internal sealed partial class CloseButtonHandler : ViewHandler<CloseButton, ACloseButton>
{
    public static void MapTintColor(CloseButtonHandler handler, CloseButton closeButton)
    {
        handler.PlatformView.IconTint = closeButton.TintColor?.ToPlatform();
    }

    public static void MapHeightRequest(CloseButtonHandler handler, CloseButton closeButton)
    {
        handler.PlatformView.SetHeight(Convert.ToInt32(handler.Context.ToPixels(closeButton.HeightRequest)));
    }

    public static void MapWidthRequest(CloseButtonHandler handler, CloseButton closeButton)
    {
        handler.PlatformView.SetWidth(Convert.ToInt32(handler.Context.ToPixels(closeButton.WidthRequest)));
    }

    protected override ACloseButton CreatePlatformView()
    {
        _ = MauiContext?.Context ?? throw new InvalidOperationException("Android Context is null, please check your MauiApplication.");

        var closeButton = new ACloseButton(MauiContext.Context);

        closeButton.UpdateAutomationId(VirtualView);

        return closeButton;
    }

    protected override void ConnectHandler(ACloseButton platformView)
    {
        base.ConnectHandler(platformView);

        platformView.Click += CloseButtonClicked;
    }

    protected override void DisconnectHandler(ACloseButton platformView)
    {
        base.DisconnectHandler(platformView);

        platformView.RemoveFromParent();
        platformView.Click -= CloseButtonClicked;
    }

    private void CloseButtonClicked(object? sender, EventArgs e)
    {
        VirtualView.RaiseClicked();
    }
}