namespace Maui.BottomSheet;

public static partial class IBottomSheetExtensions
{
	#region Header Appearance
	public static bool HasTopLeft(this IBottomSheet bottomSheet)
	{
		return bottomSheet.ShowHeader
			&& string.IsNullOrWhiteSpace(bottomSheet.TopLeftButtonText) == false
			&& (bottomSheet.HeaderAppearance == BottomSheetHeaderAppearanceMode.LeftButton
				|| bottomSheet.HeaderAppearance == BottomSheetHeaderAppearanceMode.LeftAndRightButton);
	}

	public static bool HasTopRight(this IBottomSheet bottomSheet)
	{
		return bottomSheet.ShowHeader
			&& string.IsNullOrWhiteSpace(bottomSheet.TopRightButtonText) == false
			&& (bottomSheet.HeaderAppearance == BottomSheetHeaderAppearanceMode.RightButton
				|| bottomSheet.HeaderAppearance == BottomSheetHeaderAppearanceMode.LeftAndRightButton);
	}

	public static bool HasTitle(this IBottomSheet bottomSheet)
	{
		return bottomSheet.ShowHeader
			&& string.IsNullOrWhiteSpace(bottomSheet.TitleText) == false;
	}
	#endregion
}

