namespace Plugin.Maui.BottomSheet.Navigation;

/// <summary>
/// Represents the result of a navigation operation, including whether it was successful,
/// whether it was canceled, and any exceptions that occurred.
/// </summary>
internal sealed class NavigationResult : INavigationResult
{
    /// <summary>
    /// Gets or sets a value indicating whether the navigation operation was successful.
    /// </summary>
    public bool Success { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the navigation operation was canceled.
    /// </summary>
    public bool Cancelled { get; set; }

    /// <summary>
    /// Gets or sets the exception that occurred during the navigation process, if any.
    /// </summary>
    public Exception? Exception { get; set; }
}