namespace Plugin.Maui.BottomSheet.Hosting
{
    /// <summary>
    /// Contains extension methods to extend the functionality of <see cref="MauiAppBuilder"/>
    /// and facilitate the integration and configuration of the BottomSheet plugin within a .NET MAUI application.
    /// </summary>
    public static class MauiAppBuilderExtensions
    {
        /// <summary>
        /// Registers all required handlers and services for the BottomSheet plugin.
        /// Allows optional plugin configuration using the provided delegate.
        /// </summary>
        /// <param name="builder">The <see cref="MauiAppBuilder"/> instance that is being configured.</param>
        /// <param name="configuration">
        /// A delegate optionally provided to configure settings using a <see cref="Configuration"/> instance.
        /// </param>
        /// <returns>
        /// The configured <see cref="MauiAppBuilder"/> instance, supporting method chaining.
        /// </returns>
        public static MauiAppBuilder UseBottomSheet(this MauiAppBuilder builder, Action<Configuration>? configuration = null)
        {
            Configuration config = new();
            configuration?.Invoke(config);

            builder
                .ConfigureMauiHandlers(x =>
                {
                    x.AddHandler<BottomSheet, Handlers.BottomSheetHandler>();
                    x.AddHandler<CloseButton, Handlers.CloseButtonHandler>();
                })
                .Services
                .AddSingleton<Navigation.IBottomSheetNavigationService, Navigation.BottomSheetNavigationService>()
                    .AddSingleton(config);
            return builder;
        }
    }
}