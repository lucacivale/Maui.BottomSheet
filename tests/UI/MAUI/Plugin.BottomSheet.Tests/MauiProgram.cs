using Microsoft.Extensions.Logging;
using Plugin.BottomSheet.Tests.ViewModels;
using Plugin.BottomSheet.Tests.Views.Pages;
using Plugin.Maui.BottomSheet.Hosting;

namespace Plugin.BottomSheet.Tests;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .UseBottomSheet();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        builder.Services.AddTransient<BottomSheetHeaderTestsViewModel>();
        Routing.RegisterRoute(Routes.BottomSheetHeaderTests, typeof(BottomSheetHeaderTests));
        
        Console.WriteLine("BLABLABLA");

        return builder.Build();
    }
}