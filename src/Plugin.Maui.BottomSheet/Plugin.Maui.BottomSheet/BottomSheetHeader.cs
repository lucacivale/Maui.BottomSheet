namespace Plugin.Maui.BottomSheet;

using Microsoft.Maui.Controls;

/// <summary>
/// The header shown at the top of <see cref="IBottomSheet"/>.
/// </summary>
public sealed class BottomSheetHeader : BindableObject
{
    /// <summary>
    /// Bindable property.
    /// </summary>
    public static readonly BindableProperty TitleTextProperty =
        BindableProperty.Create(
            nameof(TitleText),
            typeof(string),
            typeof(BottomSheetHeader));

    /// <summary>
    /// Bindable property.
    /// </summary>
    public static readonly BindableProperty TopLeftButtonProperty =
        BindableProperty.Create(
            nameof(TopLeftButton),
            typeof(Button),
            typeof(BottomSheetHeader));

    /// <summary>
    /// Bindable property.
    /// </summary>
    // ReSharper disable once MemberCanBePrivate.Global
    public static readonly BindableProperty TopRightButtonProperty =
        BindableProperty.Create(
            nameof(TopRightButton),
            typeof(Button),
            typeof(BottomSheetHeader));

    /// <summary>
    /// Bindable property.
    /// </summary>
    // ReSharper disable once MemberCanBePrivate.Global
    public static readonly BindableProperty HeaderDataTemplateProperty =
        BindableProperty.Create(
            nameof(HeaderDataTemplate),
            typeof(DataTemplate),
            typeof(BottomSheetHeader));

    /// <summary>
    /// Bindable property.
    /// </summary>
    // ReSharper disable once MemberCanBePrivate.Global
    public static readonly BindableProperty HeaderAppearanceProperty =
        BindableProperty.Create(
            nameof(HeaderAppearance),
            typeof(BottomSheetHeaderButtonAppearanceMode),
            typeof(BottomSheetHeader));

    /// <summary>
    /// Gets or sets title text.
    /// </summary>
    public string? TitleText { get => (string?)GetValue(TitleTextProperty); set => SetValue(TitleTextProperty, value); }

    /// <summary>
    /// Gets or sets the <see cref="Button"/> at the top left in the <see cref="BottomSheetHeader"/>.
    /// </summary>
    public Button? TopLeftButton { get => (Button?)GetValue(TopLeftButtonProperty); set => SetValue(TopLeftButtonProperty, value); }

    /// <summary>
    /// Gets or sets the <see cref="Button"/> at the top right in the <see cref="BottomSheetHeader"/>.
    /// </summary>
    public Button? TopRightButton { get => (Button?)GetValue(TopRightButtonProperty); set => SetValue(TopRightButtonProperty, value); }

    /// <summary>
    /// Gets or sets a custom header <see cref="View"/>.
    /// </summary>
    public DataTemplate? HeaderDataTemplate { get => (DataTemplate?)GetValue(HeaderDataTemplateProperty); set => SetValue(HeaderDataTemplateProperty, value); }

    /// <summary>
    /// Gets or sets the <see cref="BottomSheetHeaderButtonAppearanceMode"/>.
    /// </summary>
    public BottomSheetHeaderButtonAppearanceMode HeaderAppearance { get => (BottomSheetHeaderButtonAppearanceMode)GetValue(HeaderAppearanceProperty); set => SetValue(HeaderAppearanceProperty, value); }

    /// <summary>
    /// Gets or sets the parent <see cref="Element"/> of this element.
    /// </summary>
    public Element? Parent { get; set; }
}