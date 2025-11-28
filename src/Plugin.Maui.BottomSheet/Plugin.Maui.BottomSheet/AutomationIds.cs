namespace Plugin.Maui.BottomSheet;

/// <summary>
/// Provides a set of constant automation IDs used for identifying UI elements in the BottomSheet components.
/// These automation IDs are primarily used for UI automation testing purposes.
/// </summary>
public static class AutomationIds
{
    /// <summary>
    /// Represents the automation ID for the handle element of the bottom sheet.
    /// This constant is used to uniquely identify the handle within the automation framework,
    /// providing support for UI testing and ensuring accessibility consistency.
    /// </summary>
    public const string Handle = "Plugin.Maui.BottomSheet.Handle";

    /// <summary>
    /// Represents the automation ID for the header element of the bottom sheet.
    /// This constant is used to uniquely identify the header within the
    /// automation framework, facilitating UI testing and accessibility purposes.
    /// </summary>
    public const string Header = "Plugin.Maui.BottomSheet.BottomSheetHeader";

    /// <summary>
    /// Represents the automation ID for the title element in the header of a bottom sheet in the Plugin.Maui.BottomSheet namespace.
    /// This constant is used to automate and identify the title element within user interface tests or related interactions.
    /// </summary>
    public const string HeaderTitle = "Plugin.Maui.BottomSheet.BottomSheetHeaderTitle";

    /// <summary>
    /// Represents the automation ID for the Close Button in the header of the Bottom Sheet.
    /// This ID is used to identify and interact with the Close Button element programmatically,
    /// ensuring accessibility and supporting automated UI testing.
    /// </summary>
    public const string HeaderCloseButton = "Plugin.Maui.BottomSheet.BottomSheetHeaderCloseButton";
}