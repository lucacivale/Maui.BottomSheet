namespace Plugin.Maui.BottomSheet;

/// <summary>
/// Represents the content area of a bottom sheet with support for templates and direct content.
/// </summary>
[ContentProperty(nameof(Content))]
public sealed class BottomSheetContent : BindableObject
{
    /// <summary>
    /// Bindable property for the content template.
    /// </summary>
    public static readonly BindableProperty ContentTemplateProperty =
        BindableProperty.Create(
            nameof(ContentTemplate),
            typeof(DataTemplate),
            typeof(BottomSheetContent));

    /// <summary>
    /// Bindable property for the direct content.
    /// </summary>
    public static readonly BindableProperty ContentProperty =
        BindableProperty.Create(
            nameof(Content),
            typeof(View),
            typeof(BottomSheetContent));

    /// <summary>
    /// Gets or sets the data template used to create the content dynamically.
    /// </summary>
    public DataTemplate? ContentTemplate { get => (DataTemplate?)GetValue(ContentTemplateProperty); set => SetValue(ContentTemplateProperty, value); }

    /// <summary>
    /// Gets or sets the direct content view for the bottom sheet.
    /// </summary>
    public View? Content { get => (View?)GetValue(ContentProperty); set => SetValue(ContentProperty, value); }

    /// <summary>
    /// Gets or sets the parent element of this content.
    /// </summary>
    public Element? Parent { get; set; }

    /// <summary>
    /// Creates and returns the content view, using either the template or direct content.
    /// </summary>
    /// <returns>The content view ready for display.</returns>
    /// <exception cref="BottomSheetContentNotSetException">Thrown when neither Content nor ContentTemplate is set.</exception>
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