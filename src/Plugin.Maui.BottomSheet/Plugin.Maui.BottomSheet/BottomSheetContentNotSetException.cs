using System.Diagnostics.CodeAnalysis;

namespace Plugin.Maui.BottomSheet;

/// <summary>
/// Exception thrown when bottom sheet content is not properly configured.
/// </summary>
public class BottomSheetContentNotSetException : NullReferenceException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BottomSheetContentNotSetException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public BottomSheetContentNotSetException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BottomSheetContentNotSetException"/> class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public BottomSheetContentNotSetException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BottomSheetContentNotSetException"/> class with a specified error message and inner exception.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    [ExcludeFromCodeCoverage]
    public BottomSheetContentNotSetException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}