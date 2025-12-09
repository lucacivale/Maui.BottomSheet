using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Maui.Platform;

namespace Plugin.Maui.BottomSheet.Handlers;

using AsyncAwaitBestPractices;
using Microsoft.Maui.Handlers;
using Plugin.Maui.BottomSheet;
using Plugin.Maui.BottomSheet.Platform.Android;

/// <summary>
/// Handles the platform-specific behavior and lifecycle of a bottom sheet view within the Android platform.
/// </summary>
public sealed partial class BottomSheetHandler : ViewHandler<IBottomSheet, MauiBottomSheet>
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
    /// Asynchronously closes the bottom sheet using the platform-specific implementation.
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
        _ = MauiContext ??
            throw new InvalidOperationException("MauiContext is null, please check your MauiApplication.");
        _ = MauiContext.Context ??
            throw new InvalidOperationException("Android Context is null, please check your MauiApplication.");

        MauiBottomSheet bottomSheet = new(MauiContext, MauiContext.Context);

        bottomSheet.UpdateAutomationId(VirtualView);

        return bottomSheet;
    }

    /// <summary>
    /// Disconnects the handler from the platform-specific bottom sheet view and releases associated resources.
    /// </summary>
    /// <param name="platformView">The platform-specific bottom sheet view associated with the handler.</param>
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
    /// Maps the IsCancelable property of the bottom sheet, ensuring that the platform-specific view
    /// reflects this property when the bottom sheet is open.
    /// </summary>
    /// <param name="handler">The <see cref="BottomSheetHandler"/> instance managing the bottom sheet.</param>
    /// <param name="bottomSheet">The <see cref="IBottomSheet"/> instance representing the bottom sheet component.</param>
    private static void MapIsCancelable(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        if (handler.PlatformView.IsOpen == false)
        {
            return;
        }

        handler.PlatformView.SetIsCancelable();
    }

    /// <summary>
    /// Maps the <see cref="IBottomSheet.IsOpen"/> property to the platform-specific implementation,
    /// ensuring that the bottom sheet's open or closed state is correctly reflected on the platform.
    /// </summary>
    /// <param name="handler">
    /// The <see cref="BottomSheetHandler"/> instance responsible for managing the bottom sheet's visual state.
    /// </param>
    /// <param name="bottomSheet">
    /// The <see cref="IBottomSheet"/> instance containing the IsOpen property's value to be applied.
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
    /// Maps the <see cref="IBottomSheet.IsDraggable"/> property to the native platform view, allowing
    /// configuration of whether the bottom sheet can be dragged by the user.
    /// </summary>
    /// <param name="handler">The <see cref="BottomSheetHandler"/> responsible for coordinating the mapping process.</param>
    /// <param name="bottomSheet">The <see cref="IBottomSheet"/> instance for which the property is being applied.</param>
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
    /// Maps the current state of the bottom sheet using the specified handler and bottom sheet instance.
    /// </summary>
    /// <param name="handler">The handler managing the platform-specific implementation of the bottom sheet.</param>
    /// <param name="bottomSheet">The bottom sheet instance whose state is to be synchronized with the platform view.</param>
    private static void MapCurrentState(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        if (handler.PlatformView.IsOpen == false)
        {
            return;
        }

        handler.PlatformView.SetCurrentState();
    }

    /// <summary>
    /// Maps the `PeekHeight` property of the `IBottomSheet` to the platform-specific implementation.
    /// </summary>
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
    /// Maps the corner radius property to the platform-specific implementation of the bottom sheet.
    /// Ensures the corner radius is updated only when the bottom sheet is currently open.
    /// </summary>
    /// <param name="handler">The <see cref="BottomSheetHandler"/> that manages the platform-specific bottom sheet logic.</param>
    /// <param name="bottomSheet">The <see cref="IBottomSheet"/> instance containing the corner radius value to apply.</param>
    private static void MapCornerRadius(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        if (handler.PlatformView.IsOpen == false)
        {
            return;
        }

        handler.PlatformView.SetCornerRadius();
    }

    /// <summary>
    /// Maps the <see cref="IBottomSheet.WindowBackgroundColor"/> property to the platform-specific background color implementation.
    /// </summary>
    /// <param name="handler">
    /// The <see cref="BottomSheetHandler"/> responsible for handling the platform-specific logic for the bottom sheet.
    /// </param>
    /// <param name="bottomSheet">
    /// The <see cref="IBottomSheet"/> instance defining the properties of the cross-platform bottom sheet.
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
    /// Maps the <see cref="IBottomSheet.IsModal"/> property to the platform-specific behavior.
    /// Updates the BottomSheet to reflect the modal behavior specified by the IsModal property.
    /// This method is called when the IsModal property is changed.
    /// </summary>
    /// <param name="handler">The <see cref="BottomSheetHandler"/> instance that manages the platform-specific implementation.</param>
    /// <param name="bottomSheet">The <see cref="IBottomSheet"/> instance containing the updated IsModal property value.</param>
    private static void MapIsModal(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        if (handler.PlatformView.IsOpen == false)
        {
            return;
        }

        handler.PlatformView.SetIsModal();
    }

    /// <summary>
    /// Maps the cancel command of the bottom sheet to the platform-specific implementation.
    /// </summary>
    /// <param name="handler">The <see cref="BottomSheetHandler"/> managing the bottom sheet.</param>
    /// <param name="bottomSheet">The <see cref="IBottomSheet"/> instance representing the bottom sheet.</param>
    /// <param name="sender">The object that triggered the cancel command, if applicable.</param>
    private static void MapCancel(BottomSheetHandler handler, IBottomSheet bottomSheet, object? sender)
    {
        handler.PlatformView.Cancel();
    }

    /// <summary>
    /// Maps the margin property of the bottom sheet to the corresponding platform-specific implementation on Android.
    /// </summary>
    /// <param name="handler">The bottom sheet handler instance managing the platform-specific implementation.</param>
    /// <param name="bottomSheet">The bottom sheet interface instance whose margin is being mapped.</param>
    /// <param name="sender">The optional sender triggering this operation, if applicable.</param>
    private static void MapMargin(BottomSheetHandler handler, IBottomSheet bottomSheet, object? sender)
    {
        handler.PlatformView.SetMargin();
    }

    /// <summary>
    /// Maps the half-expanded ratio property of the bottom sheet to the platform-specific implementation on Android.
    /// </summary>
    /// <param name="handler">The <see cref="BottomSheetHandler"/> responsible for handling the bottom sheet operations.</param>
    /// <param name="bottomSheet">The <see cref="IBottomSheet"/> instance representing the bottom sheet being operated on.</param>
    /// <param name="sender">An optional parameter representing the sender of the command, if applicable.</param>
    private static void MapHalfExpandedRatio(BottomSheetHandler handler, IBottomSheet bottomSheet, object? sender)
    {
        handler.PlatformView.SetHalfExpandedRatio();
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