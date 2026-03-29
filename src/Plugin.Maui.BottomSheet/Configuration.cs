namespace Plugin.Maui.BottomSheet;

/// <summary>
/// Represents the configuration options available for customizing the behavior
/// of the BottomSheet plugin.
/// </summary>
public sealed class Configuration
{
    /// <summary>
    /// Gets or sets a value indicating whether the properties of a ContentPage, such as background color and padding,
    /// should be automatically copied to the bottom sheet when the bottom sheet is created from a page.
    /// </summary>
    public bool CopyPagePropertiesToBottomSheet { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the new peek height calculation should be used.
    /// Old behavior => measure view height manually(leads to #235)
    /// New behavior => use already computed View.DesiredSize.
    /// </summary>
    public bool UseNewPeekHeightCalculation { get; set; }
}