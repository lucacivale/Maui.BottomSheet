using System.Diagnostics.CodeAnalysis;

namespace Plugin.Maui.BottomSheet;

/// <summary>
/// Exception that is thrown when the content for the bottom sheet has not been set properly.
/// </summary>
public class BottomSheetContentNotSetException : NullReferenceException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BottomSheetContentNotSetException"/> class.
    /// Exception thrown when bottom sheet content is not properly configured.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public BottomSheetContentNotSetException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BottomSheetContentNotSetException"/> class.
    /// Exception thrown when bottom sheet content is not properly configured.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public BottomSheetContentNotSetException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BottomSheetContentNotSetException"/> class.
    /// Exception thrown when bottom sheet content is not properly configured.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    [ExcludeFromCodeCoverage]
    public BottomSheetContentNotSetException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}