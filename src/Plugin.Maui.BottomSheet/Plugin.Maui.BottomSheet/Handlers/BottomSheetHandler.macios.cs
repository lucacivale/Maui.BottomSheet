namespace Plugin.Maui.BottomSheet.Handlers;

using AsyncAwaitBestPractices;
using Microsoft.Maui.Handlers;

/// <summary>
/// <see cref="IBottomSheet"/> handler.
/// </summary>
internal sealed partial class BottomSheetHandler : ViewHandler<IBottomSheet, Platform.MaciOS.MauiBottomSheet>
{
    /// <inheritdoc/>
    protected override void ConnectHandler(Platform.MaciOS.MauiBottomSheet platformView)
    {
        base.ConnectHandler(platformView);
        platformView.SetView(VirtualView);
    }

    /// <inheritdoc/>
    protected override Platform.MaciOS.MauiBottomSheet CreatePlatformView()
    {
        _ = MauiContext ?? throw new InvalidOperationException("MauiContext is null, please check your MauiApplication.");

        return new Platform.MaciOS.MauiBottomSheet(MauiContext);
    }

    /// <inheritdoc/>
    protected override void DisconnectHandler(Platform.MaciOS.MauiBottomSheet platformView)
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
        handler.PlatformView.SetShowHeader();
    }

    private static void MapIsOpen(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        handler.PlatformView.SetIsOpenAsync().SafeFireAndForget(continueOnCapturedContext: false);
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

    private static void MapPeekHeight(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        handler.PlatformView.SetPeekHeight();
    }

    private static void MapContent(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        handler.PlatformView.SetContent();
    }

    private static void MapPadding(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        handler.PlatformView.SetPadding();
    }

    private static void MapBackgroundColor(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        handler.PlatformView.SetBottomSheetBackgroundColor();
    }

    private static void MapIgnoreSafeArea(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        handler.PlatformView.SetIgnoreSafeArea();
    }

    private static void MapCornerRadius(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        handler.PlatformView.SetCornerRadius();
    }

    private static void MapWindowBackgroundColor(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        handler.PlatformView.SetWindowBackgroundColor();
    }

    private static void MapIsModal(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        handler.PlatformView.SetIsModal();
    }

    private static void MapBottomSheetStyle(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        handler.PlatformView.SetBottomSheetStyle();
    }
}