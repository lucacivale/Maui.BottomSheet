namespace Plugin.Maui.BottomSheet.PlatformConfiguration.AndroidSpecific;

using Microsoft.Maui.Controls.PlatformConfiguration;

/// <summary>
/// Provides Android-specific platform configuration for bottom sheet controls.
/// </summary>
public static class BottomSheet
{
    /// <summary>
    /// Bindable property for the Android theme resource identifier.
    /// </summary>
    public static readonly BindableProperty ThemeProperty =
        BindableProperty.Create(
            "Theme",
            typeof(int),
            typeof(BottomSheet),
            defaultValueCreator: (_) =>
            {
                #if ANDROID
                return _Microsoft.Android.Resource.Designer.Resource.Style.Plugin_Maui_BottomSheet_BottomSheetDialog;
                #else
                return 0;
                #endif
            });

    /// <summary>
    /// Bindable property for the maximum width of the bottom sheet.
    /// </summary>
    public static readonly BindableProperty MaxWidthProperty =
        BindableProperty.Create(
            "MaxWidth",
            typeof(int),
            typeof(BottomSheet),
            defaultValue: int.MinValue);

    /// <summary>
    /// Bindable property for the maximum height of the bottom sheet.
    /// </summary>
    public static readonly BindableProperty MaxHeightProperty =
        BindableProperty.Create(
            "MaxHeight",
            typeof(int),
            typeof(BottomSheet),
            defaultValue: int.MinValue);

    /// <summary>
    /// Bindable property for the half expanded ratio of the bottom sheet.
    /// </summary>
    public static readonly BindableProperty HalfExpandedRatioProperty =
        BindableProperty.Create(
            "HalfExpandedRatio",
            typeof(float),
            typeof(BottomSheet),
            defaultValue: 0.5f);

    /// <summary>
    /// Bindable property for the margin of the bottom sheet.
    /// </summary>
    public static readonly BindableProperty MarginProperty =
        BindableProperty.Create(
            "Margin",
            typeof(Thickness),
            typeof(BottomSheet),
            defaultValue: Thickness.Zero);

    /// <summary>
    /// Sets the half expanded ratio for the bottom sheet using platform configuration.
    /// </summary>
    /// <param name="config">The Android configuration for the bottom sheet instance.</param>
    /// <param name="value">The half expanded ratio value.</param>
    /// <returns>The platform element configuration for method chaining.</returns>
    public static IPlatformElementConfiguration<Android, Maui.BottomSheet.BottomSheet> SetHalfExpandedRatio(this IPlatformElementConfiguration<Android, Maui.BottomSheet.BottomSheet> config, float value)
    {
        SetHalfExpandedRatio(config.Element as BindableObject, value);
        return config;
    }

    /// <summary>
    /// Sets the half expanded ratio for the specified bottom sheet element.
    /// </summary>
    /// <param name="element">The bottom sheet element.</param>
    /// <param name="value">The half expanded ratio value.</param>
    public static void SetHalfExpandedRatio(BindableObject element, float value)
    {
        element.SetValue(HalfExpandedRatioProperty, value);
    }

    /// <summary>
    /// Gets the half expanded ratio from the platform configuration.
    /// </summary>
    /// <param name="config">The Android configuration for the bottom sheet instance.</param>
    /// <returns>The half expanded ratio value.</returns>
    public static float GetHalfExpandedRatio(this IPlatformElementConfiguration<Android, Maui.BottomSheet.BottomSheet> config)
    {
        return GetHalfExpandedRatio(config.Element as BindableObject);
    }

    /// <summary>
    /// Gets the half expanded ratio from the specified bottom sheet element.
    /// </summary>
    /// <param name="element">The bottom sheet element.</param>
    /// <returns>The half expanded ratio value.</returns>
    public static float GetHalfExpandedRatio(BindableObject element)
    {
        return (float)element.GetValue(HalfExpandedRatioProperty);
    }

    /// <summary>
    /// Gets the theme resource identifier from the platform configuration.
    /// </summary>
    /// <param name="config">The Android configuration for the bottom sheet instance.</param>
    /// <returns>The associated theme resource identifier.</returns>
    public static int GetTheme(this IPlatformElementConfiguration<Android, Maui.BottomSheet.BottomSheet> config)
    {
        return GetTheme(config.Element as BindableObject);
    }

