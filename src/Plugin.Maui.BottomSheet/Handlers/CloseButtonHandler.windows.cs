using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml.Controls;

namespace Plugin.Maui.BottomSheet.Handlers;

/// <summary>
/// A handler for managing a <see cref="CloseButton"/> in a .NET MAUI application.
/// This handler bridges the <see cref="CloseButton"/> with its platform-specific implementation
/// (Windows), handling the mapping of properties and events between the two.
/// </summary>
internal sealed partial class CloseButtonHandler : ViewHandler<CloseButton, Plugin.BottomSheet.Windows.CloseButton>
{
    /// <summary>
    /// Maps the <see cref="CloseButton.TintColor"/> property to the platform-specific implementation.
    /// Sets the background color of the <see cref="CloseButton"/> on the platform.
    /// </summary>
    /// <param name="handler">The handler instance for <see cref="CloseButton"/>.</param>
    /// <param name="closeButton">The <see cref="CloseButton"/> instance.</param>
    public static void MapTintColor(CloseButtonHandler handler, CloseButton closeButton)
    {
        if (closeButton.TintColor is not null)
        {
            handler.PlatformView.Background = closeButton.TintColor?.ToPlatform();
        }
    }

    /// <summary>
    /// Maps the CloseButton.HeightRequest property to the platform-specific implementation.
    /// Sets the height of the <see cref="CloseButton"/> on the platform.
    /// </summary>
    /// <param name="handler">The handler instance for <see cref="CloseButton"/>.</param>
    /// <param name="closeButton">The <see cref="CloseButton"/> instance.</param>
    public static void MapHeightRequest(CloseButtonHandler handler, CloseButton closeButton)
    {
        handler.PlatformView.SetHeight(closeButton.HeightRequest);
    }

    /// <summary>
    /// Maps the WidthRequest property to the platform-specific implementation.
    /// Sets the width of the <see cref="CloseButton"/> on the platform.
    /// </summary>
    /// <param name="handler">The handler instance for <see cref="CloseButton"/>.</param>
    /// <param name="closeButton">The <see cref="CloseButton"/> instance.</param>
    public static void MapWidthRequest(CloseButtonHandler handler, CloseButton closeButton)
    {
        handler.PlatformView.SetWidth(closeButton.WidthRequest);
    }

    /// <summary>
    /// Creates the platform-specific <see cref="Plugin.BottomSheet.Windows.CloseButton"/> for the handler.
    /// </summary>
    /// <returns>A new instance of the platform-specific <see cref="Plugin.BottomSheet.Windows.CloseButton"/>.</returns>
    protected override Plugin.BottomSheet.Windows.CloseButton CreatePlatformView()
    {
        Plugin.BottomSheet.Windows.CloseButton closeButton = new();

        closeButton.UpdateAutomationId(VirtualView);

        return closeButton;
    }

    /// <summary>
    /// Connects the handler to the platform-specific <see cref="Plugin.BottomSheet.Windows.CloseButton"/> view.
    /// </summary>
    /// <param name="platformView">The platform-specific <see cref="Plugin.BottomSheet.Windows.CloseButton"/>.</param>
    protected override void ConnectHandler(Plugin.BottomSheet.Windows.CloseButton platformView)
    {
        base.ConnectHandler(platformView);

        platformView.Click += CloseButtonClicked;
    }

    /// <summary>
    /// Disconnects the handler from the platform-specific <see cref="Plugin.BottomSheet.Windows.CloseButton"/> view.
    /// Unsubscribes from events and removes the button from its parent container.
    /// </summary>
    /// <param name="platformView">The platform-specific <see cref="Plugin.BottomSheet.Windows.CloseButton"/>.</param>
    protected override void DisconnectHandler(Plugin.BottomSheet.Windows.CloseButton platformView)
    {
        base.DisconnectHandler(platformView);

        if (platformView.Parent is Panel panel)
        {
            panel.Children.Remove(platformView);
        }

        platformView.Click -= CloseButtonClicked;
    }

    /// <summary>
    /// Handles the click event of the platform-specific <see cref="Plugin.BottomSheet.Windows.CloseButton"/>.
    /// Triggers the <see cref="CloseButton.Clicked"/> event on the <see cref="CloseButton"/> in MAUI.
    /// </summary>
    /// <param name="sender">The sender of the click event.</param>
    /// <param name="e">The event arguments.</param>
    private void CloseButtonClicked(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        VirtualView.RaiseClicked();
    }
}