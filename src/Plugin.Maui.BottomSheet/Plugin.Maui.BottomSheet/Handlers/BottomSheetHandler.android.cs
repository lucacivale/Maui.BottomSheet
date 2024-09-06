namespace Plugin.Maui.BottomSheet.Handlers;

using Microsoft.Maui.Handlers;
using Plugin.Maui.BottomSheet;
// ReSharper disable once RedundantNameQualifier
using Plugin.Maui.BottomSheet.PlatformViews;

public sealed partial class BottomSheetHandler : ViewHandler<IBottomSheet, MauiBottomSheet>
{
    protected override void ConnectHandler(MauiBottomSheet platformView)
    {
        base.ConnectHandler(platformView);
        platformView.SetView(VirtualView);
    }

    protected override MauiBottomSheet CreatePlatformView()
    {
        _ = MauiContext ?? throw new InvalidOperationException("MauiContext is null, please check your MauiApplication.");
        _ = MauiContext.Context ?? throw new InvalidOperationException("Android Context is null, please check your MauiApplication.");

        return new MauiBottomSheet(MauiContext, MauiContext.Context);
    }

    protected override void DisconnectHandler(MauiBottomSheet platformView)
    {
        base.DisconnectHandler(platformView);

        platformView.Cleanup();
    }

    private static void MapIsCancelable(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        handler.PlatformView.SetIsCancelable();
    }

    private static void MapHasHandle(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        handler.PlatformView.SetHasHandle();
    }

    private static void MapShowHeader(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        handler.PlatformView.SetHeader();
    }

    private static void MapIsOpen(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        handler.PlatformView.SetIsOpen();
    }

    private static void MapIsDraggable(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        handler.PlatformView.SetIsDraggable();
    }

    private static void MapHeader(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        handler.PlatformView.SetHeader();
    }

    private static void MapStates(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        handler.PlatformView.SetStates();
    }
    
    private static void MapCurrentState(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        handler.PlatformView.SetCurrentState();
    }

    private static void MapPeek(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        handler.PlatformView.SetPeek();
    }

    private static void MapContentTemplate(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        handler.PlatformView.SetContentTemplate();
    }
}