    /// <summary>
    /// Gets the theme resource identifier from the specified bottom sheet element.
    /// </summary>
    /// <param name="element">The bottom sheet element.</param>
    /// <returns>The associated theme resource identifier.</returns>
    public static int GetTheme(this IBottomSheet element)
    {
        if (element is not BindableObject bindable)
        {
            throw new ArgumentException("Element must be a BindableObject");
        }

        return GetTheme(bindable);
    }

    /// <summary>
    /// Sets the theme resource identifier using platform configuration.
    /// </summary>
    /// <param name="config">The Android configuration for the bottom sheet instance.</param>
    /// <param name="value">The theme resource identifier.</param>
    /// <returns>The platform element configuration for method chaining.</returns>
    public static IPlatformElementConfiguration<Android, Maui.BottomSheet.BottomSheet> SetTheme(this IPlatformElementConfiguration<Android, Maui.BottomSheet.BottomSheet> config, int value)
    {
        SetTheme(config.Element as BindableObject, value);
        return config;
    }

    /// <summary>
    /// Sets the theme resource identifier for the specified bottom sheet element.
    /// </summary>
    /// <param name="element">The bottom sheet element.</param>
    /// <param name="value">The theme resource identifier.</param>
    public static void SetTheme(this IBottomSheet element, int value)
    {
        if (element is not BindableObject bindable)
        {
            throw new ArgumentException("Element must be a BindableObject");
        }

        SetTheme(bindable, value);
    }

    /// <summary>
    /// Gets the maximum width from the platform configuration.
    /// </summary>
    /// <param name="config">The Android configuration for the bottom sheet instance.</param>
    /// <returns>The maximum width value.</returns>
    public static int GetMaxWidth(this IPlatformElementConfiguration<Android, Maui.BottomSheet.BottomSheet> config)
    {
        return GetMaxWidth(config.Element as BindableObject);
    }

    /// <summary>
    /// Gets the maximum width from the specified bottom sheet element.
    /// </summary>
    /// <param name="element">The bottom sheet element.</param>
    /// <returns>The maximum width value.</returns>
    public static int GetMaxWidth(BindableObject element)
    {
        return (int)element.GetValue(MaxWidthProperty);
    }

    /// <summary>
    /// Gets the maximum height from the specified bottom sheet element.
    /// </summary>
    /// <param name="element">The bottom sheet element.</param>
    /// <returns>The maximum height value.</returns>
    public static int GetMaxHeight(BindableObject element)
    {
        return (int)element.GetValue(MaxHeightProperty);
    }

    /// <summary>
    /// Gets the maximum height from the platform configuration.
    /// </summary>
    /// <param name="config">The Android configuration for the bottom sheet instance.</param>
    /// <returns>The maximum height value.</returns>
    public static int GetMaxHeight(this IPlatformElementConfiguration<Android, Maui.BottomSheet.BottomSheet> config)
    {
        return GetMaxHeight(config.Element as BindableObject);
    }

    /// <summary>
    /// Sets the maximum width using platform configuration.
    /// </summary>
    /// <param name="config">The Android configuration for the bottom sheet instance.</param>
    /// <param name="value">The maximum width value.</param>
    /// <returns>The platform element configuration for method chaining.</returns>
    public static IPlatformElementConfiguration<Android, Maui.BottomSheet.BottomSheet> SetMaxWidth(this IPlatformElementConfiguration<Android, Maui.BottomSheet.BottomSheet> config, int value)
    {
        SetMaxWidth(config.Element, value);
        return config;
    }

    /// <summary>
    /// Sets the maximum width for the specified bottom sheet element.
    /// </summary>
    /// <param name="element">The bottom sheet element.</param>
    /// <param name="value">The maximum width value.</param>
    public static void SetMaxWidth(BindableObject element, int value)
    {
        element.SetValue(MaxWidthProperty, value);
    }

    /// <summary>
    /// Sets the maximum height using platform configuration.
    /// </summary>
    /// <param name="config">The Android configuration for the bottom sheet instance.</param>
    /// <param name="value">The maximum height value.</param>
    /// <returns>The platform element configuration for method chaining.</returns>
    public static IPlatformElementConfiguration<Android, Maui.BottomSheet.BottomSheet> SetMaxHeight(this IPlatformElementConfiguration<Android, Maui.BottomSheet.BottomSheet> config, int value)
    {
        SetMaxHeight(config.Element, value);
        return config;
    }

    /// <summary>
    /// Sets the maximum height for the specified bottom sheet element.
    /// </summary>
    /// <param name="element">The bottom sheet element.</param>
    /// <param name="value">The maximum height value.</param>
    public static void SetMaxHeight(BindableObject element, int value)
    {
        element.SetValue(MaxHeightProperty, value);
    }

