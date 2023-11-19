namespace Maui.BottomSheet.Navigation;

public static class IServiceProviderExtensions
{
    public static TType Resolve<TType>(this IServiceProvider serviceProvider)
    {
        return serviceProvider.GetService<TType>() ?? throw new Exception("Service not found.");
    }

    public static TService Resolve<TService, TImplementation>(this IServiceProvider serviceProvider)
    {
        var service = serviceProvider.GetServices<TService>().First(x => x?.GetType() == typeof(TImplementation));
        
        return service ?? throw new Exception("Service not found.");
    }
}