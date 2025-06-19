namespace Plugin.Maui.BottomSheet;

/// <summary>
/// Specifies which buttons should be displayed in the bottom sheet header.
/// </summary>
public enum BottomSheetHeaderButtonAppearanceMode
{
    /// <summary>
    /// No buttons are displayed in the header.
    /// </summary>
    None,

    /// <summary>
    /// Both left and right buttons are displayed in the header.
    /// </summary>
    LeftAndRightButton,

    /// <summary>
    /// Only the left button is displayed in the header.
    /// </summary>
    LeftButton,

    /// <summary>
    /// Only the right button is displayed in the header.
    /// </summary>
    RightButton,
}