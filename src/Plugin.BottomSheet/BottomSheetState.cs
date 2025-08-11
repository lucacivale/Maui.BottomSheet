namespace Plugin.BottomSheet;

/// <summary>
/// Represents the available states for a bottom sheet.
/// </summary>
[Flags]
public enum BottomSheetState
{
    /// <summary>
    /// The bottom sheet shows only a peek of content at a specified height.
    /// </summary>
    Peek = 1,

    /// <summary>
    /// The bottom sheet expands to half the screen height.
    /// </summary>
    Medium = 2,

    /// <summary>
    /// The bottom sheet expands to full screen height.
    /// </summary>
    Large = 4,
}