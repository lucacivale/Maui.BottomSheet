using UIKit;

namespace Maui.BottomSheet;

public static class UISheetPresentationControllerDetentIdentifierExtensions
{
	public static BottomSheetState ToSheetState(this UISheetPresentationControllerDetentIdentifier identifier)
	{
		return identifier switch
		{
			UISheetPresentationControllerDetentIdentifier.Medium => BottomSheetState.Medium,
			UISheetPresentationControllerDetentIdentifier.Large => BottomSheetState.Large,
			UISheetPresentationControllerDetentIdentifier.Unknown => BottomSheetState.Medium,
			_ => BottomSheetState.Medium
		};
	}
}

