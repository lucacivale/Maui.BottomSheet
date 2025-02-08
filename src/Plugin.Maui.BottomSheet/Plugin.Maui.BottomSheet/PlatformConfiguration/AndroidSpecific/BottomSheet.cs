namespace Plugin.Maui.BottomSheet.PlatformConfiguration.AndroidSpecific;

using Microsoft.Maui.Controls.PlatformConfiguration;

/// <summary>
/// BottomSheet platform configuration for Android.
/// </summary>
public static class BottomSheet
{
    /// <summary>
    /// Bindable property.
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
    /// Bindable property.
    /// </summary>
    public static readonly BindableProperty MaxWidthProperty =
        BindableProperty.Create(
            "MaxWidth",
            typeof(int),
            typeof(BottomSheet),
            defaultValue: int.MinValue);

    /// <summary>
    /// Bindable property.
    /// </summary>
    public static readonly BindableProperty MaxHeightProperty =
        BindableProperty.Create(
            "MaxHeight",
            typeof(int),
            typeof(BottomSheet),
            defaultValue: int.MinValue);

    /// <summary>
    /// Bindable property.
    /// </summary>
    public static readonly BindableProperty HalfExpandedRatioProperty =
        BindableProperty.Create(
            "HalfExpandedRatio",
            typeof(float),
            typeof(BottomSheet),
            defaultValue: 0.5f);

    /// <summary>
    /// Bindable property.
    /// </summary>
    public static readonly BindableProperty MarginProperty =
        BindableProperty.Create(
            "Margin",
            typeof(Thickness),
            typeof(BottomSheet),
            defaultValue: Thickness.Zero);

    /// <summary>
    /// Set half expanded ratio.
    /// </summary>
    /// <param name="config">Android configuration for <see cref="Maui.BottomSheet"/> instance.</param>.
    /// <param name="value">Half expanded ratio.</param>
    /// <returns><see cref="IPlatformElementConfiguration{TPlatform, TElement}"/>.</returns>
    public static IPlatformElementConfiguration<Android, Maui.BottomSheet.BottomSheet> SetHalfExpandedRatio(this IPlatformElementConfiguration<Android, Maui.BottomSheet.BottomSheet> config, float value)
    {
        SetHalfExpandedRatio(config.Element as BindableObject, value);
        return config;
    }

    /// <summary>
    /// Set half expanded ratio.
    /// </summary>
    /// <param name="element"><see cref="Maui.BottomSheet"/> instance.</param>
    /// <param name="value">Ratio.</param>
    public static void SetHalfExpandedRatio(BindableObject element, float value)
    {
        element.SetValue(HalfExpandedRatioProperty, value);
    }

    /// <summary>
    /// Get half expanded ratio.
    /// </summary>
    /// <param name="config">Android configuration for <see cref="Maui.BottomSheet"/> instance.</param>
    /// <returns>Half expanded ratio.</returns>
    public static float GetHalfExpandedRatio(this IPlatformElementConfiguration<Android, Maui.BottomSheet.BottomSheet> config)
    {
        return GetHalfExpandedRatio(config.Element as BindableObject);
    }

    /// <summary>
    /// Get half expanded ratio.
    /// </summary>
    /// <param name="element"><see cref="Maui.BottomSheet"/> instance.</param>
    /// <returns>Ratio.</returns>
    public static float GetHalfExpandedRatio(BindableObject element)
    {
        return (float)element.GetValue(HalfExpandedRatioProperty);
    }

    /// <summary>
    /// Get theme resource id.
    /// </summary>
    /// <param name="config">Android configuration for <see cref="Maui.BottomSheet"/> instance.</param>
    /// <returns>Associated theme resource id.</returns>
    public static int GetTheme(this IPlatformElementConfiguration<Android, Maui.BottomSheet.BottomSheet> config)
    {
        return GetTheme(config.Element as BindableObject);
    }

    /// <summary>
    /// Get theme resource id.
    /// </summary>
    /// <param name="element"><see cref="Maui.BottomSheet"/> instance.</param>
    /// <returns>Associated theme resource id.</returns>
    public static int GetTheme(this IBottomSheet element)
    {
        if (element is not BindableObject bindable)
        {
            throw new ArgumentException("Element must be a BindableObject");
        }

        return GetTheme(bindable);
    }

