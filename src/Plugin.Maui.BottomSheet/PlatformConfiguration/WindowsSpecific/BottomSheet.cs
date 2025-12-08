namespace Plugin.Maui.BottomSheet.PlatformConfiguration.WindowsSpecific;

using Microsoft.Maui.Controls.PlatformConfiguration;

/// <summary>
/// Represents a customizable bottom sheet component for displaying content or actions
/// at the bottom of the screen, commonly used for contextual menus or additional UI.
/// </summary>
public static class BottomSheet
{
    /// <summary>
    /// Bindable property for the maximum width constradouble.
    /// </summary>
    public static readonly BindableProperty MaxWidthProperty =
        BindableProperty.Create(
            "MaxWidth",
            typeof(double),
            typeof(BottomSheet),
            defaultValue: double.NaN,
            propertyChanged: MaxWidthPropertyChanged);

    /// <summary>
    /// Bindable property for specifying the maximum height constradouble of a visual element.
    /// </summary>
    public static readonly BindableProperty MaxHeightProperty =
        BindableProperty.Create(
            "MaxHeight",
            typeof(double),
            typeof(BottomSheet),
            defaultValue: double.NaN,
            propertyChanged: MaxHeightPropertyChanged);

    /// <summary>
    /// Bindable property for the minimum width constradouble.
    /// </summary>
    public static readonly BindableProperty MinWidthProperty =
        BindableProperty.Create(
            "MinWidth",
            typeof(double),
            typeof(BottomSheet),
            defaultValue: double.NaN,
            propertyChanged: MinWidthPropertyChanged);

    /// <summary>
    /// Bindable property for specifying the minimum height constradouble of a visual element.
    /// </summary>
    public static readonly BindableProperty MinHeightProperty =
        BindableProperty.Create(
            "MinHeight",
            typeof(double),
            typeof(BottomSheet),
            defaultValue: double.NaN,
            propertyChanged: MinHeightPropertyChanged);

    /// <summary>
    /// Retrieves the maximum width configured for the BottomSheet element.
    /// </summary>
    /// <param name="config">The platform-specific configuration for the BottomSheet element.</param>
    /// <returns>The calculated maximum width value for the BottomSheet element.</returns>
    public static double GetMaxWidth(this IPlatformElementConfiguration<Windows, Maui.BottomSheet.BottomSheet> config)
    {
        return GetMaxWidth(config.Element as BindableObject);
    }

    /// <summary>
    /// Retrieves the maximum width defined for the BottomSheet component.
    /// </summary>
    /// <param name="element">The bindable object associated with the BottomSheet instance.</param>
    /// <returns>The maximum width value specified for the BottomSheet.</returns>
    public static double GetMaxWidth(BindableObject element)
    {
        return (double)element.GetValue(MaxWidthProperty);
    }

    /// <summary>
    /// Retrieves the maximum height value for the specified layout configuration.
    /// </summary>
    /// <param name="config">The platform-specific configuration object for the Windows BottomSheet.</param>
    /// <returns>The maximum height value configured for the BottomSheet.</returns>
    public static double GetMaxHeight(this IPlatformElementConfiguration<Windows, Maui.BottomSheet.BottomSheet> config)
    {
        return GetMaxHeight(config.Element as BindableObject);
    }

    /// <summary>
    /// Retrieves the maximum height defined for the specified bindable object element.
    /// </summary>
    /// <param name="element">The bindable object from which to retrieve the maximum height value.</param>
    /// <returns>The maximum height value associated with the specified element.</returns>
    public static double GetMaxHeight(BindableObject element)
    {
        return (double)element.GetValue(MaxHeightProperty);
    }

    /// <summary>
    /// Retrieves the minimum width configured for the BottomSheet element.
    /// </summary>
    /// <param name="config">The platform-specific configuration for the BottomSheet element.</param>
    /// <returns>The calculated minimum width value for the BottomSheet element.</returns>
    public static double GetMinWidth(this IPlatformElementConfiguration<Windows, Maui.BottomSheet.BottomSheet> config)
    {
        return GetMinWidth(config.Element as BindableObject);
    }

    /// <summary>
    /// Retrieves the minimum width defined for the specified bindable object element.
    /// </summary>
    /// <param name="element">The bindable object from which to retrieve the minimum width value.</param>
    /// <returns>The minimum width value associated with the specified element.</returns>
    public static double GetMinWidth(BindableObject element)
    {
        return (double)element.GetValue(MinWidthProperty);
    }

    /// <summary>
    /// Retrieves the minimum height value for the specified layout configuration.
    /// </summary>
    /// <param name="config">The platform-specific configuration object for the Windows BottomSheet.</param>
    /// <returns>The minimum height value configured for the BottomSheet.</returns>
    public static double GetMinHeight(this IPlatformElementConfiguration<Windows, Maui.BottomSheet.BottomSheet> config)
    {
        return GetMinHeight(config.Element as BindableObject);
    }

    /// <summary>
    /// Retrieves the minimum height defined for the specified bindable object element.
    /// </summary>
    /// <param name="element">The bindable object from which to retrieve the minimum height value.</param>
    /// <returns>The minimum height value associated with the specified element.</returns>
    public static double GetMinHeight(BindableObject element)
    {
        return (double)element.GetValue(MinHeightProperty);
    }

