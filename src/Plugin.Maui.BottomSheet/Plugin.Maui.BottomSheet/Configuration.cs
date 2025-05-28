namespace Plugin.Maui.BottomSheet;

/// <summary>
/// Plugin config.
/// </summary>
public sealed class Configuration
{
    /// <summary>
    /// Gets or sets a value indicating whether on creation of a <see cref="BottomSheet"/> from a Page
    /// all applicable properties are copied to <see cref="BottomSheet"/>.
    /// </summary>
    public bool CopyPagePropertiesToBottomSheet { get; set; }

    /// <summary>
    /// Gets plugin feature flags.
    /// </summary>
    public FeatureFlags FeatureFlags { get; } = new();
}