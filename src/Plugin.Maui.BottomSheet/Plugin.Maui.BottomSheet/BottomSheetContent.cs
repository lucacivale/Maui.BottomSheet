namespace Plugin.Maui.BottomSheet;

/// <summary>
/// <see cref="BottomSheet"/> content.
/// </summary>
[ContentProperty(nameof(Content))]
public sealed class BottomSheetContent : BindableObject
{
    /// <summary>
    /// Bindable property.
    /// </summary>
    public static readonly BindableProperty ContentTemplateProperty =
        BindableProperty.Create(
            nameof(ContentTemplate),
            typeof(DataTemplate),
            typeof(BottomSheetContent));

    /// <summary>
    /// Bindable property.
    /// </summary>
    public static readonly BindableProperty ContentProperty =
        BindableProperty.Create(
            nameof(Content),
            typeof(View),
            typeof(BottomSheetContent));

    /// <summary>
    /// Gets or sets the content <see cref="DataTemplate"/>.
    /// </summary>
    public DataTemplate? ContentTemplate { get => (DataTemplate?)GetValue(ContentTemplateProperty); set => SetValue(ContentTemplateProperty, value); }

    /// <summary>
    /// Gets or sets the raw content of this view.
    /// </summary>
    public View? Content { get => (View?)GetValue(ContentProperty); set => SetValue(ContentProperty, value); }

    /// <summary>
    /// Gets or sets the parent <see cref="Element"/> of this element.
    /// </summary>
    public Element? Parent { get; set; }

    /// <summary>
    /// Creates content.
    /// </summary>
    /// <returns>If <see cref="ContentTemplate"/> is set the content is created and returned. Otherwise <see cref="Content"/> is returned.</returns>
    /// <exception cref="BottomSheetContentNotSetException">If <see cref="Content"/> nor <see cref="ContentTemplate"/> is set.</exception>
    public View CreateContent()
    {
        if (ContentTemplate?.CreateContent() is View content)
        {
            Content = content;
        }

        if (Content is null)
        {
            throw new BottomSheetContentNotSetException($"{nameof(Content)} must be set before creating content.");
        }

        Content.BindingContext = BindingContext;
        Content.Parent = Parent;

        return Content;
    }
}