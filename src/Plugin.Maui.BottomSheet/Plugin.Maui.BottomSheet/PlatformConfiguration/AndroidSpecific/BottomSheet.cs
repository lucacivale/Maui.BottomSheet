namespace Plugin.Maui.BottomSheet.PlatformConfiguration.AndroidSpecific;

using Microsoft.Maui.Controls.PlatformConfiguration;

/// <summary>
/// Represents a customizable bottom sheet component for displaying content or actions
/// at the bottom of the screen, commonly used for contextual menus or additional UI.
/// </summary>
public static class BottomSheet
{
    /// <summary>
    /// Bindable property that determines the theme resource identifier.
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
    /// Bindable property for the maximum width constraint.
    /// </summary>
    public static readonly BindableProperty MaxWidthProperty =
        BindableProperty.Create(
            "MaxWidth",
            typeof(int),
            typeof(BottomSheet),
            defaultValue: int.MinValue);

    /// <summary>
    /// Bindable property for specifying the maximum height constraint of a visual element.
    /// </summary>
    public static readonly BindableProperty MaxHeightProperty =
        BindableProperty.Create(
            "MaxHeight",
            typeof(int),
            typeof(BottomSheet),
            defaultValue: int.MinValue);

    /// <summary>
    /// Bindable property that defines the ratio of the component's half-expanded state relative to its full height.
    /// </summary>
    public static readonly BindableProperty HalfExpandedRatioProperty =
        BindableProperty.Create(
            "HalfExpandedRatio",
            typeof(float),
            typeof(BottomSheet),
            defaultValue: 0.5f,
            propertyChanged: HalfExpandedRatioPropertyChanged,
            validateValue: (bindable, value) =>
            {
                if (bindable.IsSet(HalfExpandedRatioProperty) == false)
                {
                    return true;
                }

                bool isValid = (float)value > 0;

                if (isValid == false)
                {
                    System.Diagnostics.Trace.TraceError("HalfExpandedRatio must be greater than 0");
                }

                return isValid;
            });

    /// <summary>
    /// Bindable property for specifying the margin values around a UI element.
    /// </summary>
    public static readonly BindableProperty MarginProperty =
        BindableProperty.Create(
            "Margin",
            typeof(Thickness),
            typeof(BottomSheet),
            defaultValue: Thickness.Zero,
            propertyChanged: MarginPropertyChanged);

    /// <summary>
    /// Updates the half-expanded ratio value of the component based on the provided configuration.
    /// </summary>
    /// <param name="config">The configuration instance specific to the platform.</param>
    /// <param name="value">The desired ratio value for the half-expanded state.</param>
    /// <returns>The updated configuration object enabling method chaining.</returns>
    public static IPlatformElementConfiguration<Android, Maui.BottomSheet.BottomSheet> SetHalfExpandedRatio(this IPlatformElementConfiguration<Android, Maui.BottomSheet.BottomSheet> config, float value)
    {
        SetHalfExpandedRatio(config.Element as BindableObject, value);
        return config;
    }

    /// <summary>
    /// Configures the half expanded ratio for a bottom sheet.
    /// </summary>
    /// <param name="element">Bottom sheet instance.</param>
    /// <param name="value">The desired ratio value for the half-expanded state.</param>
    public static void SetHalfExpandedRatio(BindableObject element, float value)
    {
        element.SetValue(HalfExpandedRatioProperty, value);
    }

    /// <summary>
    /// Gets the half expanded ratio for the bottom sheet from the platform configuration.
    /// </summary>
    /// <param name="config">The Android configuration for the bottom sheet instance.</param>
    /// <returns>The half expanded ratio value of the bottom sheet.</returns>
    public static float GetHalfExpandedRatio(this IPlatformElementConfiguration<Android, Maui.BottomSheet.BottomSheet> config)
    {
        return GetHalfExpandedRatio(config.Element as BindableObject);
    }

