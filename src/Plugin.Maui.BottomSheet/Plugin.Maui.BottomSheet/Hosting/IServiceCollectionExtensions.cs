namespace Plugin.Maui.BottomSheet.Hosting;

using Microsoft.Extensions.DependencyInjection.Extensions;

/// <summary>
/// <see cref="IServiceCollection"/> extension methods.
/// </summary>
// ReSharper disable once InconsistentNaming
public static class IServiceCollectionExtensions
{
    /// <summary>
    /// Registers ContentPage as <see cref="BottomSheet"/>.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/>.</param>
    /// <param name="name"><see cref="BottomSheet"/> name.</param>
    /// <typeparam name="T">ContentPage which will be registered as <see cref="BottomSheet"/>.</typeparam>
    /// <returns>Updated <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddBottomSheet<T>(this IServiceCollection services, string name)
        where T : class, IView
    {
        services.TryAddTransient<T>();

        services.AddKeyedTransient<IBottomSheet>(
            name,
            (serviceProvider, _) => BottomSheetFactory<T>(serviceProvider).BottomSheet);

        return services;
    }

    /// <summary>
    /// Registers ContentPage as <see cref="BottomSheet"/>.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/>.</param>
    /// <param name="name"><see cref="BottomSheet"/> name.</param>
    /// <param name="configure">Action to configure <see cref="BottomSheet"/>.</param>
    /// <typeparam name="T">ContentPage which will be registered as <see cref="BottomSheet"/>.</typeparam>
    /// <returns>Updated <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddBottomSheet<T>(this IServiceCollection services, string name, Action<IBottomSheet, T> configure)
        where T : class, IView
    {
        services.TryAddTransient<T>();

        services.AddKeyedTransient<IBottomSheet>(
            name,
            (serviceProvider, _) =>
            {
                var (bottomSheet, element) = BottomSheetFactory<T>(serviceProvider);

                configure.Invoke(bottomSheet, element);

                return bottomSheet;
            });

        return services;
    }

    private static (BottomSheet BottomSheet, T Element) BottomSheetFactory<T>(IServiceProvider serviceProvider)
        where T : class, IView
    {
        var element = serviceProvider.GetRequiredService<T>();
        IView view = element;
        object? bindingContext = null;
        var bottomSheet = new BottomSheet();

        if (view is ContentPage page)
        {
            view = page.Content;
            bindingContext = page.BindingContext;

            if (serviceProvider.GetRequiredService<Configuration>().CopyPagePropertiesToBottomSheet)
            {
                bottomSheet.BackgroundColor = page.BackgroundColor;
                bottomSheet.Padding = page.Padding;
            }
        }

        bottomSheet.Content = new BottomSheetContent
        {
            ContentTemplate = new DataTemplate(() => view),
            BindingContext = bindingContext,
        };

        return (bottomSheet, element);
    }
}