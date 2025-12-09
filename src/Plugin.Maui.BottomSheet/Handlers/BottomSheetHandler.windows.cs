namespace Plugin.Maui.BottomSheet.Handlers;

using AsyncAwaitBestPractices;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using Plugin.Maui.BottomSheet.Platform.Windows;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// A handler for managing a <see cref="IBottomSheet"/> in a .NET MAUI application.
/// This handler is responsible for interacting with the platform-specific implementation of the BottomSheet view.
/// </summary>
public sealed partial class BottomSheetHandler : ViewHandler<IBottomSheet, MauiBottomSheet>
{
    /// <summary>
    /// Opens the BottomSheet asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    internal partial Task OpenAsync()
    {
        return PlatformView.OpenAsync(true);
    }

    /// <summary>
    /// Closes the BottomSheet asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    internal partial Task CloseAsync()
    {
        return PlatformView.CloseAsync();
    }

    /// <summary>
    /// Connects the handler to the platform-specific <see cref="MauiBottomSheet"/> view.
    /// </summary>
    /// <param name="platformView">The platform-specific <see cref="MauiBottomSheet"/> instance.</param>
    protected override void ConnectHandler(MauiBottomSheet platformView)
    {
        base.ConnectHandler(platformView);
        platformView.SetView(VirtualView);
    }

    /// <summary>
    /// Creates the platform-specific <see cref="MauiBottomSheet"/> view.
    /// </summary>
    /// <returns>A new instance of <see cref="MauiBottomSheet"/>.</returns>
    /// <exception cref="InvalidOperationException">Thrown if <see cref="MauiContext"/> is null.</exception>
    protected override MauiBottomSheet CreatePlatformView()
    {
        _ = MauiContext ?? throw new InvalidOperationException("MauiContext is null, please check your MauiApplication.");

        MauiBottomSheet bottomSheet = new(MauiContext);

        bottomSheet.UpdateAutomationId(VirtualView);

        return bottomSheet;
    }

    /// <summary>
    /// Disconnects the handler from the platform-specific <see cref="MauiBottomSheet"/> view asynchronously.
    /// </summary>
    /// <param name="platformView">The platform-specific <see cref="MauiBottomSheet"/> instance.</param>
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
    /// Maps the <see cref="IBottomSheet.IsCancelable"/> property to the platform-specific implementation.
    /// </summary>
    /// <param name="handler">The handler instance.</param>
    /// <param name="bottomSheet">The <see cref="IBottomSheet"/> instance.</param>
    private static void MapIsCancelable(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        // Method intentionally left empty.
    }

    /// <summary>
    /// Maps the <see cref="IBottomSheet.IsOpen"/> property to the platform-specific implementation.
    /// </summary>
    /// <param name="handler">The handler instance.</param>
    /// <param name="bottomSheet">The <see cref="IBottomSheet"/> instance.</param>
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
    /// Maps the <see cref="IBottomSheet.IsDraggable"/> property to the platform-specific implementation.
    /// </summary>
    /// <param name="handler">The handler instance.</param>
    /// <param name="bottomSheet">The <see cref="IBottomSheet"/> instance.</param>
    private static void MapIsDraggable(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        // Method intentionally left empty.
    }

    /// <summary>
    /// Maps the <see cref="IBottomSheet.States"/> property to the platform-specific implementation.
    /// </summary>
    /// <param name="handler">The handler instance.</param>
    /// <param name="bottomSheet">The <see cref="IBottomSheet"/> instance.</param>
    private static void MapStates(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        // Method intentionally left empty.
    }

    /// <summary>
    /// Maps the <see cref="IBottomSheet.CurrentState"/> property to the platform-specific implementation.
    /// </summary>
    /// <param name="handler">The handler instance.</param>
    /// <param name="bottomSheet">The <see cref="IBottomSheet"/> instance.</param>
    private static void MapCurrentState(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        // Method intentionally left empty.
    }

    /// <summary>
    /// Maps the <see cref="IBottomSheet.PeekHeight"/> property to the platform-specific implementation.
    /// </summary>
    /// <param name="handler">The handler instance.</param>
    /// <param name="bottomSheet">The <see cref="IBottomSheet"/> instance.</param>
    private static void MapPeekHeight(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        // Method intentionally left empty.
    }

