namespace Plugin.Maui.BottomSheet.PlatformConfiguration.AndroidSpecific;

using Microsoft.Maui.Controls.PlatformConfiguration;

public static class BottomSheet
{
    public static readonly BindableProperty ThemeProperty =
        BindableProperty.Create(
            "Theme",
            typeof(int),
            typeof(BottomSheet),
            defaultValueCreator: (_) =>
            {
                #if ANDROID
                return _Microsoft.Android.Resource.Designer.Resource.Style.ThemeOverlay_MaterialComponents_BottomSheetDialog;
                #else
                return 0;
                #endif
            });

    public static int GetTheme(this IPlatformElementConfiguration<Android, Maui.BottomSheet.BottomSheet> config)
    {
        return GetTheme(config.Element as BindableObject);
    }

    public static int GetTheme(this IBottomSheet element)
    {
        if (element is not BindableObject bindable)
        {
            throw new ArgumentException("Element must be a BindableObject");
        }

        return GetTheme(bindable);
    }

    public static IPlatformElementConfiguration<Android, Maui.BottomSheet.BottomSheet> SetTheme(this IPlatformElementConfiguration<Android, Maui.BottomSheet.BottomSheet> config, int value)
    {
        SetTheme(config.Element as BindableObject, value);
        return config;
    }

    public static void SetTheme(this IBottomSheet element, int value)
    {
        if (element is not BindableObject bindable)
        {
            throw new ArgumentException("Element must be a BindableObject");
        }

        SetTheme(bindable, value);
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