    /// <summary>
    /// Retrieves the half expanded ratio for the bottom sheet using platform configuration.
    /// </summary>
    /// <param name="element">Bottom sheet instance.</param>
    /// <returns>The half expanded ratio value.</returns>
    public static float GetHalfExpandedRatio(BindableObject element)
    {
        return (float)element.GetValue(HalfExpandedRatioProperty);
    }

    /// <summary>
    /// Retrieves the current theme applied to the application or specific component.
    /// </summary>
    /// <param name="config">The Android configuration for the bottom sheet instance.</param>
    /// <returns>The active theme instance representing the current styling and appearance settings.</returns>
    public static int GetTheme(this IPlatformElementConfiguration<Android, Maui.BottomSheet.BottomSheet> config)
    {
        return GetTheme(config.Element as BindableObject);
    }

    /// <summary>
    /// Retrieves the current theme applied to the application.
    /// </summary>
    /// <param name="element">The bottom sheet element for which to retrieve the theme.</param>
    /// <returns>The current theme as an object representing the application's visual styling.</returns>
    public static int GetTheme(this IBottomSheet element)
    {
        if (element is not BindableObject bindable)
        {
            throw new ArgumentException("Element must be a BindableObject");
        }

        return GetTheme(bindable);
    }

    /// <summary>
    /// Applies a specific theme to the application or a component.
    /// </summary>
    /// <param name="config">The Android configuration for the bottom sheet instance.</param>
    /// <param name="value">Theme Id.</param>
    /// <returns>A boolean value indicating whether the theme was successfully applied.</returns>
    public static IPlatformElementConfiguration<Android, Maui.BottomSheet.BottomSheet> SetTheme(this IPlatformElementConfiguration<Android, Maui.BottomSheet.BottomSheet> config, int value)
    {
        SetTheme(config.Element as BindableObject, value);
        return config;
    }

    /// <summary>
    /// Sets the theme for the specified bottom sheet element with the provided value.
    /// </summary>
    /// <param name="element">The bottom sheet element to which the theme will be applied.</param>
    /// <param name="value">The integer value representing the theme to be set.</param>
    /// <exception cref="ArgumentException">Thrown when the provided element is not a BindableObject.</exception>
    public static void SetTheme(this IBottomSheet element, int value)
    {
        if (element is not BindableObject bindable)
        {
            throw new ArgumentException("Element must be a BindableObject");
        }

        SetTheme(bindable, value);
    }

    /// <summary>
    /// Retrieves the maximum width configured for the BottomSheet element.
    /// </summary>
    /// <param name="config">The platform-specific configuration for the BottomSheet element.</param>
    /// <returns>The calculated maximum width value for the BottomSheet element.</returns>
    public static int GetMaxWidth(this IPlatformElementConfiguration<Android, Maui.BottomSheet.BottomSheet> config)
    {
        return GetMaxWidth(config.Element as BindableObject);
    }

    /// <summary>
    /// Retrieves the maximum width defined for the BottomSheet component.
    /// </summary>
    /// <param name="element">The bindable object associated with the BottomSheet instance.</param>
    /// <returns>The maximum width value specified for the BottomSheet.</returns>
    public static int GetMaxWidth(BindableObject element)
    {
        return (int)element.GetValue(MaxWidthProperty);
    }

    /// <summary>
    /// Retrieves the maximum height defined for the specified bindable object element.
    /// </summary>
    /// <param name="element">The bindable object from which to retrieve the maximum height value.</param>
    /// <returns>The maximum height value associated with the specified element.</returns>
    public static int GetMaxHeight(BindableObject element)
    {
        return (int)element.GetValue(MaxHeightProperty);
    }

    /// <summary>
    /// Retrieves the maximum height value for the specified layout configuration.
    /// </summary>
    /// <param name="config">The platform-specific configuration object for the Android BottomSheet.</param>
    /// <returns>The maximum height value configured for the BottomSheet.</returns>
    public static int GetMaxHeight(this IPlatformElementConfiguration<Android, Maui.BottomSheet.BottomSheet> config)
    {
        return GetMaxHeight(config.Element as BindableObject);
    }

