using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Maui.Platform;

namespace Plugin.Maui.BottomSheet.Handlers;

using AsyncAwaitBestPractices;
using Microsoft.Maui.Handlers;
using Plugin.Maui.BottomSheet;
using Plugin.Maui.BottomSheet.Platform.Android;

/// <summary>
/// Represents a handler for managing the behavior and lifecycle of a bottom sheet view on Android.
/// </summary>
internal sealed partial class BottomSheetHandler : ViewHandler<IBottomSheet, MauiBottomSheet>
{
    /// <summary>
    /// Asynchronously opens a bottom sheet using the platform-specific implementation.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    internal partial Task OpenAsync()
    {
        return PlatformView.OpenAsync(true);
    }

    /// <summary>
    /// Closes the bottom sheet asynchronously.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    internal partial Task CloseAsync()
    {
        return PlatformView.CloseAsync();
    }

    /// <summary>
    /// Connects the handler to the associated platform-specific <see cref="MauiBottomSheet"/>.
    /// </summary>
    /// <param name="platformView">The platform-specific bottom sheet view to connect the handler with.</param>
    protected override void ConnectHandler(MauiBottomSheet platformView)
    {
        base.ConnectHandler(platformView);
        platformView.SetView(VirtualView);
    }

    /// <summary>
    /// Creates a new instance of the platform-specific bottom sheet view.
    /// </summary>
    /// <returns>
    /// A new instance of <see cref="MauiBottomSheet"/> representing the platform-specific implementation of the bottom sheet.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the <see cref="MauiContext"/> or its Android <see cref="MauiContext.Context"/> is null.
    /// </exception>
    protected override MauiBottomSheet CreatePlatformView()
    {
        _ = MauiContext ?? throw new InvalidOperationException("MauiContext is null, please check your MauiApplication.");
        _ = MauiContext.Context ?? throw new InvalidOperationException("Android Context is null, please check your MauiApplication.");

        var bottomSheet = new MauiBottomSheet(MauiContext, MauiContext.Context);

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
    /// Maps the IsCancelable property of the bottom sheet, ensuring that the platform-specific view reflects this property when the bottom sheet is open.
    /// </summary>
    /// <param name="handler">The <see cref="BottomSheetHandler"/> that manages the bottom sheet.</param>
    /// <param name="bottomSheet">The <see cref="IBottomSheet"/> instance representing the bottom sheet element.</param>
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
    /// ensuring that the bottom sheet's state corresponds to the desired behavior.
    /// </summary>
    /// <param name="handler">
    /// The <see cref="BottomSheetHandler"/> instance handling the platform-specific behavior.
    /// </param>
    /// <param name="bottomSheet">
    /// The <see cref="IBottomSheet"/> instance providing the IsOpen property value.
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
    /// Maps the <see cref="IBottomSheet.IsDraggable"/> property to the native platform view.
    /// </summary>
    /// <param name="handler">The BottomSheetHandler handling the mapping of the property.</param>
    /// <param name="bottomSheet">The BottomSheet control whose property is being mapped.</param>
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
    /// <param name="handler">The handler associated with the BottomSheet.</param>
    /// <param name="bottomSheet">The BottomSheet instance to map the states for.</param>
    private static void MapStates(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        if (handler.PlatformView.IsOpen == false)
        {
            return;
        }

        handler.PlatformView.SetStates();
    }

    /// <summary>
    /// Maps the current state of the bottom sheet using the given handler and bottom sheet instance.
    /// </summary>
    /// <param name="handler">The handler responsible for managing the platform-specific implementation of the bottom sheet.</param>
    /// <param name="bottomSheet">The bottom sheet instance whose current state needs to be mapped.</param>
    private static void MapCurrentState(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        if (handler.PlatformView.IsOpen == false)
        {
            return;
        }

        handler.PlatformView.SetCurrentState();
    }

    /// Maps the `PeekHeight` property of the `IBottomSheet` to the platform-specific implementation.
    /// <param name="handler">The handler for the bottom sheet view.</param>
    /// <param name="bottomSheet">The bottom sheet whose properties are being mapped.</param>
    private static void MapPeekHeight(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        if (handler.PlatformView.IsOpen == false)
        {
            return;
        }

        handler.PlatformView.SetPeekHeight();
    }

    /// <summary>
    /// Maps the background color property of the bottom sheet.
    /// </summary>
    /// <param name="handler">The handler for the bottom sheet.</param>
    /// <param name="bottomSheet">The bottom sheet whose background color property is to be mapped.</param>
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
    /// Maps the <see cref="IBottomSheet.IsModal"/> property to the platform-specific implementation on Android.
    /// Ensures that the BottomSheet is updated to reflect whether it should behave modally or not.
    /// This method is invoked whenever the IsModal property changes.
    /// </summary>
    /// <param name="handler">The <see cref="BottomSheetHandler"/> responsible for managing the BottomSheet control.</param>
    /// <param name="bottomSheet">The <see cref="IBottomSheet"/> instance containing the updated IsModal property value.</param>
    private static void MapIsModal(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        if (handler.PlatformView.IsOpen == false)
        {
            return;
        }

        handler.PlatformView.SetIsModal();
    }

    private static void MapCancel(BottomSheetHandler handler, IBottomSheet bottomSheet, object? sender)
    {
        handler.PlatformView.Cancel();
    }

    private static void MapMargin(BottomSheetHandler handler, IBottomSheet bottomSheet, object? sender)
    {
        handler.PlatformView.SetMargin();
    }

    private static void MapHalfExpandedRatio(BottomSheetHandler handler, IBottomSheet bottomSheet, object? sender)
    {
        handler.PlatformView.SetHalfExpandedRatio();
    }
}