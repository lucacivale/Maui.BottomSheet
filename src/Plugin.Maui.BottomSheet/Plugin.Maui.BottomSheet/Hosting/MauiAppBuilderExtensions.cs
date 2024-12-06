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
    /// <returns>Builder.</returns>
    public static MauiAppBuilder UseBottomSheet(this MauiAppBuilder builder)
    {
        builder.ConfigureMauiHandlers(x =>
        {
            x.AddHandler<BottomSheet, Handlers.BottomSheetHandler>();
        });

        return builder;
    }
}