#if ANDROID
using ABottomSheet = Plugin.Maui.BottomSheet.Platform.Android.MauiBottomSheet;
#endif

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
    public static int GetMaxWidth(this IBottomSheet element)
    {
        if (element is not BindableObject bindable)
        {
            throw new ArgumentException("Element must be a BindableObject");
        }

        return GetMaxWidth(bindable);
    }

    public static int GetMaxWidth(BindableObject element)
    {
        return (int)element.GetValue(MaxWidthProperty);
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
    /// Get <see cref="Maui.BottomSheet"/> max height.
    /// </summary>
    /// <param name="element"><see cref="Maui.BottomSheet"/> instance.</param>
    /// <returns>Max height.</returns>
    public static int GetMaxHeight(this IBottomSheet element)
    {
        if (element is not BindableObject bindable)
        {
            throw new ArgumentException("Element must be a BindableObject");
        }

        return GetMaxHeight(bindable);
    }

    public static int GetMaxHeight(BindableObject element)
    {
        return (int)element.GetValue(MaxHeightProperty);
    }

    /// <summary>
    /// Set <see cref="Maui.BottomSheet"/> max width.
    /// </summary>
    /// <param name="config">Android configuration for <see cref="Maui.BottomSheet"/> instance.</param>.
    /// <param name="value">Max width.</param>
    /// <returns><see cref="IPlatformElementConfiguration{TPlatform, TElement}"/>.</returns>
    public static IPlatformElementConfiguration<Android, Maui.BottomSheet.BottomSheet> SetMaxWidth(this IPlatformElementConfiguration<Android, Maui.BottomSheet.BottomSheet> config, int value)
    {
        SetMaxWidth(config.Element as BindableObject, value);
        return config;
    }

    /// <summary>
    /// Set <see cref="Maui.BottomSheet"/> max width.
    /// </summary>
    /// <param name="element"><see cref="Maui.BottomSheet"/> instance.</param>
    /// <param name="value">Max width.</param>
    public static void SetMaxWidth(this IBottomSheet element, int value)
    {
        if (element is not BindableObject bindable)
        {
            throw new ArgumentException("Element must be a BindableObject");
        }

        SetMaxWidth(bindable, value);
    }

    public static void SetMaxWidth(BindableObject element, int value)
    {
        element.SetValue(MaxWidthProperty, value);

#if ANDROID
        if (element is Maui.BottomSheet.BottomSheet bottomSheet
            && bottomSheet.Handler?.PlatformView is ABottomSheet mauiBottomSheet)
        {
            mauiBottomSheet.SetMaxWidth(value);
        }
#endif
    }

    /// <summary>
    /// Set <see cref="Maui.BottomSheet"/> max height.
    /// </summary>
    /// <param name="config">Android configuration for <see cref="Maui.BottomSheet"/> instance.</param>.
    /// <param name="value">Max height.</param>
    /// <returns><see cref="IPlatformElementConfiguration{TPlatform, TElement}"/>.</returns>
    public static IPlatformElementConfiguration<Android, Maui.BottomSheet.BottomSheet> SetMaxHeight(this IPlatformElementConfiguration<Android, Maui.BottomSheet.BottomSheet> config, int value)
    {
        SetMaxHeight(config.Element as BindableObject, value);
        return config;
    }

    /// <summary>
    /// Set <see cref="Maui.BottomSheet"/> max height.
    /// </summary>
    /// <param name="element"><see cref="Maui.BottomSheet"/> instance.</param>
    /// <param name="value">Max height.</param>
    public static void SetMaxHeight(this IBottomSheet element, int value)
    {
        if (element is not BindableObject bindable)
        {
            throw new ArgumentException("Element must be a BindableObject");
        }

        SetMaxHeight(bindable, value);
    }

    public static void SetMaxHeight(BindableObject element, int value)
    {
        element.SetValue(MaxHeightProperty, value);

#if ANDROID
        if (element is Maui.BottomSheet.BottomSheet bottomSheet
            && bottomSheet.Handler?.PlatformView is ABottomSheet mauiBottomSheet)
        {
            mauiBottomSheet.SetMaxHeight(value);
        }
#endif
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