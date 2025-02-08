namespace Plugin.Maui.BottomSheet.Platform.MaciOS;

/// <inheritdoc />
internal sealed class BottomSheetPage : ContentPage
{
    /// <summary>
    /// Gets or sets <see cref="IBottomSheet"/>.
    /// </summary>
    internal IBottomSheet? BottomSheet { get; set; }
}