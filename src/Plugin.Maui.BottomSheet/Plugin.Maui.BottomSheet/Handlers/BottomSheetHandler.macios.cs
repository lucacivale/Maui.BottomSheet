using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Plugin.Maui.BottomSheet.Handlers;

using AsyncAwaitBestPractices;
using Microsoft.Maui.Handlers;

/// <summary>
/// Provides a handler implementation for the <see cref="IBottomSheet"/> interface on macOS/iOS platforms using MAUI.
/// This handler manages platform-specific logic and property mappings required for displaying and operating a bottom sheet component.
/// </summary>
/// <remarks>
/// The <c>BottomSheetHandler</c> manages the lifecycle, property synchronization, and feature integration
/// with the native <see cref="Plugin.Maui.BottomSheet.Platform.MaciOS"/> view, interfacing between cross-platform and platform-specific code.
/// </remarks>
internal sealed partial class BottomSheetHandler : ViewHandler<IBottomSheet, Platform.MaciOS.MauiBottomSheet>
{
    /// <summary>
    /// Asynchronously opens the bottom sheet on the platform-specific view.
    /// </summary>
    /// <returns>A task that represents the asynchronous open operation.</returns>
    internal partial Task OpenAsync()
    {
        return PlatformView.OpenAsync();
    }

    /// <summary>
    /// Asynchronously closes the bottom sheet on the platform-specific view.
    /// </summary>
    /// <returns>A task that represents the asynchronous close operation.</returns>
    internal partial Task CloseAsync()
    {
        return PlatformView.CloseAsync();
    }

    /// <summary>
    /// Connects the handler to the platform-specific bottom sheet view and sets the associated view.
    /// </summary>
    /// <param name="platformView">The platform-specific bottom sheet view.</param>
    protected override void ConnectHandler(Platform.MaciOS.MauiBottomSheet platformView)
    {
        base.ConnectHandler(platformView);
        platformView.SetView(VirtualView);
    }

    /// <summary>
    /// Creates a new instance of the platform-specific bottom sheet view.
    /// </summary>
    /// <returns>A new platform-specific <see cref="Plugin.Maui.BottomSheet.Platform.MaciOS.MauiBottomSheet"/> instance.</returns>
    /// <exception cref="InvalidOperationException">Thrown if <c>MauiContext</c> is null.</exception>
    protected override Platform.MaciOS.MauiBottomSheet CreatePlatformView()
    {
        _ = MauiContext ?? throw new InvalidOperationException("MauiContext is null, please check your MauiApplication.");
        return new Platform.MaciOS.MauiBottomSheet(MauiContext);
    }

    /// <summary>
    /// Disconnects the handler from the platform-specific bottom sheet view and performs cleanup.
    /// </summary>
    /// <param name="platformView">The platform-specific bottom sheet view.</param>
    [SuppressMessage("Usage", "VSTHRD100: Avoid async void methods, because any exceptions not handled by the method will crash the process", Justification = "Exceptions are handled by the method.")]
    [SuppressMessage("Design", "CA1031: Do not catch general exception types", Justification = "Catch all exceptions to prevent crash.")]
    protected async override void DisconnectHandler(Platform.MaciOS.MauiBottomSheet platformView)
    {
        base.DisconnectHandler(platformView);

        try
        {
            await platformView.CleanupAsync().ConfigureAwait(true);
        }
        catch (Exception e)
        {
            Trace.TraceError($"Error while cleaning up bottom sheet: {e}");
        }
    }

    /// <summary>
    /// Maps the <c>IsCancelable</c> property to the platform-specific handler.
    /// </summary>
    /// <param name="handler">The bottom sheet handler.</param>
    /// <param name="bottomSheet">The bottom sheet interface.</param>
    private static void MapIsCancelable(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        if (handler.PlatformView.IsOpen == false)
        {
            return;
        }

        handler.PlatformView.SetIsCancelable();
    }

    /// <summary>
    /// Maps the <c>HasHandle</c> property to the platform-specific handler.
    /// </summary>
    /// <param name="handler">The bottom sheet handler.</param>
    /// <param name="bottomSheet">The bottom sheet interface.</param>
    private static void MapHasHandle(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        if (handler.PlatformView.IsOpen == false)
        {
            return;
        }

        handler.PlatformView.SetHasHandle();
    }

    /// <summary>
    /// Maps the <c>ShowHeader</c> property to the platform-specific handler.
    /// </summary>
    /// <param name="handler">The bottom sheet handler.</param>
    /// <param name="bottomSheet">The bottom sheet interface.</param>
    private static void MapShowHeader(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        if (handler.PlatformView.IsOpen == false)
        {
            return;
        }

        handler.PlatformView.SetShowHeader();
    }

    /// <summary>
    /// Maps the <c>IsOpen</c> property to the platform-specific handler.
    /// </summary>
    /// <param name="handler">The bottom sheet handler.</param>
    /// <param name="bottomSheet">The bottom sheet interface.</param>
    private static void MapIsOpen(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        if (handler.IsConnecting && bottomSheet.IsOpen == false)
        {
            return;
        }

        handler.PlatformView.SetIsOpenAsync().SafeFireAndForget(continueOnCapturedContext: false);
    }

