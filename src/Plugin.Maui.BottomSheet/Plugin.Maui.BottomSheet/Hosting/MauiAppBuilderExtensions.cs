namespace Plugin.Maui.BottomSheet.Hosting;

/// <summary>
/// <see cref="MauiAppBuilder"/> extension methods.
/// </summary>
public static class MauiAppBuilderExtensions
{
    /// <summary>
    /// Register all required services for the plugin.
    /// </summary>
    /// <param name="builder"><see cref="MauiAppBuilder"/>.</param>
    /// <param name="configuration">Plugin configuration.</param>
    /// <returns>Builder.</returns>
    public static MauiAppBuilder UseBottomSheet(this MauiAppBuilder builder, Action<Configuration>? configuration = null)
    {
        var config = new Configuration();
        configuration?.Invoke(config);

        builder
            .ConfigureMauiHandlers(x =>
            {
                x.AddHandler<BottomSheet, Handlers.BottomSheetHandler>();
#if IOS || MACCATALYST
                x.AddHandler<Platform.MaciOS.CloseButton, Handlers.CloseButtonHandler>();
#endif
            })
            .Services
                .AddSingleton<Navigation.IBottomSheetNavigationService, Navigation.BottomSheetNavigationService>()
                .AddSingleton(config);
        return builder;
    }
}