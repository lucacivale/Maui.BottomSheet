namespace Plugin.Maui.BottomSheet;

[ContentProperty(nameof(Content))]
public class BottomSheetContentView : BindableObject
{
    /// <summary>
    /// Bindable property for the parent.
    /// </summary>
    public static readonly BindableProperty ParentProperty =
        BindableProperty.Create(
            nameof(Parent),
            typeof(Element),
            typeof(BottomSheetContent),
            propertyChanged: OnParentChanged);

    /// <summary>
    /// Bindable property for the direct content.
    /// </summary>
    public static readonly BindableProperty ContentProperty =
        BindableProperty.Create(
            nameof(Content),
            typeof(View),
            typeof(BottomSheetContent));

    /// <summary>
    /// Bindable property for the content template.
    /// </summary>
    public static readonly BindableProperty ContentTemplateProperty =
        BindableProperty.Create(
            nameof(ContentTemplate),
            typeof(DataTemplate),
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
    public Element? Parent { get => (Element?)GetValue(ParentProperty); set => SetValue(ParentProperty, value); }

    /// <summary>
    /// Creates and returns the content view, using either the template or direct content.
    /// </summary>
    /// <returns>The content view ready for display.</returns>
    /// <exception cref="BottomSheetContentNotSetException">Thrown when neither Content nor ContentTemplate is set.</exception>
    internal virtual View CreateContent()
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

    internal virtual void Remove()
    {
        if (Content is not null)
        {
            Content.BindingContext = null;
            Content.Parent = null;
        }

        Content?.DisconnectHandlers();
    }

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();

        OnBindingContextChanged(Content);
    }

    protected void OnBindingContextChanged(View? content)
    {
        if (content is null)
        {
            return;
        }

        content.BindingContext = BindingContext;

        foreach (View view in content.GetVisualTreeDescendants().OfType<View>())
        {
            view.BindingContext = content.BindingContext;
        }
    }

    protected virtual void OnParentChanged(Element parent)
    {
        if (Content is not null)
        {
            Content.Parent = parent;
        }
    }

    private static void OnParentChanged(BindableObject bindable, object oldValue, object newValue)
        => ((BottomSheetContentView)bindable).OnParentChanged((Element)newValue);
}