    /// <summary>
    /// Maps the <c>IsDraggable</c> property to the platform-specific handler.
    /// </summary>
    /// <param name="handler">The bottom sheet handler.</param>
    /// <param name="bottomSheet">The bottom sheet interface.</param>
    private static void MapIsDraggable(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        if (handler.PlatformView.IsOpen == false)
        {
            return;
        }

        handler.PlatformView.SetIsDraggable();
    }

    /// <summary>
    /// Maps the <c>Header</c> property to the platform-specific handler.
    /// </summary>
    /// <param name="handler">The bottom sheet handler.</param>
    /// <param name="bottomSheet">The bottom sheet interface.</param>
    private static void MapHeader(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        if (handler.PlatformView.IsOpen == false)
        {
            return;
        }

        handler.PlatformView.SetHeader();
    }

    /// <summary>
    /// Maps the <c>States</c> property to the platform-specific handler.
    /// </summary>
    /// <param name="handler">The bottom sheet handler.</param>
    /// <param name="bottomSheet">The bottom sheet interface.</param>
    private static void MapStates(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        if (handler.PlatformView.IsOpen == false)
        {
            return;
        }

        handler.PlatformView.SetStates();
    }

    /// <summary>
    /// Maps the <c>CurrentState</c> property to the platform-specific handler.
    /// </summary>
    /// <param name="handler">The bottom sheet handler.</param>
    /// <param name="bottomSheet">The bottom sheet interface.</param>
    private static void MapCurrentState(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        if (handler.PlatformView.IsOpen == false)
        {
            return;
        }

        handler.PlatformView.SetCurrentState();
    }

    /// <summary>
    /// Maps the <c>PeekHeight</c> property to the platform-specific handler. Always sets the peek height.
    /// </summary>
    /// <param name="handler">The bottom sheet handler.</param>
    /// <param name="bottomSheet">The bottom sheet interface.</param>
    private static void MapPeekHeight(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        // Always set peek height!
        handler.PlatformView.SetPeekHeight();
    }

    /// <summary>
    /// Maps the <c>Content</c> property to the platform-specific handler.
    /// </summary>
    /// <param name="handler">The bottom sheet handler.</param>
    /// <param name="bottomSheet">The bottom sheet interface.</param>
    private static void MapContent(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        if (handler.PlatformView.IsOpen == false)
        {
            return;
        }

        handler.PlatformView.SetContent();
    }

    /// <summary>
    /// Maps the <c>Padding</c> property to the platform-specific handler.
    /// </summary>
    /// <param name="handler">The bottom sheet handler.</param>
    /// <param name="bottomSheet">The bottom sheet interface.</param>
    private static void MapPadding(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        if (handler.PlatformView.IsOpen == false)
        {
            return;
        }

        handler.PlatformView.SetPadding();
    }

    /// <summary>
    /// Maps the <c>BackgroundColor</c> property to the platform-specific handler.
    /// </summary>
    /// <param name="handler">The bottom sheet handler.</param>
    /// <param name="bottomSheet">The bottom sheet interface.</param>
    private static void MapBackgroundColor(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        if (handler.PlatformView.IsOpen == false)
        {
            return;
        }

        handler.PlatformView.SetBottomSheetBackgroundColor();
    }

    /// <summary>
    /// Maps the <c>IgnoreSafeArea</c> property to the platform-specific handler.
    /// </summary>
    /// <param name="handler">The bottom sheet handler.</param>
    /// <param name="bottomSheet">The bottom sheet interface.</param>
    private static void MapIgnoreSafeArea(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        if (handler.PlatformView.IsOpen == false)
        {
            return;
        }

        handler.PlatformView.SetIgnoreSafeArea();
    }

    /// <summary>
    /// Maps the <c>CornerRadius</c> property to the platform-specific handler.
    /// </summary>
    /// <param name="handler">The bottom sheet handler.</param>
    /// <param name="bottomSheet">The bottom sheet interface.</param>
    private static void MapCornerRadius(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        if (handler.PlatformView.IsOpen == false)
        {
            return;
        }

        handler.PlatformView.SetCornerRadius();
    }

    /// <summary>
    /// Maps the <c>WindowBackgroundColor</c> property to the platform-specific handler.
    /// </summary>
    /// <param name="handler">The bottom sheet handler.</param>
    /// <param name="bottomSheet">The bottom sheet interface.</param>
    private static void MapWindowBackgroundColor(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        if (handler.PlatformView.IsOpen == false)
        {
            return;
        }

        handler.PlatformView.SetWindowBackgroundColor();
    }

    /// <summary>
    /// Maps the <c>IsModal</c> property to the platform-specific handler.
    /// </summary>
    /// <param name="handler">The bottom sheet handler.</param>
    /// <param name="bottomSheet">The bottom sheet interface.</param>
    private static void MapIsModal(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        if (handler.PlatformView.IsOpen == false)
        {
            return;
        }

        handler.PlatformView.SetIsModal();
    }

    /// <summary>
    /// Maps the <c>BottomSheetStyle</c> property to the platform-specific handler.
    /// </summary>
    /// <param name="handler">The bottom sheet handler.</param>
    /// <param name="bottomSheet">The bottom sheet interface.</param>
    private static void MapBottomSheetStyle(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        if (handler.PlatformView.IsOpen == false)
        {
            return;
        }

        handler.PlatformView.SetBottomSheetStyle();
    }
    
    private static void MapCancel(BottomSheetHandler handler, IBottomSheet bottomSheet, object? sender)
    {

    }
}