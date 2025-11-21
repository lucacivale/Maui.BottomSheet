namespace Plugin.BottomSheet;

/// <summary>
/// Defines the available appearance modes for buttons in the bottom sheet header.
/// </summary>
public enum BottomSheetHeaderButtonAppearanceMode
{
    /// <summary>
    /// Indicates that no buttons will appear in the bottom sheet header.
    /// </summary>
    None,

    /// <summary>
    /// Both left and right buttons are displayed in the header.
    /// </summary>
    LeftAndRightButton,

    /// <summary>
    /// Displays only the left button in the bottom sheet header.
    /// </summary>
    LeftButton,

    /// <summary>
    /// Indicates that only the right button is displayed in the bottom sheet header.
    /// </summary>
    RightButton,
}