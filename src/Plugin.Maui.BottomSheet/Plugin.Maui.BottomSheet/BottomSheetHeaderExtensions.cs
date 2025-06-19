namespace Plugin.Maui.BottomSheet;

/// <summary>
/// Extension methods for working with bottom sheet headers.
/// </summary>
internal static class BottomSheetHeaderExtensions
{
    /// <summary>
    /// Determines whether the header has a top left button configured.
    /// </summary>
    /// <param name="bottomSheetHeader">The bottom sheet header to check.</param>
    /// <returns>True if a top left button is configured, false otherwise.</returns>
    public static bool HasTopLeftButton(this BottomSheetHeader? bottomSheetHeader)
    {
        return bottomSheetHeader is not null
            && bottomSheetHeader.HeaderAppearance is BottomSheetHeaderButtonAppearanceMode.LeftButton
                or BottomSheetHeaderButtonAppearanceMode.LeftAndRightButton
            && bottomSheetHeader.TopLeftButton is not null
            && bottomSheetHeader.HasTopLeftCloseButton() == false;
    }

    /// <summary>
    /// Determines whether the header has a top left close button configured.
    /// </summary>
    /// <param name="bottomSheetHeader">The bottom sheet header to check.</param>
    /// <returns>True if a top left close button is configured, false otherwise.</returns>
    public static bool HasTopLeftCloseButton(this BottomSheetHeader? bottomSheetHeader)
    {
        return bottomSheetHeader is not null
            && bottomSheetHeader.ShowCloseButton
            && bottomSheetHeader.CloseButtonPosition == CloseButtonPosition.TopLeft;
    }

    /// <summary>
    /// Determines whether the header has a top right button configured.
    /// </summary>
    /// <param name="bottomSheetHeader">The bottom sheet header to check.</param>
    /// <returns>True if a top right button is configured, false otherwise.</returns>
    public static bool HasTopRightButton(this BottomSheetHeader? bottomSheetHeader)
    {
        return bottomSheetHeader is not null
            && bottomSheetHeader.HeaderAppearance is BottomSheetHeaderButtonAppearanceMode.RightButton
               or BottomSheetHeaderButtonAppearanceMode.LeftAndRightButton
            && bottomSheetHeader.TopRightButton is not null
            && bottomSheetHeader.HasTopRightCloseButton() == false;
    }

    /// <summary>
    /// Determines whether the header has a top right close button configured.
    /// </summary>
    /// <param name="bottomSheetHeader">The bottom sheet header to check.</param>
    /// <returns>True if a top right close button is configured, false otherwise.</returns>
    public static bool HasTopRightCloseButton(this BottomSheetHeader? bottomSheetHeader)
    {
        return bottomSheetHeader is not null
            && bottomSheetHeader.ShowCloseButton
            && bottomSheetHeader.CloseButtonPosition == CloseButtonPosition.TopRight;
    }

    /// <summary>
    /// Determines whether the header has a title configured.
    /// </summary>
    /// <param name="bottomSheetHeader">The bottom sheet header to check.</param>
    /// <returns>True if a title is configured, false otherwise.</returns>
    public static bool HasTitle(this BottomSheetHeader? bottomSheetHeader)
    {
        return bottomSheetHeader is not null
            && !string.IsNullOrWhiteSpace(bottomSheetHeader.TitleText);
    }

    /// <summary>
    /// Determines whether the header has a custom header view configured.
    /// </summary>
    /// <param name="bottomSheetHeader">The bottom sheet header to check.</param>
    /// <returns>True if a custom header view is configured, false otherwise.</returns>
    public static bool HasHeaderView(this BottomSheetHeader? bottomSheetHeader)
    {
        return bottomSheetHeader is not null
            && (bottomSheetHeader.HeaderDataTemplate is not null
                || bottomSheetHeader.Content is not null);
    }
}