    /// <summary>
    /// Maps the <see cref="IBottomSheet.BackgroundColor"/> property to the platform-specific implementation.
    /// </summary>
    /// <param name="handler">The handler instance.</param>
    /// <param name="bottomSheet">The <see cref="IBottomSheet"/> instance.</param>
    private static void MapBackgroundColor(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        if (handler.PlatformView.IsOpen == false)
        {
            return;
        }

        handler.PlatformView.SetBottomSheetBackgroundColor();
    }

    /// <summary>
    /// Maps the <see cref="IBottomSheet.CornerRadius"/> property to the platform-specific implementation.
    /// </summary>
    /// <param name="handler">The handler instance.</param>
    /// <param name="bottomSheet">The <see cref="IBottomSheet"/> instance.</param>
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
    /// <param name="handler">The handler instance.</param>
    /// <param name="bottomSheet">The <see cref="IBottomSheet"/> instance.</param>
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
    /// </summary>
    /// <param name="handler">The handler instance.</param>
    /// <param name="bottomSheet">The <see cref="IBottomSheet"/> instance.</param>
    private static void MapIsModal(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        // Method intentionally left empty.
    }

    /// <summary>
    /// Handles the cancel action for the BottomSheet.
    /// </summary>
    /// <param name="handler">The handler instance.</param>
    /// <param name="bottomSheet">The <see cref="IBottomSheet"/> instance.</param>
    /// <param name="sender">The sender object.</param>
    private static void MapCancel(BottomSheetHandler handler, IBottomSheet bottomSheet, object? sender)
    {
        handler.PlatformView.Cancel();
    }

    /// <summary>
    /// Maps the min width property of the bottom sheet to the corresponding platform-specific implementation on Android.
    /// </summary>
    /// <param name="handler">The bottom sheet handler instance managing the platform-specific implementation.</param>
    /// <param name="bottomSheet">The bottom sheet interface instance whose min width is being mapped.</param>
    /// <param name="sender">The optional sender triggering this operation, if applicable.</param>
    private static void MapMinWidth(BottomSheetHandler handler, IBottomSheet bottomSheet, object? sender)
    {
        handler.PlatformView.SetMinWidth();
    }

    /// <summary>
    /// Maps the min height property of the bottom sheet to the corresponding platform-specific implementation on Android.
    /// </summary>
    /// <param name="handler">The bottom sheet handler instance managing the platform-specific implementation.</param>
    /// <param name="bottomSheet">The bottom sheet interface instance whose min height is being mapped.</param>
    /// <param name="sender">The optional sender triggering this operation, if applicable.</param>
    private static void MapMinHeight(BottomSheetHandler handler, IBottomSheet bottomSheet, object? sender)
    {
        handler.PlatformView.SetMinHeight();
    }

    /// <summary>
    /// Maps the max width property of the bottom sheet to the corresponding platform-specific implementation on Android.
    /// </summary>
    /// <param name="handler">The bottom sheet handler instance managing the platform-specific implementation.</param>
    /// <param name="bottomSheet">The bottom sheet interface instance whose max width is being mapped.</param>
    /// <param name="sender">The optional sender triggering this operation, if applicable.</param>
    private static void MapMaxWidth(BottomSheetHandler handler, IBottomSheet bottomSheet, object? sender)
    {
        handler.PlatformView.SetMaxWidth();
    }

    /// <summary>
    /// Maps the max height property of the bottom sheet to the corresponding platform-specific implementation on Android.
    /// </summary>
    /// <param name="handler">The bottom sheet handler instance managing the platform-specific implementation.</param>
    /// <param name="bottomSheet">The bottom sheet interface instance whose max height is being mapped.</param>
    /// <param name="sender">The optional sender triggering this operation, if applicable.</param>
    private static void MapMaxHeight(BottomSheetHandler handler, IBottomSheet bottomSheet, object? sender)
    {
        handler.PlatformView.SetMaxHeight();
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
    }
}