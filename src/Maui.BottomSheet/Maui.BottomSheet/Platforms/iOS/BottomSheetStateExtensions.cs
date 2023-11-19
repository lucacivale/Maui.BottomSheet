using UIKit;

namespace Maui.BottomSheet;

public static partial class IBottomSheetStateExtensions
{
	public static UISheetPresentationControllerDetentIdentifier ToPlatform(this BottomSheetState state)
	{
		return state switch
		{
			BottomSheetState.Medium => UISheetPresentationControllerDetentIdentifier.Medium,
			BottomSheetState.Large => UISheetPresentationControllerDetentIdentifier.Large,
			BottomSheetState.Peek => UISheetPresentationControllerDetentIdentifier.Unknown,
			_ => UISheetPresentationControllerDetentIdentifier.Unknown
		};
	}
}

