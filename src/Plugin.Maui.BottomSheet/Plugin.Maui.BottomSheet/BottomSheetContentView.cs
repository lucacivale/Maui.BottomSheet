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

    internal void Remove()
    {
        BindingContext = null;
        Parent = null;

        Content?.DisconnectHandlers();
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CS0618: Class 'Microsoft.Maui.Controls.Compatibility.Layout' is obsolete: 'Use Microsoft.Maui.Controls.Layout instead. For more information, see https://learn.microsoft.com/dotnet/maui/user-interface/layouts/custom'", Justification = "Views like ContentView still use compatibility layout. As long as Compatibility.Layout isn't removed we need this.")]
    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();

        if (Content is not null)
        {
            Content.BindingContext = BindingContext;

            if (Content is Layout layout)
            {
                foreach (View view in layout.Children.OfType<View>())
                {
                    view.BindingContext = Content.BindingContext;
                }
            }

            // Views like ContentView still use compatibility layout. As long as Compatibility.Layout isn't removed we need this.
            else if (Content is Microsoft.Maui.Controls.Compatibility.Layout layoutController)
            {
                foreach (Element element in layoutController.Children)
                {
                    element.BindingContext = Content.BindingContext;
                }
            }
        }
    }

    private static void OnParentChanged(BindableObject bindable, object oldValue, object newValue)
        => ((BottomSheetContentView)bindable).OnParentChanged((Element)newValue);

    private void OnParentChanged(Element parent)
    {
        if (Content is not null)
        {
            Content.Parent = parent;
        }
    }
}