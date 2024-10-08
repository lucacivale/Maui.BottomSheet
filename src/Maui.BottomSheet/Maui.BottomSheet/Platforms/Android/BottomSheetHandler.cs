﻿using Microsoft.Maui.Handlers;

namespace Maui.BottomSheet;

public partial class BottomSheetHandler : ViewHandler<IBottomSheet, MauiBottomSheet>
{
	public static void MapIsOpen(BottomSheetHandler handler, IBottomSheet view)
	{
		handler.PlatformView.SetIsOpen();
	}

    public static void MapIsCancelable(BottomSheetHandler handler, IBottomSheet view)
    {
        handler.PlatformView.SetIsCancelable();
    }
    
    public static void MapBackgroundColor(BottomSheetHandler handler, IBottomSheet view)
    {
	    handler.PlatformView.SetBackgroundColor();
    }

    public static void MapIsDraggable(BottomSheetHandler handler, IBottomSheet view)
	{
		handler.PlatformView.SetIsDraggable();
	}

	public static void MapPeek(BottomSheetHandler handler, IBottomSheet view)
	{
		handler.PlatformView.SetPeek();
	}

	public static void MapSelectedSheetState(BottomSheetHandler handler, IBottomSheet view)
	{
		handler.PlatformView.SetSelectedSheetState();
	}

	public static void MapTitleText(BottomSheetHandler handler, IBottomSheet view)
	{
		handler.PlatformView.SetTitleText();
	}

	public static void MapTopLeftText(BottomSheetHandler handler, IBottomSheet view)
	{
		handler.PlatformView.SetTopLeftText();
	}

	public static void MapTopRightText(BottomSheetHandler handler, IBottomSheet view)
	{
		handler.PlatformView.SetTopRightText();
	}

	public static void MapHeaderAppearance(BottomSheetHandler handler, IBottomSheet view)
	{
		handler.PlatformView.SetHeaderAppearance();
	}

	public static void MapShowHeader(BottomSheetHandler handler, IBottomSheet view)
	{
		handler.PlatformView.SetHeader();
	}

	public static void MapHasHandle(BottomSheetHandler handler, IBottomSheet view)
	{
		handler.PlatformView.SetHasHandle();
	}

	public static void MapSheetStates(BottomSheetHandler handler, IBottomSheet view)
	{
		handler.PlatformView.SetSheetStates();
	}

	protected override void ConnectHandler(MauiBottomSheet platformView)
	{
		base.ConnectHandler(platformView);
		platformView.SetView(VirtualView);
	}

	protected override MauiBottomSheet CreatePlatformView()
	{
        _ = MauiContext ?? throw new InvalidOperationException("MauiContext is null, please check your MauiApplication.");
        _ = MauiContext.Context ?? throw new InvalidOperationException("Android Context is null, please check your MauiApplication.");

        return new MauiBottomSheet(
			MauiContext, 
			MauiContext.Context);
	}

	protected override void DisconnectHandler(MauiBottomSheet platformView)
	{
		base.DisconnectHandler(platformView);

		platformView.Cleanup();
	}
}

