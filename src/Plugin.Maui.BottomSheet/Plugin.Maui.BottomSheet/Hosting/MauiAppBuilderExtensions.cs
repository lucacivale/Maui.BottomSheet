namespace Plugin.Maui.BottomSheet.Hosting
{
    /// <summary>
    /// Provides extension methods for <see cref="MauiAppBuilder"/> to register and configure
    /// the BottomSheet plugin and its required services.
    /// </summary>
    public static class MauiAppBuilderExtensions
    {
        /// <summary>
        /// Registers all necessary handlers and services required by the BottomSheet plugin.
        /// Optionally applies configuration settings using the provided delegate.
        /// </summary>
        /// <param name="builder">The <see cref="MauiAppBuilder"/> instance to configure.</param>
        /// <param name="configuration">
        /// An optional delegate for configuring plugin-specific options with a <see cref="Configuration"/> instance.
        /// </param>
        /// <returns>
        /// The same <see cref="MauiAppBuilder"/> instance, enabling method chaining.
        /// </returns>
        public static MauiAppBuilder UseBottomSheet(this MauiAppBuilder builder, Action<Configuration>? configuration = null)
        {
            var config = new Configuration();
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