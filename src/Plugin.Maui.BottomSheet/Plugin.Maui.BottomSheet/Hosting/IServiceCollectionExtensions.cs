namespace Plugin.Maui.BottomSheet.Hosting;

using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Provides extension methods for <see cref="IServiceCollection"/> to register <see cref="BottomSheet"/> instances.
/// These methods allow registering a <see cref="ContentPage"/> as a <see cref="BottomSheet"/>,
/// optionally associating a view model and providing custom configuration
/// for use within a .NET MAUI application using dependency injection.
/// </summary>
// ReSharper disable once InconsistentNaming
public static class IServiceCollectionExtensions
{
    /// <summary>
    /// Registers a <typeparamref name="T"/> ContentPage as a <see cref="BottomSheet"/> by the specified name.
    /// </summary>
    /// <typeparam name="T">The ContentPage type to register as <see cref="BottomSheet"/>. Must implement <see cref="IView"/>.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> for dependency registration.</param>
    /// <param name="name">The unique name to associate with the <see cref="BottomSheet"/>.</param>
    /// <returns>The updated <see cref="IServiceCollection"/> with the bottom sheet registered.</returns>
    public static IServiceCollection AddBottomSheet<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(this IServiceCollection services, string name)
        where T : class, IView
    {
        services.TryAddTransient<T>();
        services.AddKeyedTransient<IBottomSheet>(
            name,
            (serviceProvider, _) => BottomSheetFactory<T>(serviceProvider).BottomSheet);
        return services;
    }

    /// <summary>
    /// Registers a <typeparamref name="T"/> ContentPage as a <see cref="BottomSheet"/>, with an associated view model <typeparamref name="TViewModel"/>, by the specified name.
    /// </summary>
    /// <typeparam name="T">The ContentPage type to register as <see cref="BottomSheet"/>. Must implement <see cref="IView"/>.</typeparam>
    /// <typeparam name="TViewModel">The view model type to associate with the bottom sheet.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> for dependency registration.</param>
    /// <param name="name">The unique name to associate with the <see cref="BottomSheet"/>.</param>
    /// <returns>The updated <see cref="IServiceCollection"/> with the bottom sheet and view model registered.</returns>
    public static IServiceCollection AddBottomSheet<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TViewModel>(this IServiceCollection services, string name)
        where T : class, IView
        where TViewModel : class
    {
        services.TryAddTransient<T>();
        services.TryAddTransient<TViewModel>();
        Navigation.BottomSheetNavigationService.BottomSheetToViewModelMapping.Add(name, typeof(TViewModel));
        services.AddKeyedTransient<IBottomSheet>(
            name,
            (serviceProvider, _) => BottomSheetFactory<T>(serviceProvider).BottomSheet);
        return services;
    }

    /// <summary>
    /// Registers a <typeparamref name="T"/> ContentPage as a <see cref="BottomSheet"/> by the specified name, applying custom configuration.
    /// </summary>
    /// <typeparam name="T">The ContentPage type to register as <see cref="BottomSheet"/>. Must implement <see cref="IView"/>.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> for dependency registration.</param>
    /// <param name="name">The unique name to associate with the <see cref="BottomSheet"/>.</param>
    /// <param name="configure">
    /// The action used to configure the created <see cref="BottomSheet"/> and its underlying element of type <typeparamref name="T"/>.
    /// </param>
    /// <returns>The updated <see cref="IServiceCollection"/> with the bottom sheet registered and configured.</returns>
    public static IServiceCollection AddBottomSheet<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(this IServiceCollection services, string name, Action<IBottomSheet, T> configure)
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

    /// <summary>
    /// Registers a <typeparamref name="T"/> ContentPage as a <see cref="BottomSheet"/> with an associated view model <typeparamref name="TViewModel"/>, 
    /// by the specified name, and applies custom configuration.
    /// </summary>
    /// <typeparam name="T">The ContentPage type to register as <see cref="BottomSheet"/>. Must implement <see cref="IView"/>.</typeparam>
    /// <typeparam name="TViewModel">The view model type to associate with the bottom sheet.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> for dependency registration.</param>
    /// <param name="name">The unique name to associate with the <see cref="BottomSheet"/>.</param>
    /// <param name="configure">
    /// The action used to configure the created <see cref="BottomSheet"/> and its underlying element of type <typeparamref name="T"/>.
    /// </param>
    /// <returns>The updated <see cref="IServiceCollection"/> with the bottom sheet and view model registered and configured.</returns>
    public static IServiceCollection AddBottomSheet<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TViewModel>(this IServiceCollection services, string name, Action<IBottomSheet, T> configure)
        where T : class, IView
        where TViewModel : class
    {
        services.TryAddTransient<T>();
        services.TryAddTransient<TViewModel>();
        Navigation.BottomSheetNavigationService.BottomSheetToViewModelMapping.Add(name, typeof(TViewModel));
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

    /// <summary>
    /// Creates the <see cref="BottomSheet"/> and its associated element of type <typeparamref name="T"/>.
    /// Assigns the content and optionally copies relevant page properties if the element is a <see cref="ContentPage"/>.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="IView"/> to use for the bottom sheet's content.</typeparam>
    /// <param name="serviceProvider">The service provider for dependency resolution.</param>
    /// <returns>
    /// A tuple containing the instantiated <see cref="BottomSheet"/> and its associated <typeparamref name="T"/> element.
    /// </returns>
    private static (BottomSheet BottomSheet, T Element) BottomSheetFactory<T>(IServiceProvider serviceProvider)
        where T : class, IView
    {
        if (typeof(IBottomSheet).IsAssignableFrom(typeof(T)))
        {
            var sheet = serviceProvider.GetRequiredService<T>();
            return ((sheet as BottomSheet)!, sheet);
        }

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