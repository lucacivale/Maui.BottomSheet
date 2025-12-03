namespace Plugin.Maui.BottomSheet.Handlers;

using AsyncAwaitBestPractices;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using Plugin.Maui.BottomSheet.Platform.Windows;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

internal sealed partial class BottomSheetHandler : ViewHandler<IBottomSheet, MauiBottomSheet>
{
    internal partial Task OpenAsync()
    {
        return PlatformView.OpenAsync(true);
    }

    internal partial Task CloseAsync()
    {
        return PlatformView.CloseAsync();
    }

    protected override void ConnectHandler(MauiBottomSheet platformView)
    {
        base.ConnectHandler(platformView);
        platformView.SetView(VirtualView);
    }

    protected override MauiBottomSheet CreatePlatformView()
    {
        _ = MauiContext ?? throw new InvalidOperationException("MauiContext is null, please check your MauiApplication.");

        MauiBottomSheet bottomSheet = new(MauiContext);

        bottomSheet.UpdateAutomationId(VirtualView);

        return bottomSheet;
    }

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

    private static void MapIsCancelable(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        //if (handler.PlatformView.IsOpen == false)
        //{
        //    return;
        //}
        //
        //handler.PlatformView.SetIsCancelable();
    }

    private static void MapIsOpen(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        if (handler.IsConnecting
            && bottomSheet.IsOpen == false)
        {
            return;
        }

        handler.PlatformView.SetIsOpenAsync().SafeFireAndForget();
    }

    private static void MapIsDraggable(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        if (handler.PlatformView.IsOpen == false)
        {
            return;
        }

        handler.PlatformView.SetIsDraggable();
    }

    private static void MapStates(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        //if (handler.PlatformView.IsOpen == false)
        //{
        //    return;
        //}
        //
        //handler.PlatformView.SetStates();
    }

    private static void MapCurrentState(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        //if (handler.PlatformView.IsOpen == false)
        //{
        //    return;
        //}
        //
        //handler.PlatformView.SetCurrentState();
    }

    private static void MapPeekHeight(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        //handler.PlatformView.SetPeekHeight();
    }

    private static void MapBackgroundColor(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        if (handler.PlatformView.IsOpen == false)
        {
            return;
        }

        handler.PlatformView.SetBottomSheetBackgroundColor();
    }

    private static void MapCornerRadius(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        if (handler.PlatformView.IsOpen == false)
        {
            return;
        }

        handler.PlatformView.SetCornerRadius();
    }

    private static void MapWindowBackgroundColor(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        if (handler.PlatformView.IsOpen == false)
        {
            return;
        }

        handler.PlatformView.SetWindowBackgroundColor();
    }

    private static void MapIsModal(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        //if (handler.PlatformView.IsOpen == false)
        //{
        //    return;
        //}
        //
        //handler.PlatformView.SetIsModal();
    }

    private static void MapCancel(BottomSheetHandler handler, IBottomSheet bottomSheet, object? sender)
    {
        handler.PlatformView.Cancel();
    }
}