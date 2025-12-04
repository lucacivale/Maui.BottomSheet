namespace Plugin.Maui.BottomSheet;

#if ANDROID
using Microsoft.Maui.Platform;
#endif

/// <summary>
/// Represents a style configuration for customizing the appearance and behavior
/// of a bottom sheet header, including title text properties and close button dimensions.
/// </summary>
public class BottomSheetHeaderStyle : BindableObject
{
    /// <summary>
    /// Bindable property that defines the color of the title text in the bottom sheet header.
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
    /// Bindable property for enabling or disabling auto-scaling of the title text font.
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
    /// Bindable property for the tint color of the close button.
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
    /// Gets or sets the color of the title text in the BottomSheet header.
    /// </summary>
    public Color TitleTextColor
    {
        get => (Color)GetValue(TitleTextColorProperty);
        set => SetValue(TitleTextColorProperty, value);
    }

    /// <summary>
    /// Gets or sets the font size of the title text.
    /// </summary>
    public double TitleTextFontSize
    {
        get => (double)GetValue(TitleTextFontSizeProperty);
        set => SetValue(TitleTextFontSizeProperty, value);
    }

    /// <summary>
    /// Gets or sets the font attributes of the title text.
    /// </summary>
    public FontAttributes TitleTextFontAttributes
    {
        get => (FontAttributes)GetValue(TitleTextFontAttributesProperty);
        set => SetValue(TitleTextFontAttributesProperty, value);
    }

    /// <summary>
    /// Gets or sets the font family of the title text.
    /// </summary>
    public string TitleTextFontFamily
    {
        get => (string)GetValue(TitleTextFontFamilyProperty);
        set => SetValue(TitleTextFontFamilyProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the title text font supports auto-scaling based on user preferences.
    /// </summary>
    public bool TitleTextFontAutoScalingEnabled
    {
        get => (bool)GetValue(TitleTextFontAutoScalingEnabledProperty);
        set => SetValue(TitleTextFontAutoScalingEnabledProperty, value);
    }

    /// <summary>
    /// Gets or sets the height request for the close button in the bottom sheet header.
    /// </summary>
    public double CloseButtonHeightRequest
    {
        get => (double)GetValue(CloseButtonHeightRequestProperty);
        set => SetValue(CloseButtonHeightRequestProperty, value);
    }

    /// <summary>
    /// Gets or sets the width request for the close button in the bottom sheet header.
    /// </summary>
    public double CloseButtonWidthRequest
    {
        get => (double)GetValue(CloseButtonWidthRequestProperty);
        set => SetValue(CloseButtonWidthRequestProperty, value);
    }

    /// <summary>
    /// Gets or sets the color of the close button tint.
    /// </summary>
    public Color CloseButtonTintColor
    {
        get => (Color)GetValue(CloseButtonTintColorProperty);
        set => SetValue(CloseButtonTintColorProperty, value);
    }
}