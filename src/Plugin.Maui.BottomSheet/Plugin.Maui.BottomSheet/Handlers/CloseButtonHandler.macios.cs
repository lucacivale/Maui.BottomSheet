namespace Plugin.Maui.BottomSheet.Handlers;

using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using UIKit;

/// <summary>
/// A handler implementation for managing the behavior and appearance of the native close button
/// on macOS and iOS platforms. This class provides platform-specific mappings for properties
/// defined in the <see cref="CloseButton"/> view, such as TintColor, height, and width.
/// Utilizes property mappers to enable seamless property updates and enforces behavior specific
/// to the native UIButton implementation.
/// </summary>
internal partial class CloseButtonHandler : ViewHandler<CloseButton, UIButton>
{
    /// <summary>
    /// Maps the <see cref="CloseButton.TintColor"/> property to the native platform-specific button.
    /// Updates the appearance of the button, including background color and foreground color,
    /// to reflect the specified tint color from the virtual view.
    /// </summary>
    /// <param name="handler">The handler associated with the close button.</param>
    /// <param name="virtualView">The virtual view providing the TintColor property value.</param>
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

    /// <summary>
    /// Maps the height request of the <see cref="CloseButton"/> view to the underlying native button.
    /// This method ensures that the platform-specific height configuration reflects the requested height
    /// specified within the virtual view.
    /// </summary>
    /// <param name="handler">The close button handler responsible for managing the platform view configuration.</param>
    /// <param name="closeButton">The virtual close button view that specifies the height property value.</param>
    public static void MapHeightRequest(CloseButtonHandler handler, CloseButton closeButton)
    {
        // Method intentionally left empty.
    }

    /// <summary>
    /// Maps the <see cref="VisualElement.WidthRequest"/> property to the underlying native button.
    /// Configures the width of the platform-specific close button based on the virtual close button view.
    /// </summary>
    /// <param name="handler">The button handler being configured.</param>
    /// <param name="closeButton">The virtual close button view providing property values.</param>
    public static void MapWidthRequest(CloseButtonHandler handler, CloseButton closeButton)
    {
        // Method intentionally left empty.
    }

    /// <summary>
    /// Creates and initializes the native platform-specific close button view.
    /// </summary>
    /// <returns>
    /// An instance of <see cref="UIButton"/> configured as a close button with appropriate properties and behavior.
    /// </returns>
    protected override UIButton CreatePlatformView()
    {
        UIButton closeButton = new(UIButtonType.Close);

        closeButton.UpdateAutomationId(VirtualView);
        return closeButton;
    }

    /// <summary>
    /// Initializes the handler and attaches events or functionality to the native platform view.
    /// This method is invoked when the handler is connected to the virtual view and a platform-specific
    /// view is being prepared for display.
    /// </summary>
    /// <param name="platformView">The native <see cref="UIButton"/> instance representing the platform-specific close button.</param>
    protected override void ConnectHandler(UIButton platformView)
    {
        base.ConnectHandler(platformView);

        platformView.TouchUpInside += CloseButtonClicked;
    }

    /// <summary>
    /// Disconnects the handler from the underlying native <see cref="UIButton"/>.
    /// Removes event subscriptions and cleans up the native button from the view hierarchy.
    /// </summary>
    /// <param name="platformView">The native platform view being managed by the handler.</param>
    protected override void DisconnectHandler(UIButton platformView)
    {
        base.DisconnectHandler(platformView);

        platformView.RemoveFromSuperview();
        platformView.TouchUpInside -= CloseButtonClicked;
    }

    /// <summary>
    /// Handles the click event for the native close button and raises the corresponding event on the virtual <see cref="CloseButton"/> view.
    /// </summary>
    /// <param name="sender">The source of the event, typically the native <see cref="UIButton"/>.</param>
    /// <param name="e">An instance of <see cref="EventArgs"/> providing event data.</param>
    private void CloseButtonClicked(object? sender, EventArgs e)
    {
        VirtualView.RaiseClicked();
    }
}