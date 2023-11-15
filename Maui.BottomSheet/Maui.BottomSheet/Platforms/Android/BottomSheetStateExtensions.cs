using Google.Android.Material.BottomSheet;

namespace Maui.BottomSheet.Platforms.Android;

public static class BottomSheetStateExtensions
{
	public static int ToPlatform(this BottomSheetState state)
	{
		return state switch
		{
			BottomSheetState.Medium => BottomSheetBehavior.StateHalfExpanded,
			BottomSheetState.Large => BottomSheetBehavior.StateExpanded,
			BottomSheetState.All => BottomSheetBehavior.StateHalfExpanded,
			BottomSheetState.Peek => BottomSheetBehavior.StateCollapsed,
			_ => BottomSheetBehavior.StateHalfExpanded
		};
	}
}

