namespace Plugin.Maui.BottomSheet;

/// <summary>
/// Style built in <see cref="BottomSheet"/> components.
/// </summary>
public sealed class BottomSheetStyle : BindableObject
{
    /// <summary>
    /// Bindable property.
    /// </summary>
    public static readonly BindableProperty HeaderStyleProperty =
        BindableProperty.Create(
            nameof(HeaderStyle),
            typeof(BottomSheetHeaderStyle),
            typeof(BottomSheetStyle),
            defaultValue: new BottomSheetHeaderStyle());

    /// <summary>
    /// Gets or sets style class for <see cref="BottomSheetHeader"/>.
    /// </summary>
    public BottomSheetHeaderStyle HeaderStyle { get => (BottomSheetHeaderStyle)GetValue(HeaderStyleProperty); set => SetValue(HeaderStyleProperty, value); }
}