    /// <summary>
    /// Configures the maximum width for the BottomSheet based on the specified platform configuration.
    /// </summary>
    /// <param name="config">The platform-specific configuration for the BottomSheet element.</param>
    /// <param name="value">The maximum width value to apply to the element.</param>
    /// <returns>The updated platform element configuration allowing for method chaining.</returns>
    public static IPlatformElementConfiguration<Windows, Maui.BottomSheet.BottomSheet> SetMaxWidth(this IPlatformElementConfiguration<Windows, Maui.BottomSheet.BottomSheet> config, double value)
    {
        SetMaxWidth(config.Element, value);
        return config;
    }

    /// <summary>
    /// Sets the maximum allowable width for the element.
    /// </summary>
    /// <param name="element">The bindable object for which the maximum width is being set.</param>
    /// <param name="value">The value representing the maximum width to be applied.</param>
    public static void SetMaxWidth(BindableObject element, double value)
    {
        element.SetValue(MaxWidthProperty, value);
    }

    /// <summary>
    /// Sets the maximum height for the component using the specified configuration.
    /// </summary>
    /// <param name="config">The platform-specific configuration for the component.</param>
    /// <param name="value">The maximum height value to set.</param>
    /// <returns>The platform element configuration for method chaining.</returns>
    public static IPlatformElementConfiguration<Windows, Maui.BottomSheet.BottomSheet> SetMaxHeight(this IPlatformElementConfiguration<Windows, Maui.BottomSheet.BottomSheet> config, double value)
    {
        SetMaxHeight(config.Element, value);
        return config;
    }

    /// <summary>
    /// Sets the maximum height for the bottom sheet on the specified element.
    /// </summary>
    /// <param name="element">The bindable object representing the bottom sheet instance.</param>
    /// <param name="value">The desired maximum height value for the bottom sheet.</param>
    public static void SetMaxHeight(BindableObject element, double value)
    {
        element.SetValue(MaxHeightProperty, value);
    }

    /// <summary>
    /// Sets the minimum width for the component using the specified configuration.
    /// </summary>
    /// <param name="config">The platform-specific configuration for the component.</param>
    /// <param name="value">The minimum width value to set.</param>
    /// <returns>The platform element configuration for method chaining.</returns>
    public static IPlatformElementConfiguration<Windows, Maui.BottomSheet.BottomSheet> SetMinWidth(this IPlatformElementConfiguration<Windows, Maui.BottomSheet.BottomSheet> config, double value)
    {
        SetMinWidth(config.Element, value);
        return config;
    }

    /// <summary>
    /// Sets the minimum width for the bottom sheet on the specified element.
    /// </summary>
    /// <param name="element">The bindable object representing the bottom sheet instance.</param>
    /// <param name="value">The desired minimum width value for the bottom sheet.</param>
    public static void SetMinWidth(BindableObject element, double value)
    {
        element.SetValue(MinWidthProperty, value);
    }

    /// <summary>
    /// Sets the minimum height for the component using the specified configuration.
    /// </summary>
    /// <param name="config">The platform-specific configuration for the component.</param>
    /// <param name="value">The minimum height value to set.</param>
    /// <returns>The platform element configuration for method chaining.</returns>
    public static IPlatformElementConfiguration<Windows, Maui.BottomSheet.BottomSheet> SetMinHeight(this IPlatformElementConfiguration<Windows, Maui.BottomSheet.BottomSheet> config, double value)
    {
        SetMinHeight(config.Element, value);
        return config;
    }

    /// <summary>
    /// Sets the minimum height for the bottom sheet on the specified element.
    /// </summary>
    /// <param name="element">The bindable object representing the bottom sheet instance.</param>
    /// <param name="value">The desired minimum height value for the bottom sheet.</param>
    public static void SetMinHeight(BindableObject element, double value)
    {
        element.SetValue(MinHeightProperty, value);
    }

    /// <summary>
    /// Retrieves the maximum allowable width for the bottom sheet based on the specified bindable object.
    /// </summary>
    /// <param name="element">The bindable object representing the bottom sheet component.</param>
    /// <returns>The maximum width value for the bottom sheet as defined in the platform configuration.</returns>
    internal static double GetMaxWidth(this IBottomSheet element)
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
    internal static double GetMaxHeight(this IBottomSheet element)
    {
        if (element is not BindableObject bindable)
        {
            throw new ArgumentException("Element must be a BindableObject");
        }

        return GetMaxHeight(bindable);
    }

    /// <summary>
    /// Retrieves the minimum height defined for the given bindable UI element.
    /// </summary>
    /// <param name="element">The bindable UI element for which to get the minimum height.</param>
    /// <returns>The minimum height value configured for the specified element.</returns>
    internal static double GetMinHeight(this IBottomSheet element)
    {
        if (element is not BindableObject bindable)
        {
            throw new ArgumentException("Element must be a BindableObject");
        }

        return GetMinHeight(bindable);
    }

    /// <summary>
    /// Retrieves the minimum width defined for the given bindable UI element.
    /// </summary>
    /// <param name="element">The bindable UI element for which to get the minimum width.</param>
    /// <returns>The minimum height value configured for the specified element.</returns>
    internal static double GetMinWidth(this IBottomSheet element)
    {
        if (element is not BindableObject bindable)
        {
            throw new ArgumentException("Element must be a BindableObject");
        }

        return GetMinWidth(bindable);
    }

    private static void MaxWidthPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is Element element)
        {
            element.Handler?.Invoke(nameof(SetMaxWidth));
        }
    }

    private static void MaxHeightPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is Element element)
        {
            element.Handler?.Invoke(nameof(SetMaxHeight));
        }
    }

    private static void MinWidthPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is Element element)
        {
            element.Handler?.Invoke(nameof(SetMinWidth));
        }
    }

    private static void MinHeightPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is Element element)
        {
            element.Handler?.Invoke(nameof(SetMinHeight));
        }
    }
}