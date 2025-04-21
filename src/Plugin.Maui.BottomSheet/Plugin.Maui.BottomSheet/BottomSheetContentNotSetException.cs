namespace Plugin.Maui.BottomSheet;

public class BottomSheetContentNotSetException : NullReferenceException
{
    public BottomSheetContentNotSetException(string message)
        : base(message)
    {
    }

    public BottomSheetContentNotSetException()
    {
    }

    public BottomSheetContentNotSetException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}