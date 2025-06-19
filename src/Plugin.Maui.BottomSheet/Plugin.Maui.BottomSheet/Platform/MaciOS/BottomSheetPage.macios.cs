namespace Plugin.Maui.BottomSheet.Platform.MaciOS;

/// <summary>
/// A specialized content page for displaying bottom sheet content on macOS and iOS platforms.
/// </summary>
internal sealed class BottomSheetPage : ContentPage
{
    /// <summary>
    /// Gets or sets the bottom sheet interface associated with this page.
    /// </summary>
    internal IBottomSheet? BottomSheet { get; set; }
}