    /// <summary>
    /// Set theme resource id.
    /// </summary>
    /// <param name="config">Android configuration for <see cref="Maui.BottomSheet"/> instance.</param>.
    /// <param name="value">Theme resource id.</param>
    /// <returns><see cref="IPlatformElementConfiguration{TPlatform, TElement}"/>.</returns>
    public static IPlatformElementConfiguration<Android, Maui.BottomSheet.BottomSheet> SetTheme(this IPlatformElementConfiguration<Android, Maui.BottomSheet.BottomSheet> config, int value)
    {
        SetTheme(config.Element as BindableObject, value);
        return config;
    }

    /// <summary>
    /// Set theme resource id.
    /// </summary>
    /// <param name="element"><see cref="Maui.BottomSheet"/> instance.</param>
    /// <param name="value">Theme resource id.</param>
    public static void SetTheme(this IBottomSheet element, int value)
    {
        if (element is not BindableObject bindable)
        {
            throw new ArgumentException("Element must be a BindableObject");
        }

        SetTheme(bindable, value);
    }

    /// <summary>
    /// Get <see cref="Maui.BottomSheet"/> max width.
    /// </summary>
    /// <param name="config">Android configuration for <see cref="Maui.BottomSheet"/> instance.</param>
    /// <returns>Max width.</returns>
    public static int GetMaxWidth(this IPlatformElementConfiguration<Android, Maui.BottomSheet.BottomSheet> config)
    {
        return GetMaxWidth(config.Element as BindableObject);
    }

    /// <summary>
    /// Get <see cref="Maui.BottomSheet"/> max width.
    /// </summary>
    /// <param name="element"><see cref="Maui.BottomSheet"/> instance.</param>
    /// <returns>Max width.</returns>
    public static int GetMaxWidth(BindableObject element)
    {
        return (int)element.GetValue(MaxWidthProperty);
    }

    /// <summary>
    /// Get <see cref="Maui.BottomSheet"/> max height.
    /// </summary>
    /// <param name="element"><see cref="Maui.BottomSheet"/> instance.</param>
    /// <returns>Max height.</returns>
    public static int GetMaxHeight(BindableObject element)
    {
        return (int)element.GetValue(MaxHeightProperty);
    }

    /// <summary>
    /// Get <see cref="Maui.BottomSheet"/> max height.
    /// </summary>
    /// <param name="config">Android configuration for <see cref="Maui.BottomSheet"/> instance.</param>
    /// <returns>Max height.</returns>
    public static int GetMaxHeight(this IPlatformElementConfiguration<Android, Maui.BottomSheet.BottomSheet> config)
    {
        return GetMaxHeight(config.Element as BindableObject);
    }

    /// <summary>
    /// Set <see cref="Maui.BottomSheet"/> max width.
    /// </summary>
    /// <param name="config">Android configuration for <see cref="Maui.BottomSheet"/> instance.</param>.
    /// <param name="value">Max width.</param>
    /// <returns><see cref="IPlatformElementConfiguration{TPlatform, TElement}"/>.</returns>
    public static IPlatformElementConfiguration<Android, Maui.BottomSheet.BottomSheet> SetMaxWidth(this IPlatformElementConfiguration<Android, Maui.BottomSheet.BottomSheet> config, int value)
    {
        SetMaxWidth(config.Element, value);
        return config;
    }

    /// <summary>
    /// Set max width.
    /// </summary>
    /// <param name="element"><see cref="Maui.BottomSheet"/> instance.</param>
    /// <param name="value">Max width.</param>
    public static void SetMaxWidth(BindableObject element, int value)
    {
        element.SetValue(MaxWidthProperty, value);
    }

    /// <summary>
    /// Set <see cref="Maui.BottomSheet"/> max height.
    /// </summary>
    /// <param name="config">Android configuration for <see cref="Maui.BottomSheet"/> instance.</param>.
    /// <param name="value">Max height.</param>
    /// <returns><see cref="IPlatformElementConfiguration{TPlatform, TElement}"/>.</returns>
    public static IPlatformElementConfiguration<Android, Maui.BottomSheet.BottomSheet> SetMaxHeight(this IPlatformElementConfiguration<Android, Maui.BottomSheet.BottomSheet> config, int value)
    {
        SetMaxHeight(config.Element, value);
        return config;
    }

