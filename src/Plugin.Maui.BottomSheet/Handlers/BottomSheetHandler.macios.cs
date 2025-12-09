namespace Plugin.Maui.BottomSheet.Handlers;

using AsyncAwaitBestPractices;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using Plugin.Maui.BottomSheet.Platform.MaciOS;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Implements the handler for the <see cref="IBottomSheet"/> interface, enabling bottom sheet functionality on macOS/iOS platforms using MAUI.
/// This class facilitates the interaction between the cross-platform implementation and the platform-specific native components.
/// </summary>
/// <remarks>
/// The <c>BottomSheetHandler</c> is responsible for coordinating the platform-specific behavior of the bottom sheet,
/// ensuring property synchronization, lifecycle management, and interoperation with the native implementation on supported devices.
/// </remarks>
public sealed partial class BottomSheetHandler : ViewHandler<IBottomSheet, MauiBottomSheet>
{
    /// <summary>
    /// Asynchronously opens a bottom sheet using the platform-specific implementation on macOS/iOS.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    internal partial Task OpenAsync()
    {
        return PlatformView.OpenAsync(true);
    }

    /// <summary>
    /// Asynchronously closes a bottom sheet using the platform-specific implementation.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    internal partial Task CloseAsync()
    {
        return PlatformView.CloseAsync();
    }

    /// <summary>
    /// Connects the handler to the associated platform-specific implementation of the bottom sheet component.
    /// </summary>
    /// <param name="platformView">The platform-specific bottom sheet view instance to be connected with the handler.</param>
    protected override void ConnectHandler(MauiBottomSheet platformView)
    {
        base.ConnectHandler(platformView);
        platformView.SetView(VirtualView);
    }

    /// <summary>
    /// Creates a platform-specific view for the bottom sheet component.
    /// </summary>
    /// <returns>
    /// An instance of <see cref="MauiBottomSheet"/> representing the platform-specific bottom sheet implementation.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the <see cref="MauiContext"/> is null or its required properties are not initialized.
    /// </exception>
    protected override MauiBottomSheet CreatePlatformView()
    {
        _ = MauiContext ?? throw new InvalidOperationException("MauiContext is null, please check your MauiApplication.");

        MauiBottomSheet bottomSheet = new(MauiContext);

        bottomSheet.UpdateAutomationId(VirtualView);

        return bottomSheet;
    }

    /// <summary>
    /// Disconnects the handler from the platform-specific view and releases associated resources.
    /// </summary>
    /// <param name="platformView">The platform-specific view associated with the handler.</param>
    [SuppressMessage("Usage", "VSTHRD100: Avoid async void methods", Justification = "Is okay here.")]
    [SuppressMessage("Design", "CA1031: Do not catch general exception types", Justification = "Catch all exceptions to prevent crash.")]
    protected override async void DisconnectHandler(MauiBottomSheet platformView)
    {
        try
        {
            base.DisconnectHandler(platformView);

            await platformView.CloseAsync().ConfigureAwait(true);

            platformView.Cleanup();
        }
        catch (Exception e)
        {
            Trace.TraceError("Disconnecting BottomSheetHandler failed: {0}", e);
        }
    }

    /// <summary>
    /// Maps the IsCancelable property of the bottom sheet, ensuring that the platform-specific view reflects the cancelable behavior of the bottom sheet when it is open.
    /// </summary>
    /// <param name="handler">The handler that manages the platform-specific behavior of the bottom sheet.</param>
    /// <param name="bottomSheet">The bottom sheet instance whose IsCancelable property is being mapped.</param>
    private static void MapIsCancelable(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        if (handler.PlatformView.IsOpen == false)
        {
            return;
        }

        handler.PlatformView.SetIsCancelable();
    }

    /// <summary>
    /// Maps the IsOpen property of the BottomSheet to the platform-specific implementation,
    /// ensuring that the bottom sheet's open state aligns with the desired behavior.
    /// </summary>
    /// <param name="handler">
    /// The <see cref="BottomSheetHandler"/> instance responsible for applying the mapping.
    /// </param>
    /// <param name="bottomSheet">
    /// The <see cref="IBottomSheet"/> instance containing the IsOpen property value to be mapped.
    /// </param>
    private static void MapIsOpen(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        if (handler.IsConnecting
            && bottomSheet.IsOpen == false)
        {
            return;
        }

        handler.PlatformView.SetIsOpenAsync().SafeFireAndForget();
    }

    /// <summary>
    /// Maps the <see cref="IBottomSheet.IsDraggable"/> property to the native platform-specific implementation.
    /// </summary>
    /// <param name="handler">The instance of <see cref="BottomSheetHandler"/> responsible for handling the mapping.</param>
    /// <param name="bottomSheet">The <see cref="IBottomSheet"/> control whose draggable property is being configured.</param>
    private static void MapIsDraggable(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        if (handler.PlatformView.IsOpen == false)
        {
            return;
        }

        handler.PlatformView.SetIsDraggable();
    }

