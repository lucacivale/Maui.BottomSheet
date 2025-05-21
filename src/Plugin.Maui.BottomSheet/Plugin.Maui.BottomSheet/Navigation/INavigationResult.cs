namespace Plugin.Maui.BottomSheet.Navigation;

/// <summary>
/// Provides access to navigation result e.g. exceptions.
/// </summary>
public interface INavigationResult
{
    /// <summary>
    /// Gets a value indicating whether navigation was successful and no errors occurred.
    /// </summary>
    bool Success { get; }

    /// <summary>
    /// Gets a value indicating whether navigation was cancelled.
    /// </summary>
    bool Cancelled { get; }

    /// <summary>
    /// Gets the exception if one occurred.
    /// </summary>
    Exception? Exception { get; }
}