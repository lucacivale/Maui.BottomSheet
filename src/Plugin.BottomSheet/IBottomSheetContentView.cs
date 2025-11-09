namespace Plugin.BottomSheet;

internal interface IBottomSheetContentView
{
    public object? Content { get; }

    public object? ContentTemplate { get; }
}