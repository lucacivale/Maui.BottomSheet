namespace Plugin.Maui.BottomSheet.Navigation;

/// <summary>
/// Implementation of navigation result containing operation status and error information.
/// </summary>
internal sealed class NavigationResult : INavigationResult
{
    /// <summary>
    /// Gets or sets a value indicating  whether the navigation completed successfully.
    /// </summary>
    public bool Success { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating  whether the navigation was cancelled.
    /// </summary>
    public bool Cancelled { get; set; }

    /// <summary>
    /// Gets or sets any exception that occurred during navigation.
    /// </summary>
    public Exception? Exception { get; set; }
}