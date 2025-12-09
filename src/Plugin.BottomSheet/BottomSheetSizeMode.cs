namespace Plugin.BottomSheet;

/// <summary>
/// Represents a size mode for the bottom sheet where the height is adjusted dynamically
/// to fit the content displayed within it. In this mode, the bottom sheet wraps its
/// content and does not occupy more space than necessary.
/// </summary>
public enum BottomSheetSizeMode
{
    /// <summary>
    /// Represents a size mode for the bottom sheet where the height
    /// is set to a fixed value based on the number of states it can display.
    /// This mode ensures the bottom sheet is fully expanded rather
    /// than dynamically adjusting its size to-fit content.
    /// </summary>
    States,

    /// <summary>
    /// A size mode indicating that the bottom sheet dynamically adjusts its height
    /// to precisely fit the content it displays. In this mode, the bottom sheet wraps
    /// around its contents and does not occupy any more vertical space than necessary.
    /// </summary>
    FitToContent,
}
