namespace Plugin.Maui.BottomSheet.Navigation;

/// <inheritdoc />
internal sealed class NavigationResult : INavigationResult
{
    /// <inheritdoc />
    public bool Success { get; set; } = true;

    /// <inheritdoc />
    public bool Cancelled { get; set; }

    /// <inheritdoc />
    public Exception? Exception { get; set; }
}