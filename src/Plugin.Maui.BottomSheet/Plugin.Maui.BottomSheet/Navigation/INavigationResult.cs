namespace Plugin.Maui.BottomSheet.Navigation;

/// <summary>
/// Contains the result information from a navigation operation.
/// </summary>
public interface INavigationResult
{
    /// <summary>
    /// Gets whether the navigation completed successfully without errors.
    /// </summary>
    bool Success { get; }

    /// <summary>
    /// Gets whether the navigation was cancelled by user or system action.
    /// </summary>
    bool Cancelled { get; }

    /// <summary>
    /// Gets any exception that occurred during navigation.
    /// </summary>
    Exception? Exception { get; }
}