namespace Plugin.BottomSheet;

/// <summary>
/// Defines the possible states of a bottom sheet, representing its level of expansion or visibility.
/// </summary>
[Flags]
public enum BottomSheetState
{
    /// <summary>
    /// Represents the state where the bottom sheet is partially visible,
    /// showing a limited portion of its content, typically at a predefined height.
    /// </summary>
    Peek = 1,

    /// <summary>
    /// The bottom sheet is displayed at a medium height, which provides more visibility into the content while not occupying the full screen.
    /// </summary>
    Medium = 2,

    /// <summary>
    /// The bottom sheet expands to its largest height, typically spanning most or all of the available vertical space.
    /// </summary>
    Large = 4,
}