    /// <summary>
    /// Configures the maximum width for the BottomSheet based on the specified platform configuration.
    /// </summary>
    /// <param name="config">The platform-specific configuration for the BottomSheet element.</param>
    /// <param name="value">The maximum width value to apply to the element.</param>
    /// <returns>The updated platform element configuration allowing for method chaining.</returns>
    public static IPlatformElementConfiguration<Android, Maui.BottomSheet.BottomSheet> SetMaxWidth(this IPlatformElementConfiguration<Android, Maui.BottomSheet.BottomSheet> config, int value)
    {
        SetMaxWidth(config.Element, value);
        return config;
    }

    /// <summary>
    /// Sets the maximum allowable width for the element.
    /// </summary>
    /// <param name="element">The bindable object for which the maximum width is being set.</param>
    /// <param name="value">The value representing the maximum width to be applied.</param>
    public static void SetMaxWidth(BindableObject element, int value)
    {
        element.SetValue(MaxWidthProperty, value);
    }

    /// <summary>
    /// Sets the maximum height for the component using the specified configuration.
    /// </summary>
    /// <param name="config">The platform-specific configuration for the component.</param>
    /// <param name="value">The maximum height value to set.</param>
    /// <returns>The platform element configuration for method chaining.</returns>
    public static IPlatformElementConfiguration<Android, Maui.BottomSheet.BottomSheet> SetMaxHeight(this IPlatformElementConfiguration<Android, Maui.BottomSheet.BottomSheet> config, int value)
    {
        SetMaxHeight(config.Element, value);
        return config;
    }

    /// <summary>
    /// Sets the maximum height for the bottom sheet on the specified element.
    /// </summary>
    /// <param name="element">The bindable object representing the bottom sheet instance.</param>
    /// <param name="value">The desired maximum height value for the bottom sheet.</param>
    public static void SetMaxHeight(BindableObject element, int value)
    {
        element.SetValue(MaxHeightProperty, value);
    }

    /// <summary>
    /// Retrieves the margin value for the specified UI element in the Android-specific configuration.
    /// </summary>
    /// <param name="config">The platform-specific configuration for Android.</param>
    /// <returns>The margin values represented as a Thickness object.</returns>
    public static Thickness GetMargin(this IPlatformElementConfiguration<Android, Maui.BottomSheet.BottomSheet> config)
    {
        return GetMargin(config.Element as BindableObject);
    }

    /// <summary>
    /// Retrieves the margin value associated with the specified bindable object.
    /// </summary>
    /// <param name="element">The bindable object for which the margin value is to be retrieved.</param>
    /// <returns>The margin value of the specified bindable object.</returns>
    public static Thickness GetMargin(BindableObject element)
    {
        return (Thickness)element.GetValue(MarginProperty);
    }

    /// <summary>
    /// Configures the margin for the bottom sheet element.
    /// </summary>
    /// <param name="config">The platform-specific configuration for the bottom sheet.</param>
    /// <param name="value">The thickness value representing the margin to set.</param>
    public static void SetMargin(this IPlatformElementConfiguration<Android, Maui.BottomSheet.BottomSheet> config, Thickness value)
    {
        SetMargin(config.Element, value);
    }

    /// <summary>
    /// Configures the margin for a specific UI element within the BottomSheet component.
    /// </summary>
    /// <param name="element">The UI element to which the margin should be applied.</param>
    /// <param name="value">The margin value to set for the specified element.</param>
    public static void SetMargin(BindableObject element, Thickness value)
    {
        element.SetValue(MarginProperty, value);
    }

    /// <summary>
    /// Retrieves the half-expanded ratio value of the bottom sheet based on the platform-specific configuration.
    /// </summary>
    /// <param name="element">The bottom sheet instance implementing the IBottomSheet interface.</param>
    /// <returns>The current half-expanded ratio value.</returns>
    internal static float GetHalfExpandedRatio(this IBottomSheet element)
    {
        if (element is not BindableObject bindable)
        {
            throw new ArgumentException("Element must be a BindableObject");
        }

        return GetHalfExpandedRatio(bindable);
    }