    /// <summary>
    /// Gets the margin from the platform configuration.
    /// </summary>
    /// <param name="config">The Android configuration for the bottom sheet instance.</param>
    /// <returns>The margin thickness of the bottom sheet.</returns>
    public static Thickness GetMargin(this IPlatformElementConfiguration<Android, Maui.BottomSheet.BottomSheet> config)
    {
        return GetMargin(config.Element as BindableObject);
    }

    /// <summary>
    /// Gets the margin from the specified bottom sheet element.
    /// </summary>
    /// <param name="element">The bottom sheet element.</param>
    /// <returns>The margin thickness of the bottom sheet.</returns>
    public static Thickness GetMargin(BindableObject element)
    {
        return (Thickness)element.GetValue(MarginProperty);
    }

    /// <summary>
    /// Sets the margin using platform configuration.
    /// </summary>
    /// <param name="config">The Android configuration for the bottom sheet instance.</param>
    /// <param name="value">The margin thickness value.</param>
    public static void SetMargin(this IPlatformElementConfiguration<Android, Maui.BottomSheet.BottomSheet> config, Thickness value)
    {
        SetMargin(config.Element, value);
    }

    /// <summary>
    /// Sets the margin for the specified bottom sheet element.
    /// </summary>
    /// <param name="element">The bottom sheet element.</param>
    /// <param name="value">The margin thickness value.</param>
    public static void SetMargin(BindableObject element, Thickness value)
    {
        element.SetValue(MarginProperty, value);
    }

    /// <summary>
    /// Gets the half expanded ratio from the specified bottom sheet interface (internal use).
    /// </summary>
    /// <param name="element">The bottom sheet element.</param>
    /// <returns>The half expanded ratio value.</returns>
    internal static float GetHalfExpandedRatio(this IBottomSheet element)
    {
        if (element is not BindableObject bindable)
        {
            throw new ArgumentException("Element must be a BindableObject");
        }

        return GetHalfExpandedRatio(bindable);
    }

    /// <summary>
    /// Sets the half expanded ratio for the specified bottom sheet interface (internal use).
    /// </summary>
    /// <param name="element">The bottom sheet element.</param>
    /// <param name="value">The half expanded ratio value.</param>
    internal static void SetHalfExpandedRatio(this IBottomSheet element, float value)
    {
        if (element is not BindableObject bindable)
        {
            throw new ArgumentException("Element must be a BindableObject");
        }

        SetHalfExpandedRatio(bindable, value);
    }

    /// <summary>
    /// Gets the maximum width from the specified bottom sheet interface (internal use).
    /// </summary>
    /// <param name="element">The bottom sheet element.</param>
    /// <returns>The maximum width value.</returns>
    internal static int GetMaxWidth(this IBottomSheet element)
    {
        if (element is not BindableObject bindable)
        {
            throw new ArgumentException("Element must be a BindableObject");
        }

        return GetMaxWidth(bindable);
    }

    /// <summary>
    /// Gets the maximum height from the specified bottom sheet interface (internal use).
    /// </summary>
    /// <param name="element">The bottom sheet element.</param>
    /// <returns>The maximum height value.</returns>
    internal static int GetMaxHeight(this IBottomSheet element)
    {
        if (element is not BindableObject bindable)
        {
            throw new ArgumentException("Element must be a BindableObject");
        }

        return GetMaxHeight(bindable);
    }

    /// <summary>
    /// Gets the margin from the specified bottom sheet interface (internal use).
    /// </summary>
    /// <param name="element">The bottom sheet element.</param>
    /// <returns>The margin thickness of the bottom sheet.</returns>
    internal static Thickness GetMargin(this IBottomSheet element)
    {
        if (element is not BindableObject bindable)
        {
            throw new ArgumentException("Element must be a BindableObject");
        }

        return GetMargin(bindable);
    }

    /// <summary>
    /// Sets the theme resource identifier for the specified bindable object.
    /// </summary>
    /// <param name="element">The bindable object element.</param>
    /// <param name="value">The theme resource identifier.</param>
    private static void SetTheme(BindableObject element, int value)
    {
        element.SetValue(ThemeProperty, value);
    }

    /// <summary>
    /// Gets the theme resource identifier from the specified bindable object.
    /// </summary>
    /// <param name="element">The bindable object element.</param>
    /// <returns>The theme resource identifier.</returns>
    private static int GetTheme(BindableObject element)
    {
        return (int)element.GetValue(ThemeProperty);
    }
}