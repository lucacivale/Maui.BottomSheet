using Microsoft.Extensions.Logging;
using Plugin.BottomSheet.Tests.Maui.Ui.Application.ViewModels;
using Plugin.BottomSheet.Tests.Maui.Ui.Application.Views.Pages;
using Plugin.Maui.BottomSheet.Hosting;

namespace Plugin.BottomSheet.Tests.Maui.Ui.Application;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        MauiAppBuilder builder = MauiApp.CreateBuilder();
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
        
        builder.Services.AddTransient<BottomSheetTestsViewModel>();
        Routing.RegisterRoute(Routes.BottomSheetTests, typeof(BottomSheetTests));
        
        Routing.RegisterRoute(Routes.ModalPageBottomSheetTests, typeof(ModalPageBottomSheetTestsPage));
        
        return builder.Build();
    }
}