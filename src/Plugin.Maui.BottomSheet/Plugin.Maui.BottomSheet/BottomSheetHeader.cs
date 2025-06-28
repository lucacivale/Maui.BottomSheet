namespace Plugin.Maui.BottomSheet;

using Microsoft.Maui.Controls;

/// <summary>
/// Represents the header section displayed at the top of a bottom sheet.
/// </summary>
[ContentProperty(nameof(Content))]
public sealed class BottomSheetHeader : BindableObject
{
    /// <summary>
    /// Bindable property for the title text.
    /// </summary>
    public static readonly BindableProperty TitleTextProperty =
        BindableProperty.Create(
            nameof(TitleText),
            typeof(string),
            typeof(BottomSheetHeader));

    /// <summary>
    /// Bindable property for the top left button.
    /// </summary>
    public static readonly BindableProperty TopLeftButtonProperty =
        BindableProperty.Create(
            nameof(TopLeftButton),
            typeof(Button),
            typeof(BottomSheetHeader));

    /// <summary>
    /// Bindable property for the top right button.
    /// </summary>
    // ReSharper disable once MemberCanBePrivate.Global
    public static readonly BindableProperty TopRightButtonProperty =
        BindableProperty.Create(
            nameof(TopRightButton),
            typeof(Button),
            typeof(BottomSheetHeader));

    /// <summary>
    /// Bindable property for the close button visibility.
    /// </summary>
    public static readonly BindableProperty ShowCloseButtonProperty =
        BindableProperty.Create(
            nameof(ShowCloseButton),
            typeof(bool),
            typeof(BottomSheetHeader));

    /// <summary>
    /// Bindable property for the close button position.
    /// </summary>
    public static readonly BindableProperty CloseButtonPositionProperty =
        BindableProperty.Create(
            nameof(CloseButtonPosition),
            typeof(CloseButtonPosition),
            typeof(BottomSheetHeader),
            defaultValue: CloseButtonPosition.TopRight);

    /// <summary>
    /// Bindable property for the custom header data template.
    /// </summary>
    // ReSharper disable once MemberCanBePrivate.Global
    public static readonly BindableProperty HeaderDataTemplateProperty =
        BindableProperty.Create(
            nameof(HeaderDataTemplate),
            typeof(DataTemplate),
            typeof(BottomSheetHeader));

    /// <summary>
    /// Bindable property for the direct content.
    /// </summary>
    public static readonly BindableProperty ContentProperty =
        BindableProperty.Create(
            nameof(Content),
            typeof(View),
            typeof(BottomSheetContent));

    /// <summary>
    /// Bindable property for the header button appearance mode.
    /// </summary>
    // ReSharper disable once MemberCanBePrivate.Global
    public static readonly BindableProperty HeaderAppearanceProperty =
        BindableProperty.Create(
            nameof(HeaderAppearance),
            typeof(BottomSheetHeaderButtonAppearanceMode),
            typeof(BottomSheetHeader));

    /// <summary>
    /// Gets or sets the title text displayed in the header.
    /// </summary>
    public string? TitleText { get => (string?)GetValue(TitleTextProperty); set => SetValue(TitleTextProperty, value); }

    /// <summary>
    /// Gets or sets the button displayed at the top left of the header.
    /// </summary>
    public Button? TopLeftButton { get => (Button?)GetValue(TopLeftButtonProperty); set => SetValue(TopLeftButtonProperty, value); }

    /// <summary>
    /// Gets or sets the button displayed at the top right of the header.
    /// </summary>
    public Button? TopRightButton { get => (Button?)GetValue(TopRightButtonProperty); set => SetValue(TopRightButtonProperty, value); }

    /// <summary>
    /// Gets or sets the position of the close button in the header.
    /// </summary>
    public CloseButtonPosition CloseButtonPosition { get => (CloseButtonPosition)GetValue(CloseButtonPositionProperty); set => SetValue(CloseButtonPositionProperty, value); }

    /// <summary>
    /// Gets or sets a value indicating whether the close button should be displayed.
    /// </summary>
    public bool ShowCloseButton { get => (bool)GetValue(ShowCloseButtonProperty); set => SetValue(ShowCloseButtonProperty, value); }

    /// <summary>
    /// Gets or sets the data template for creating a custom header view.
    /// </summary>
    public DataTemplate? HeaderDataTemplate { get => (DataTemplate?)GetValue(HeaderDataTemplateProperty); set => SetValue(HeaderDataTemplateProperty, value); }

    /// <summary>
    /// Gets or sets the direct content view for the bottom sheet.
    /// </summary>
    public View? Content { get => (View?)GetValue(ContentProperty); set => SetValue(ContentProperty, value); }

    /// <summary>
    /// Gets or sets the appearance mode for header buttons.
    /// </summary>
    public BottomSheetHeaderButtonAppearanceMode HeaderAppearance { get => (BottomSheetHeaderButtonAppearanceMode)GetValue(HeaderAppearanceProperty); set => SetValue(HeaderAppearanceProperty, value); }

    /// <summary>
    /// Gets or sets the parent element of this header.
    /// </summary>
    public Element? Parent { get; set; }

    /// <summary>
    /// Creates and returns the content view, using either the template or direct content.
    /// </summary>
    /// <returns>The content view ready for display.</returns>
    /// <exception cref="BottomSheetContentNotSetException">Thrown when neither Content nor HeaderDataTemplate is set.</exception>
    public View CreateContent()
    {
        if (HeaderDataTemplate?.CreateContent() is View content)
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