using UIKit;

namespace Maui.BottomSheet;

public static partial class IBottomSheetExtensions
{
	public static IList<UISheetPresentationControllerDetent> Detents(this IBottomSheet bottomSheet)
	{
		return bottomSheet.SheetStates switch
		{
			BottomSheetState.Peek => new List<UISheetPresentationControllerDetent>(),
			BottomSheetState.Medium => new List<UISheetPresentationControllerDetent> { { UISheetPresentationControllerDetent.CreateMediumDetent() } },
			BottomSheetState.Large => new List<UISheetPresentationControllerDetent> { { UISheetPresentationControllerDetent.CreateLargeDetent() } },
			BottomSheetState.All => new List<UISheetPresentationControllerDetent>
				{
					{ UISheetPresentationControllerDetent.CreateMediumDetent() },
					{ UISheetPresentationControllerDetent.CreateLargeDetent() }
				},
			_ => new List<UISheetPresentationControllerDetent> { { UISheetPresentationControllerDetent.CreateMediumDetent() } }
		};
	}
}

