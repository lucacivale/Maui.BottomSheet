using Microsoft.Extensions.Logging;

namespace Plugin.Maui.BottomSheet.Sample;

using CommunityToolkit.Maui;
using Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .UseBottomSheet(config => config.CopyPagePropertiesToBottomSheet = true);

        builder.Services.AddTransient<ShowCasePage>();
        builder.Services.AddTransient<ShowCaseViewModel>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}