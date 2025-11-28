namespace Plugin.Maui.BottomSheet.Navigation;

/// <summary>
/// Represents the outcome of a navigation operation within the bottom sheet navigation system.
/// </summary>
public interface INavigationResult
{
    /// <summary>
    /// Gets a value indicating whether the navigation operation was successful.
    /// </summary>
    bool Success { get; }

    /// <summary>
    /// Gets a value indicating whether the navigation operation was cancelled before completion.
    /// </summary>
    bool Cancelled { get; }

    /// <summary>
    /// Gets the exception encountered during the navigation operation, if any.
    /// </summary>
    Exception? Exception { get; }
}