    /// <summary>
    /// Set max height.
    /// </summary>
    /// <param name="element"><see cref="Maui.BottomSheet"/> instance.</param>
    /// <param name="value">Max height.</param>
    public static void SetMaxHeight(BindableObject element, int value)
    {
        element.SetValue(MaxHeightProperty, value);
    }

    /// <summary>
    /// Get margin.
    /// </summary>
    /// <param name="config">Android configuration for <see cref="Maui.BottomSheet"/> instance.</param>.
    /// <returns><see cref="IPlatformElementConfiguration{TPlatform, TElement}"/>.</returns>
    public static Thickness GetMargin(this IPlatformElementConfiguration<Android, Maui.BottomSheet.BottomSheet> config)
    {
        return GetMargin(config.Element as BindableObject);
    }

    /// <summary>
    /// Get margin.
    /// </summary>
    /// <param name="element"><see cref="Maui.BottomSheet"/> instance.</param>
    /// <returns>Margin of <see cref="Maui.BottomSheet"/>.</returns>
    public static Thickness GetMargin(BindableObject element)
    {
        return (Thickness)element.GetValue(MarginProperty);
    }

    /// <summary>
    /// Set margin.
    /// </summary>
    /// <param name="config">Android configuration for <see cref="Maui.BottomSheet"/> instance.</param>.
    /// <param name="value"><see cref="Maui.BottomSheet"/> margin.</param>
    public static void SetMargin(this IPlatformElementConfiguration<Android, Maui.BottomSheet.BottomSheet> config, Thickness value)
    {
        SetMargin(config.Element, value);
    }

    /// <summary>
    /// Set margin.
    /// </summary>
    /// <param name="element"><see cref="Maui.BottomSheet"/> instance.</param>
    /// <param name="value"><see cref="Maui.BottomSheet"/> margin.</param>
    public static void SetMargin(BindableObject element, Thickness value)
    {
        element.SetValue(MarginProperty, value);
    }

    /// <summary>
    /// Get half expanded ratio.
    /// </summary>
    /// <param name="element"><see cref="Maui.BottomSheet"/> instance.</param>
    /// <returns>Half expanded ratio.</returns>
    internal static float GetHalfExpandedRatio(this IBottomSheet element)
    {
        if (element is not BindableObject bindable)
        {
            throw new ArgumentException("Element must be a BindableObject");
        }

        return GetHalfExpandedRatio(bindable);
    }

    /// <summary>
    /// Set half expanded ratio.
    /// </summary>
    /// <param name="element"><see cref="Maui.BottomSheet"/> instance.</param>
    /// <param name="value">Half expanded ratio.</param>
    internal static void SetHalfExpandedRatio(this IBottomSheet element, float value)
    {
        if (element is not BindableObject bindable)
        {
            throw new ArgumentException("Element must be a BindableObject");
        }

        SetHalfExpandedRatio(bindable, value);
    }

    /// <summary>
    /// Get <see cref="Maui.BottomSheet"/> max width.
    /// </summary>
    /// <param name="element"><see cref="Maui.BottomSheet"/> instance.</param>
    /// <returns>Max width.</returns>
    internal static int GetMaxWidth(this IBottomSheet element)
    {
        if (element is not BindableObject bindable)
        {
            throw new ArgumentException("Element must be a BindableObject");
        }

        return GetMaxWidth(bindable);
    }

    /// <summary>
    /// Get <see cref="Maui.BottomSheet"/> max height.
    /// </summary>
    /// <param name="element"><see cref="Maui.BottomSheet"/> instance.</param>
    /// <returns>Max height.</returns>
    internal static int GetMaxHeight(this IBottomSheet element)
    {
        if (element is not BindableObject bindable)
        {
            throw new ArgumentException("Element must be a BindableObject");
        }

        return GetMaxHeight(bindable);
    }

    /// <summary>
    /// Get margin.
    /// </summary>
    /// <param name="element"><see cref="Maui.BottomSheet"/> instance.</param>
    /// <returns>Margin of <see cref="Maui.BottomSheet"/>.</returns>
    internal static Thickness GetMargin(this IBottomSheet element)
    {
        if (element is not BindableObject bindable)
        {
            throw new ArgumentException("Element must be a BindableObject");
        }

        return GetMargin(bindable);
    }

    private static void SetTheme(BindableObject element, int value)
    {
        element.SetValue(ThemeProperty, value);
    }

    private static int GetTheme(BindableObject element)
    {
        return (int)element.GetValue(ThemeProperty);
    }
}