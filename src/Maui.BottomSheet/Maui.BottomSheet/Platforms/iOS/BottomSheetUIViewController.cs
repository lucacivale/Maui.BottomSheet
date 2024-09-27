using System.Windows.Input;
using UIKit;

namespace Maui.BottomSheet.Platforms.iOS;

public class BottomSheetUIViewController : UINavigationController
{
	private readonly IBottomSheet? bottomSheet;
	public bool IsDismissed { get; private set; }

	public BottomSheetUIViewController(IBottomSheet? bottomSheet, UIViewController viewController) : base(viewController)
	{
		this.bottomSheet = bottomSheet;

		SetNavigationBarHidden(bottomSheet?.ShowHeader == false, false);
	}

	public override void ViewDidAppear(bool animated)
	{
		base.ViewDidAppear(animated);
		IsDismissed = false;

		EnableDragging(bottomSheet?.IsDraggable == true);

        bottomSheet.OnCompleteOpenCloseAction(true);
    }

    public override void ViewDidDisappear(bool animated)
	{
		base.ViewDidDisappear(animated);

		IsDismissed = true;
		if (bottomSheet != null)
		{
			bottomSheet.IsOpen = false;
		}

        bottomSheet.OnCompleteOpenCloseAction(false);
    }

	public void EnableDragging(bool enable)
	{
		if (PresentationController?.PresentedView?.GestureRecognizers?.First() is not null)
		{
			PresentationController.PresentedView.GestureRecognizers.First().Enabled = enable;
		}
	}
}