    /// <summary>
    /// Maps the states of the BottomSheet control from the handler to the platform-specific implementation.
    /// </summary>
    /// <param name="handler">The <see cref="BottomSheetHandler"/> responsible for managing the BottomSheet behavior and rendering.</param>
    /// <param name="bottomSheet">The <see cref="IBottomSheet"/> instance containing the state and properties of the BottomSheet.</param>
    private static void MapStates(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        if (handler.PlatformView.IsOpen == false)
        {
            return;
        }

        handler.PlatformView.SetStates();
    }

    /// <summary>
    /// Maps the current state of the bottom sheet using the specified handler and bottom sheet instance.
    /// </summary>
    /// <param name="handler">The <see cref="BottomSheetHandler"/> instance responsible for handling platform-specific behavior for the bottom sheet.</param>
    /// <param name="bottomSheet">The <see cref="IBottomSheet"/> instance whose state is being mapped to the platform-specific implementation.</param>
    private static void MapCurrentState(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        if (handler.PlatformView.IsOpen == false)
        {
            return;
        }

        handler.PlatformView.SetCurrentState();
    }

    /// <summary>
    /// Maps the <c>PeekHeight</c> property of the <see cref="IBottomSheet"/> to the platform-specific implementation.
    /// </summary>
    /// <param name="handler">The handler for the bottom sheet view.</param>
    /// <param name="bottomSheet">The bottom sheet whose properties are being mapped.</param>
    private static void MapPeekHeight(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        handler.PlatformView.SetPeekHeight();
    }

    /// <summary>
    /// Maps the background color property of the bottom sheet.
    /// </summary>
    /// <param name="handler">The handler responsible for managing the bottom sheet.</param>
    /// <param name="bottomSheet">The bottom sheet instance whose background color is being mapped.</param>
    private static void MapBackgroundColor(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        if (handler.PlatformView.IsOpen == false)
        {
            return;
        }

        handler.PlatformView.SetBottomSheetBackgroundColor();
    }

    /// <summary>
    /// Maps the corner radius property to the platform-specific bottom sheet implementation.
    /// Ensures that the corner radius is applied only if the bottom sheet is currently open.
    /// </summary>
    /// <param name="handler">The <see cref="BottomSheetHandler"/> responsible for handling the bottom sheet.</param>
    /// <param name="bottomSheet">The <see cref="IBottomSheet"/> representing the bottom sheet's interface implementation.</param>
    private static void MapCornerRadius(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        if (handler.PlatformView.IsOpen == false)
        {
            return;
        }

        handler.PlatformView.SetCornerRadius();
    }

    /// <summary>
    /// Maps the <see cref="IBottomSheet.WindowBackgroundColor"/> property to the platform-specific implementation.
    /// </summary>
    /// <param name="handler">
    /// The <see cref="BottomSheetHandler"/> responsible for managing the platform-specific implementation of the bottom sheet.
    /// </param>
    /// <param name="bottomSheet">
    /// The <see cref="IBottomSheet"/> instance representing the cross-platform bottom sheet.
    /// </param>
    private static void MapWindowBackgroundColor(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        if (handler.PlatformView.IsOpen == false)
        {
            return;
        }

        handler.PlatformView.SetWindowBackgroundColor();
    }

    /// <summary>
    /// Maps the <see cref="IBottomSheet.IsModal"/> property to the platform-specific implementation.
    /// Ensures the BottomSheet reflects the correct modal behavior when the IsModal property is updated.
    /// </summary>
    /// <param name="handler">The <see cref="BottomSheetHandler"/> responsible for managing the platform-specific BottomSheet control.</param>
    /// <param name="bottomSheet">The <see cref="IBottomSheet"/> instance whose IsModal property has changed.</param>
    private static void MapIsModal(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        if (handler.PlatformView.IsOpen == false)
        {
            return;
        }

        handler.PlatformView.SetIsModal();
    }

    /// <summary>
    /// Handles the cancel operation for the bottom sheet by invoking the platform-specific cancel functionality.
    /// </summary>
    /// <param name="handler">The <see cref="BottomSheetHandler"/> managing the bottom sheet.</param>
    /// <param name="bottomSheet">The <see cref="IBottomSheet"/> instance associated with this operation.</param>
    /// <param name="sender">The sender initiating the cancel operation, or null if not applicable.</param>
    private static void MapCancel(BottomSheetHandler handler, IBottomSheet bottomSheet, object? sender)
    {
        handler.PlatformView.Cancel();
    }

    /// <summary>
    /// Maps the <c>SizeMode</c> property of the <see cref="IBottomSheet"/> to the platform-specific implementation.
    /// </summary>
    /// <param name="handler">The <see cref="BottomSheetHandler"/> responsible for managing the platform-specific view representation.</param>
    /// <param name="bottomSheet">The <see cref="IBottomSheet"/> whose <c>SizeMode</c> is being mapped.</param>
    private static void MapSizeMode(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        if (handler.PlatformView.IsOpen == false)
        {
            return;
        }

        handler.PlatformView.SetSizeMode();
    }
}