using Google.Android.Material.BottomSheet;
using AndroidView = Android.Views.View;

namespace Maui.BottomSheet.Platforms.Android;

public class BottomSheetCallback : BottomSheetBehavior.BottomSheetCallback
{
	private readonly IBottomSheet? _bottomSheet;
	private readonly MauiBottomSheet _mauiBottomSheet;

	#region Constructor
	public BottomSheetCallback(IBottomSheet? bottomSheet, MauiBottomSheet mauiBottomSheet)
	{
		_mauiBottomSheet = mauiBottomSheet;
		_bottomSheet = bottomSheet;
	}
	#endregion

	public override void OnSlide(AndroidView bottomSheet, float newState)
	{
	}

	public override void OnStateChanged(AndroidView view, int p1)
	{
		if (_bottomSheet is null)
		{
			return;
		}

		if (_mauiBottomSheet.TrySetState(p1) == false)
		{
			_mauiBottomSheet.SetSelectedSheetState();
		}
	}
}

