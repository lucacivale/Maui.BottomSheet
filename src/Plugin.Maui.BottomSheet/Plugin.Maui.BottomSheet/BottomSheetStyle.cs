namespace Plugin.Maui.BottomSheet;

/// <summary>
/// Style configuration for built-in bottom sheet components.
/// </summary>
public sealed class BottomSheetStyle : BindableObject
{
    /// <summary>
    /// Bindable property for the header style configuration.
    /// </summary>
    public static readonly BindableProperty HeaderStyleProperty =
        BindableProperty.Create(
            nameof(HeaderStyle),
            typeof(BottomSheetHeaderStyle),
            typeof(BottomSheetStyle),
            defaultValue: new BottomSheetHeaderStyle());

    /// <summary>
    /// Gets or sets the style configuration for the bottom sheet header.
    /// </summary>
    public BottomSheetHeaderStyle HeaderStyle { get => (BottomSheetHeaderStyle)GetValue(HeaderStyleProperty); set => SetValue(HeaderStyleProperty, value); }
}