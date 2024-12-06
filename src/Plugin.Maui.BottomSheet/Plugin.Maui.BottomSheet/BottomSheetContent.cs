namespace Plugin.Maui.BottomSheet;

/// <summary>
/// <see cref="BottomSheet"/> content.
/// </summary>
public class BottomSheetContent : BindableObject
{
    /// <summary>
    /// Bindable property.
    /// </summary>
    // ReSharper disable once MemberCanBePrivate.Global
    public static readonly BindableProperty ContentTemplateProperty =
        BindableProperty.Create(
            nameof(ContentTemplate),
            typeof(DataTemplate),
            typeof(BottomSheetContent));

    /// <summary>
    /// Gets or sets the content <see cref="DataTemplate"/>.
    /// </summary>
    public DataTemplate? ContentTemplate { get => (DataTemplate?)GetValue(ContentTemplateProperty); set => SetValue(ContentTemplateProperty, value); }

    /// <summary>
    /// Gets or sets the parent <see cref="Element"/> of this element.
    /// </summary>
    public Element? Parent { get; set; }
}