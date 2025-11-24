using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using ACloseButton = Plugin.BottomSheet.Android.CloseButton;

namespace Plugin.Maui.BottomSheet.Handlers;

/// <summary>
/// Represents a handler responsible for managing the logic and lifecycle
/// of the <see cref="CloseButton"/> control on Android platforms.
/// It facilitates communication between the cross-platform control and the
/// platform-specific implementation, ensuring proper rendering and behavior.
/// </summary>
internal sealed partial class CloseButtonHandler : ViewHandler<CloseButton, ACloseButton>
{
    /// <summary>
    /// Maps the tint color property from the cross-platform CloseButton control to the native Android CloseButton control.
    /// </summary>
    /// <param name="handler">The handler responsible for translating the cross-platform control to the platform-specific control.</param>
    /// <param name="closeButton">The cross-platform CloseButton control whose property is being mapped.</param>
    public static void MapTintColor(CloseButtonHandler handler, CloseButton closeButton)
    {
        handler.PlatformView.IconTint = closeButton.TintColor?.ToPlatform();
    }

    /// <summary>
    /// Maps the height request from the cross-platform <see cref="CloseButton"/> to the native platform-specific control.
    /// </summary>
    /// <param name="handler">The handler instance that manages the bridge between the native and cross-platform views.</param>
    /// <param name="closeButton">The cross-platform <see cref="CloseButton"/> instance whose height request is being mapped.</param>
    public static void MapHeightRequest(CloseButtonHandler handler, CloseButton closeButton)
    {
        handler.PlatformView.SetHeight(Convert.ToInt32(handler.Context.ToPixels(closeButton.HeightRequest)));
    }

    /// <summary>
    /// Maps the WidthRequest property of the <see cref="CloseButton"/> to the platform-specific width configuration
    /// for the <see cref="Plugin.BottomSheet.Android.CloseButton"/> control.
    /// </summary>
    /// <param name="handler">
    /// The <see cref="CloseButtonHandler"/> that links the MAUI <see cref="CloseButton"/> control
    /// to the platform-specific implementation.
    /// </param>
    /// <param name="closeButton">
    /// The <see cref="CloseButton"/> instance whose WidthRequest property is being mapped to
    /// its platform-specific representation.
    /// </param>
    public static void MapWidthRequest(CloseButtonHandler handler, CloseButton closeButton)
    {
        handler.PlatformView.SetWidth(Convert.ToInt32(handler.Context.ToPixels(closeButton.WidthRequest)));
    }

    /// <summary>
    /// Creates a platform-specific view for the CloseButton control on Android.
    /// </summary>
    /// <remarks>
    /// This method ensures that the platform-specific Android view for the CloseButton
    /// is initialized with the necessary context and configurations. It configures the view
    /// with the appropriate automation properties and applies the default Android-specific
    /// settings for the CloseButton.
    /// </remarks>
    /// <returns>
    /// An instance of <see cref="Plugin.BottomSheet.Android.CloseButton"/> configured for Android.
    /// </returns>
    protected override ACloseButton CreatePlatformView()
    {
        _ = MauiContext?.Context ?? throw new InvalidOperationException("Android Context is null, please check your MauiApplication.");

        ACloseButton closeButton = new(MauiContext.Context);

        closeButton.UpdateAutomationId(VirtualView);

        return closeButton;
    }

    /// <summary>
    /// Establishes event listeners and other connections necessary for the proper functioning
    /// of the platform-specific CloseButton view.
    /// </summary>
    /// <param name="platformView">
    /// The Android-specific <see cref="Plugin.BottomSheet.Android.CloseButton"/> instance
    /// that serves as the rendered button on the platform.
    /// </param>
    protected override void ConnectHandler(ACloseButton platformView)
    {
        base.ConnectHandler(platformView);

        platformView.Click += CloseButtonClicked;
    }

    /// <summary>
    /// Disconnects the handler from the platform-specific view and cleans up resources.
    /// </summary>
    /// <param name="platformView">
    /// The platform-specific <see cref="ACloseButton"/> instance that is currently connected to the handler.
    /// </param>
    /// <remarks>
    /// This method ensures that event handlers are unsubscribed and the view is removed from its parent.
    /// It also calls the base implementation to perform additional cleanup.
    /// </remarks>
    protected override void DisconnectHandler(ACloseButton platformView)
    {
        base.DisconnectHandler(platformView);

        platformView.RemoveFromParent();
        platformView.Click -= CloseButtonClicked;
    }

    /// <summary>
    /// Handles the click event triggered when the close button is clicked.
    /// </summary>
    /// <param name="sender">The source of the event, typically the close button.</param>
    /// <param name="e">An object that contains event data.</param>
    private void CloseButtonClicked(object? sender, EventArgs e)
    {
        VirtualView.RaiseClicked();
    }
}