namespace Plugin.Maui.BottomSheet;

/// <summary>
/// Common extensions.
/// </summary>
internal static class BottomSheetHeaderExtensions
{
    /// <summary>
    /// Is <see cref="BottomSheetHeader"/> top left button configured.
    /// </summary>
    /// <param name="bottomSheetHeader"><see cref="BottomSheetHeader"/>.</param>
    /// <returns>Is button configured.</returns>
    public static bool HasTopLeftButton(this BottomSheetHeader? bottomSheetHeader)
    {
        return bottomSheetHeader is not null
            && bottomSheetHeader.HeaderAppearance is BottomSheetHeaderButtonAppearanceMode.LeftButton
                or BottomSheetHeaderButtonAppearanceMode.LeftAndRightButton
            && bottomSheetHeader.TopLeftButton is not null;
    }

    /// <summary>
    /// Is <see cref="BottomSheetHeader"/> top right button configured.
    /// </summary>
    /// <param name="bottomSheetHeader"><see cref="BottomSheetHeader"/>.</param>
    /// <returns>Is button configured.</returns>
    public static bool HasTopRightButton(this BottomSheetHeader? bottomSheetHeader)
    {
        return bottomSheetHeader is not null
            && bottomSheetHeader.HeaderAppearance is BottomSheetHeaderButtonAppearanceMode.RightButton
               or BottomSheetHeaderButtonAppearanceMode.LeftAndRightButton
            && bottomSheetHeader.TopRightButton is not null;
    }

    /// <summary>
    /// Is <see cref="BottomSheetHeader"/> title configured.
    /// </summary>
    /// <param name="bottomSheetHeader"><see cref="BottomSheetHeader"/>.</param>
    /// <returns>Is title configured.</returns>
    public static bool HasTitle(this BottomSheetHeader? bottomSheetHeader)
    {
        return bottomSheetHeader is not null
            && !string.IsNullOrWhiteSpace(bottomSheetHeader.TitleText);
    }

    /// <summary>
    /// Is <see cref="BottomSheetHeader"/> header view configured.
    /// </summary>
    /// <param name="bottomSheetHeader"><see cref="BottomSheetHeader"/>.</param>
    /// <returns>Is header view configured.</returns>
    public static bool HasHeaderView(this BottomSheetHeader? bottomSheetHeader)
    {
        return bottomSheetHeader is not null
            && bottomSheetHeader.HeaderDataTemplate is not null;
    }
}