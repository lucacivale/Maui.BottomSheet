namespace Plugin.Maui.BottomSheet;

/// <summary>
/// Represents a bindable style configuration for the appearance and behavior of bottom sheet components.
/// </summary>
public sealed class BottomSheetStyle : BindableObject
{
    /// <summary>
    /// Bindable property used for defining the styling of the header component in a bottom sheet.
    /// </summary>
    public static readonly BindableProperty HeaderStyleProperty =
        BindableProperty.Create(
            nameof(HeaderStyle),
            typeof(BottomSheetHeaderStyle),
            typeof(BottomSheetStyle),
            defaultValue: new BottomSheetHeaderStyle());

    /// <summary>
    /// Gets or sets the style configuration for the header component of the bottom sheet.
    /// </summary>
    public BottomSheetHeaderStyle HeaderStyle
    {
        get => (BottomSheetHeaderStyle)GetValue(HeaderStyleProperty);
        set => SetValue(HeaderStyleProperty, value);
    }
}