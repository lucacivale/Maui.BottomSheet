namespace Plugin.Maui.BottomSheet;

/// <summary>
/// Represents the content view for a bottom sheet component.
/// This class provides the layout and associated functionality
/// for presenting content within a bottom sheet control.
/// </summary>
[ContentProperty(nameof(Content))]
public class BottomSheetContentView : BindableObject
{
    /// <summary>
    /// Bindable property for the parent element of the bottom sheet content.
    /// </summary>
    public static readonly BindableProperty ParentProperty =
        BindableProperty.Create(
            nameof(Parent),
            typeof(Element),
            typeof(BottomSheetContent),
            propertyChanged: OnParentChanged);

    /// <summary>
    /// Bindable property for the direct content of the bottom sheet.
    /// </summary>
    public static readonly BindableProperty ContentProperty =
        BindableProperty.Create(
            nameof(Content),
            typeof(View),
            typeof(BottomSheetContent));

    /// <summary>
    /// Bindable property for the content template, allowing the definition
    /// of a custom UI template to be used within the bottom sheet.
    /// </summary>
    public static readonly BindableProperty ContentTemplateProperty =
        BindableProperty.Create(
            nameof(ContentTemplate),
            typeof(DataTemplate),
            typeof(BottomSheetContent));

    /// <summary>
    /// Gets or sets the template used to render the content.
    /// </summary>
    public DataTemplate? ContentTemplate
    {
        get => (DataTemplate?)GetValue(ContentTemplateProperty);
        set => SetValue(ContentTemplateProperty, value);
    }

    /// <summary>
    /// Gets or sets the content displayed in the bottom sheet.
    /// </summary>
    public View? Content
    {
        get => (View?)GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    /// <summary>
    /// Gets or sets the parent element associated with this view.
    /// </summary>
    public Element? Parent
    {
        get => (Element?)GetValue(ParentProperty);
        set => SetValue(ParentProperty, value);
    }

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

        Content.BindingContext = null;
        Content.Parent = null;

        Content.BindingContext = BindingContext;
        Content.Parent = Parent;

        return Content;
    }

    /// <summary>
    /// Removes the associated content view and disconnects any bindings or event handlers.
    /// </summary>
    internal virtual void Remove()
    {
        if (Content is not null)
        {
            Content.BindingContext = null;
            Content.Parent = null;
        }

        Content?.DisconnectHandlers();
    }

    /// <summary>
    /// Invoked when the binding context of the current object changes.
    /// This method ensures that the binding context of associated content elements
    /// is updated to match the current binding context.
    /// </summary>
    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();

        OnBindingContextChanged(Content);
    }

    /// <summary>
    /// Updates the binding context of the current view and all its visual descendants.
    /// </summary>
    /// <param name="content">The view whose binding context needs to be updated.</param>
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

    /// <summary>
    /// Handles logic triggered when the parent element of the view changes.
    /// Updates the parent property of the associated content view accordingly.
    /// </summary>
    /// <param name="parent">The new parent element of the view.</param>
    protected virtual void OnParentChanged(Element parent)
    {
        if (Content is not null)
        {
            Content.Parent = parent;
        }
    }

    /// <summary>
    /// Handles the logic to update the content view when the parent changes.
    /// </summary>
    /// <param name="bindable">The bindable object.</param>
    /// <param name="oldValue">The old value.</param>
    /// <param name="newValue">The new value.</param>
    private static void OnParentChanged(BindableObject bindable, object oldValue, object newValue)
        => ((BottomSheetContentView)bindable).OnParentChanged((Element)newValue);
}