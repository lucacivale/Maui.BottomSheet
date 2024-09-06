namespace Plugin.Maui.BottomSheet;

/// <summary>
/// Show a button on the left, right, on both sides or no button at all.
/// </summary>
public enum BottomSheetHeaderButtonAppearanceMode
{
    /// <summary>
    /// Don't show a button.
    /// </summary>
    None,

    /// <summary>
    /// Show a button on the left and right.
    /// </summary>
    LeftAndRightButton,

    /// <summary>
    /// Show a button on the left.
    /// </summary>
    LeftButton,

    /// <summary>
    /// Show a button on the right.
    /// </summary>
    RightButton,
}