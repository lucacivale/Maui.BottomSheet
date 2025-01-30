namespace Plugin.Maui.BottomSheet.Handlers;

using Microsoft.Maui.Handlers;
using Plugin.Maui.BottomSheet;

using Plugin.Maui.BottomSheet.Platform.Android;

/// <summary>
/// <see cref="IBottomSheet"/> handler.
/// </summary>
internal sealed partial class BottomSheetHandler : ViewHandler<IBottomSheet, MauiBottomSheet>
{
    /// <inheritdoc/>
    protected override void ConnectHandler(MauiBottomSheet platformView)
    {
        base.ConnectHandler(platformView);
        platformView.SetView(VirtualView);
    }

    /// <inheritdoc/>
    protected override MauiBottomSheet CreatePlatformView()
    {
        _ = MauiContext ?? throw new InvalidOperationException("MauiContext is null, please check your MauiApplication.");
        _ = MauiContext.Context ?? throw new InvalidOperationException("Android Context is null, please check your MauiApplication.");

        return new MauiBottomSheet(MauiContext, MauiContext.Context);
    }

    /// <inheritdoc/>
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
        handler.PlatformView.SetShowHeader();
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
        MauiBottomSheet.SetStates();
    }

    private static void MapCurrentState(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        handler.PlatformView.SetCurrentState();
    }

    private static void MapPeek(BottomSheetHandler handler, IBottomSheet bottomSheet)
    {
        handler.PlatformView.SetPeek();
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
        // Method intentionally left empty.
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
}