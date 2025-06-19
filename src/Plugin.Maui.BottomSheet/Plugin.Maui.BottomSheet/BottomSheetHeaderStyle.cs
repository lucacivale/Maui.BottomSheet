namespace Plugin.Maui.BottomSheet;

using Microsoft.Maui.Platform;

/// <summary>
/// Style configuration for built-in bottom sheet header components.
/// </summary>
public class BottomSheetHeaderStyle : BindableObject
{
    /// <summary>
    /// Bindable property for the title text color.
    /// </summary>
    public static readonly BindableProperty TitleTextColorProperty =
        BindableProperty.Create(
            nameof(TitleTextColor),
            typeof(Color),
            typeof(BottomSheetHeaderStyle));

    /// <summary>
    /// Bindable property for the title text font size.
    /// </summary>
    public static readonly BindableProperty TitleTextFontSizeProperty =
        BindableProperty.Create(
            nameof(TitleTextFontSize),
            typeof(double),
            typeof(BottomSheetHeaderStyle));

    /// <summary>
    /// Bindable property for the title text font attributes.
    /// </summary>
    public static readonly BindableProperty TitleTextFontAttributesProperty =
        BindableProperty.Create(
            nameof(TitleTextFontAttributes),
            typeof(FontAttributes),
            typeof(BottomSheetHeaderStyle));

    /// <summary>
    /// Bindable property for the title text font family.
    /// </summary>
    public static readonly BindableProperty TitleTextFontFamilyProperty =
        BindableProperty.Create(
            nameof(TitleTextFontFamily),
            typeof(string),
            typeof(BottomSheetHeaderStyle));

    /// <summary>
    /// Bindable property for the title text font auto-scaling setting.
    /// </summary>
    public static readonly BindableProperty TitleTextFontAutoScalingEnabledProperty =
        BindableProperty.Create(
            nameof(TitleTextFontAutoScalingEnabled),
            typeof(bool),
            typeof(BottomSheetHeaderStyle));

    /// <summary>
    /// Bindable property for the close button height request.
    /// </summary>
    public static readonly BindableProperty CloseButtonHeightRequestProperty =
        BindableProperty.Create(
            nameof(CloseButtonHeightRequest),
            typeof(double),
            typeof(BottomSheetHeaderStyle),
            defaultValue: 40.0);

    /// <summary>
    /// Bindable property for the close button width request.
    /// </summary>
    public static readonly BindableProperty CloseButtonWidthRequestProperty =
        BindableProperty.Create(
            nameof(CloseButtonWidthRequest),
            typeof(double),
            typeof(BottomSheetHeaderStyle),
            defaultValue: 40.0);

    /// <summary>
    /// Bindable property for the close button tint color.
    /// </summary>
    public static readonly BindableProperty CloseButtonTintColorProperty =
        BindableProperty.Create(
            nameof(CloseButtonTintColor),
            typeof(Color),
            typeof(BottomSheetHeaderStyle),
            defaultValueCreator: _ =>
            {
                #if ANDROID
                return Android.Graphics.Color.Gray.ToColor();
                #else
                return Color.FromArgb("#EFEFF0");
                #endif
            });

    /// <summary>
    /// Gets or sets the color of the title text in the header.
    /// </summary>
    public Color TitleTextColor { get => (Color)GetValue(TitleTextColorProperty); set => SetValue(TitleTextColorProperty, value); }

    /// <summary>
    /// Gets or sets the font size of the title text in the header.
    /// </summary>
    public double TitleTextFontSize { get => (double)GetValue(TitleTextFontSizeProperty); set => SetValue(TitleTextFontSizeProperty, value); }

    /// <summary>
    /// Gets or sets the font attributes (bold, italic, etc.) of the title text.
    /// </summary>
    public FontAttributes TitleTextFontAttributes { get => (FontAttributes)GetValue(TitleTextFontAttributesProperty); set => SetValue(TitleTextFontAttributesProperty, value); }

    /// <summary>
    /// Gets or sets the font family of the title text in the header.
    /// </summary>
    public string TitleTextFontFamily { get => (string)GetValue(TitleTextFontFamilyProperty); set => SetValue(TitleTextFontFamilyProperty, value); }

    /// <summary>
    /// Gets or sets a value indicating whether font auto-scaling is enabled for the title text.
    /// </summary>
    public bool TitleTextFontAutoScalingEnabled { get => (bool)GetValue(TitleTextFontAutoScalingEnabledProperty); set => SetValue(TitleTextFontAutoScalingEnabledProperty, value); }

    /// <summary>
    /// Gets or sets the requested height of the close button.
    /// </summary>
    public double CloseButtonHeightRequest { get => (double)GetValue(CloseButtonHeightRequestProperty); set => SetValue(CloseButtonHeightRequestProperty, value); }

    /// <summary>
    /// Gets or sets the requested width of the close button.
    /// </summary>
    public double CloseButtonWidthRequest { get => (double)GetValue(CloseButtonWidthRequestProperty); set => SetValue(CloseButtonWidthRequestProperty, value); }

    /// <summary>
    /// Gets or sets the tint color applied to the close button.
    /// </summary>
    public Color CloseButtonTintColor { get => (Color)GetValue(CloseButtonTintColorProperty); set => SetValue(CloseButtonTintColorProperty, value); }
}