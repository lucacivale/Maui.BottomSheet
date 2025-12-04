namespace Plugin.BottomSheet.Windows;

/// <summary>
/// Represents a context class used to manage the lifecycle of a close operation.
/// Ensures that only one instance of <see cref="CloseContext"/> exists at a time.
/// Implements the <see cref="IDisposable"/> interface to release resources when no longer needed.
/// </summary>
internal partial class CloseContext : IDisposable
{
    private static CloseContext? _instance;

    /// <summary>
    /// Initializes a new instance of the <see cref="CloseContext"/> class.
    /// Ensures that only one instance of <see cref="CloseContext"/> can exist.
    /// </summary>
    /// <exception cref="NotSupportedException">Thrown if an attempt is made to create more than one instance of <see cref="CloseContext"/>.</exception>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S3010:Static fields should not be updated in constructors", Justification = "As designed.")]
    internal CloseContext()
    {
        if (_instance is not null)
        {
            throw new NotSupportedException();
        }

        _instance = this;
    }

    /// <summary>
    /// Releases resources used by the <see cref="CloseContext"/> instance.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Gets the current instance of the <see cref="CloseContext"/> class.
    /// Returns null if no instance exists.
    /// </summary>
    /// <returns>The current instance of <see cref="CloseContext"/> or null if no instance exists.</returns>
    internal static CloseContext? Instance()
    {
        return _instance;
    }

    /// <summary>
    /// Releases unmanaged and, optionally, managed resources.
    /// </summary>
    /// <param name="disposing">A value indicating whether to release both managed and unmanaged resources.
    /// If false, only unmanaged resources are released.</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Code Smell", "S2696:Instance members should not write to \"static\" fields", Justification = "As designed.")]
    protected virtual void Dispose(bool disposing)
    {
        if (!disposing)
        {
            return;
        }

        _instance = null;
    }
}
