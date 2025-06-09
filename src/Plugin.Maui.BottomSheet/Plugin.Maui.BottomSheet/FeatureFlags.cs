namespace Plugin.Maui.BottomSheet;

/// <summary>
/// Represents the configuration for enabling or disabling various plugin features.
/// </summary>
public sealed class FeatureFlags
{
    /// <summary>
    /// Gets or sets a value indicating whether content height fills up all available space. See issue #125.
    /// </summary>
    public bool ContentFillsAvailableSpace { get; set; }
}
