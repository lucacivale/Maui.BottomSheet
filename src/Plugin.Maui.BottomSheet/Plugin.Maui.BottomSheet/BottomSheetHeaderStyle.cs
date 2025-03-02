namespace Plugin.Maui.BottomSheet;

using Microsoft.Maui.Platform;

/// <summary>
/// Style built in <see cref="BottomSheetHeader"/> components.
/// </summary>
public class BottomSheetHeaderStyle : BindableObject
{
    /// <summary>
    /// Bindable property.
    /// </summary>
    public static readonly BindableProperty TitleTextColorProperty =
        BindableProperty.Create(
            nameof(TitleTextColor),
            typeof(Color),
            typeof(BottomSheetHeaderStyle));

    /// <summary>
    /// Bindable property.
    /// </summary>
    public static readonly BindableProperty TitleTextFontSizeProperty =
        BindableProperty.Create(
            nameof(TitleTextFontSize),
            typeof(double),
            typeof(BottomSheetHeaderStyle));

    /// <summary>
    /// Bindable property.
    /// </summary>
    public static readonly BindableProperty TitleTextFontAttributesProperty =
        BindableProperty.Create(
            nameof(TitleTextFontAttributes),
            typeof(FontAttributes),
            typeof(BottomSheetHeaderStyle));

    /// <summary>
    /// Bindable property.
    /// </summary>
    public static readonly BindableProperty TitleTextFontFamilyProperty =
        BindableProperty.Create(
            nameof(TitleTextFontFamily),
            typeof(string),
            typeof(BottomSheetHeaderStyle));

    /// <summary>
    /// Bindable property.
    /// </summary>
    public static readonly BindableProperty TitleTextFontAutoScalingEnabledProperty =
        BindableProperty.Create(
            nameof(TitleTextFontAutoScalingEnabled),
            typeof(bool),
            typeof(BottomSheetHeaderStyle));

    /// <summary>
    /// Bindable property.
    /// </summary>
    public static readonly BindableProperty CloseButtonHeightRequestProperty =
        BindableProperty.Create(
            nameof(CloseButtonHeightRequest),
            typeof(double),
            typeof(BottomSheetHeaderStyle),
            defaultValue: 40.0);

    /// <summary>
    /// Bindable property.
    /// </summary>
    public static readonly BindableProperty CloseButtonWidthRequestProperty =
        BindableProperty.Create(
            nameof(CloseButtonWidthRequest),
            typeof(double),
            typeof(BottomSheetHeaderStyle),
            defaultValue: 40.0);

    /// <summary>
    /// Bindable property.
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
    /// Gets or sets title text color.
    /// </summary>
    public Color TitleTextColor { get => (Color)GetValue(TitleTextColorProperty); set => SetValue(TitleTextColorProperty, value); }

    /// <summary>
    /// Gets or sets title text font size.
    /// </summary>
    public double TitleTextFontSize { get => (double)GetValue(TitleTextFontSizeProperty); set => SetValue(TitleTextFontSizeProperty, value); }

    /// <summary>
    /// Gets or sets title text font attributes.
    /// </summary>
    public FontAttributes TitleTextFontAttributes { get => (FontAttributes)GetValue(TitleTextFontAttributesProperty); set => SetValue(TitleTextFontAttributesProperty, value); }

    /// <summary>
    /// Gets or sets title text font family.
    /// </summary>
    public string TitleTextFontFamily { get => (string)GetValue(TitleTextFontFamilyProperty); set => SetValue(TitleTextFontFamilyProperty, value); }

    /// <summary>
    /// Gets or sets a value indicating whether title text font auto-scaling is enabled.
    /// </summary>
    public bool TitleTextFontAutoScalingEnabled { get => (bool)GetValue(TitleTextFontAutoScalingEnabledProperty); set => SetValue(TitleTextFontAutoScalingEnabledProperty, value); }

    /// <summary>
    /// Gets or sets close button height request.
    /// </summary>
    public double CloseButtonHeightRequest { get => (double)GetValue(CloseButtonHeightRequestProperty); set => SetValue(CloseButtonHeightRequestProperty, value); }

    /// <summary>
    /// Gets or sets close button width request.
    /// </summary>
    public double CloseButtonWidthRequest { get => (double)GetValue(CloseButtonWidthRequestProperty); set => SetValue(CloseButtonWidthRequestProperty, value); }

    /// <summary>
    /// Gets or sets close button tint.
    /// </summary>
    public Color CloseButtonTintColor { get => (Color)GetValue(CloseButtonTintColorProperty); set => SetValue(CloseButtonTintColorProperty, value); }
}