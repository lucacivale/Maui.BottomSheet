namespace Plugin.Maui.BottomSheet.Handlers;

using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using UIKit;

/// <summary>
/// Handler responsible for managing the native close button view on macOS and iOS platforms,
/// including mapping custom button properties such as <see cref="CloseButton.TintColor"/>.
/// </summary>
internal partial class CloseButtonHandler : ViewHandler<CloseButton, UIButton>
{
    /// <summary>
    /// Maps the <see cref="CloseButton.TintColor"/> property to the underlying native button.
    /// Configures the background color, foreground color, and appearance of the platform button.
    /// </summary>
    /// <param name="handler">The button handler being configured.</param>
    /// <param name="virtualView">The virtual close button view providing property values.</param>
    public static void MapTintColor(CloseButtonHandler handler, CloseButton virtualView)
    {
        if (virtualView.TintColor is not null)
        {
            UIButtonConfiguration config = UIButtonConfiguration.FilledButtonConfiguration;
            config.Background.CornerRadius = 100;
            config.Background.BackgroundColor = virtualView.TintColor?.ToPlatform();
            config.BaseForegroundColor = UIColor.FromRGB(132, 132, 136);

            handler.PlatformView.Configuration = config;
        }
    }

    public static void MapHeightRequest(CloseButtonHandler handler, CloseButton closeButton)
    {
        // Method intentionally left empty.
    }

    public static void MapWidthRequest(CloseButtonHandler handler, CloseButton closeButton)
    {
        // Method intentionally left empty.
    }

    /// <summary>
    /// Creates and returns the native <see cref="UIButton"/> for the close button.
    /// </summary>
    /// <returns>
    /// A new instance of <see cref="UIButton"/> configured as a close button.
    /// </returns>
    protected override UIButton CreatePlatformView()
    {
        UIButton closeButton = new(UIButtonType.Close);

        closeButton.UpdateAutomationId(VirtualView);
        return closeButton;
    }

    protected override void ConnectHandler(UIButton platformView)
    {
        base.ConnectHandler(platformView);

        platformView.TouchUpInside += CloseButtonClicked;
    }

    protected override void DisconnectHandler(UIButton platformView)
    {
        base.DisconnectHandler(platformView);

        platformView.RemoveFromSuperview();
        platformView.TouchUpInside -= CloseButtonClicked;
    }

    private void CloseButtonClicked(object? sender, EventArgs e)
    {
        VirtualView.RaiseClicked();
    }
}