    /// <summary>
    /// Sets the ratio that defines the half-expanded state for a bottom sheet component.
    /// </summary>
    /// <param name="element">The bottom sheet instance for which the ratio is being configured.</param>
    /// <param name="value">The ratio value to apply for the half-expanded state.</param>
    /// <exception cref="ArgumentException">Thrown when the provided element is not a valid BindableObject.</exception>
    internal static void SetHalfExpandedRatio(this IBottomSheet element, float value)
    {
        if (element is not BindableObject bindable)
        {
            throw new ArgumentException("Element must be a BindableObject");
        }

        SetHalfExpandedRatio(bindable, value);
    }

    /// <summary>
    /// Retrieves the maximum allowable width for the bottom sheet based on the specified bindable object.
    /// </summary>
    /// <param name="element">The bindable object representing the bottom sheet component.</param>
    /// <returns>The maximum width value for the bottom sheet as defined in the platform configuration.</returns>
    internal static int GetMaxWidth(this IBottomSheet element)
    {
        if (element is not BindableObject bindable)
        {
            throw new ArgumentException("Element must be a BindableObject");
        }

        return GetMaxWidth(bindable);
    }

    /// <summary>
    /// Retrieves the maximum height defined for the given bindable UI element.
    /// </summary>
    /// <param name="element">The bindable UI element for which to get the maximum height.</param>
    /// <returns>The maximum height value configured for the specified element.</returns>
    internal static int GetMaxHeight(this IBottomSheet element)
    {
        if (element is not BindableObject bindable)
        {
            throw new ArgumentException("Element must be a BindableObject");
        }

        return GetMaxHeight(bindable);
    }

    /// <summary>
    /// Retrieves the margin value for the specified bindable layout element.
    /// </summary>
    /// <param name="element">The bindable layout element for which the margin is being retrieved.</param>
    /// <returns>The margin value of the specified bindable layout element.</returns>
    internal static Thickness GetMargin(this IBottomSheet element)
    {
        if (element is not BindableObject bindable)
        {
            throw new ArgumentException("Element must be a BindableObject");
        }

        return GetMargin(bindable);
    }

    /// <summary>
    /// Sets the theme resource identifier for the bottom sheet element.
    /// </summary>
    /// <param name="element">The bottom sheet element for which to set the theme.</param>
    /// <param name="value">The identifier of the theme resource to be applied.</param>
    private static void SetTheme(BindableObject element, int value)
    {
        element.SetValue(ThemeProperty, value);
    }

    /// <summary>
    /// Retrieves the theme value associated with the provided BottomSheet configuration.
    /// </summary>
    /// <param name="element">The bottom sheet element for which to set the theme.</param>
    /// <returns>An integer representing the theme value.</returns>
    private static int GetTheme(BindableObject element)
    {
        return (int)element.GetValue(ThemeProperty);
    }

    /// <summary>
    /// Called when the Margin property of the bottom sheet changes.
    /// </summary>
    /// <param name="bindable">The bindable object where the property change originated.</param>
    /// <param name="oldValue">The previous value of the Margin property.</param>
    /// <param name="newValue">The new value of the Margin property.</param>
    private static void MarginPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is Element element)
        {
            element.Handler?.Invoke(nameof(SetMargin));
        }
    }

    /// <summary>
    /// Handles the property change event for the HalfExpandedRatio property.
    /// </summary>
    /// <param name="bindable">The bindable object on which the HalfExpandedRatio property is changed.</param>
    /// <param name="oldValue">The old value of the HalfExpandedRatio property before the change.</param>
    /// <param name="newValue">The new value of the HalfExpandedRatio property after the change.</param>
    private static void HalfExpandedRatioPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is Element element)
        {
            element.Handler?.Invoke(nameof(SetHalfExpandedRatio));
        }
    }
}