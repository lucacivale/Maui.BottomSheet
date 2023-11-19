namespace Maui.BottomSheet;

public partial class BottomSheetHandler
{
	public static IPropertyMapper<IBottomSheet, BottomSheetHandler> BottomSheetMapper = new PropertyMapper<IBottomSheet, BottomSheetHandler>(ElementMapper)
	{
		[nameof(IBottomSheet.IsOpen)] = MapIsOpen,
		[nameof(IBottomSheet.IsDraggable)] = MapIsDraggable,
		[nameof(IBottomSheet.TitleText)] = MapTitleText,
		[nameof(IBottomSheet.TopLeftButtonText)] = MapTopLeftText,
		[nameof(IBottomSheet.TopRightButtonText)] = MapTopRightText,
		[nameof(IBottomSheet.HeaderAppearance)] = MapHeaderAppearance,
		[nameof(IBottomSheet.ShowHeader)] = MapShowHeader,
		[nameof(IBottomSheet.HasHandle)] = MapHasHandle,
		[nameof(IBottomSheet.SheetStates)] = MapSheetStates,
		[nameof(IBottomSheet.Peek)] = MapPeek,
		[nameof(IBottomSheet.SelectedSheetState)] = MapSelectedSheetState
	};

	public BottomSheetHandler(IPropertyMapper? mapper, CommandMapper? commandMapper)
		: base(mapper ?? BottomSheetMapper)
	{
	}

	public BottomSheetHandler()
		: base(BottomSheetMapper)
	{
	}

	public BottomSheetHandler(IMauiContext context)
		: base(BottomSheetMapper)
	{
		SetMauiContext(context);
	}
}

