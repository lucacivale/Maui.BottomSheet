using UIKit;

namespace Maui.BottomSheet;

public class BottomSheetControllerDelegate : UISheetPresentationControllerDelegate
{
	private readonly IBottomSheet? _bottomSheet;

	public BottomSheetControllerDelegate(IBottomSheet? sheet) : base()
	{
		_bottomSheet = sheet;
	}

	public override void DidChangeSelectedDetentIdentifier(UISheetPresentationController sheetPresentationController)
	{
		if (_bottomSheet is not null)
		{
			_bottomSheet.SelectedSheetState = sheetPresentationController.SelectedDetentIdentifier.ToSheetState();
		}
	}
}

