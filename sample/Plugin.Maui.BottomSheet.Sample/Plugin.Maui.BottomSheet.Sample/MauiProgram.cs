namespace Plugin.Maui.BottomSheet.Sample;

using CommunityToolkit.Maui;
using Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
        builder.Services.AddTransient<CustomHeaderShowcaseViewModel>();

        builder.Services.AddBottomSheet<CustomHeaderShowcase>("CustomHeaderShowcase",
            (sheet, _) =>
            {
                sheet.States = [BottomSheetState.Medium, BottomSheetState.Large];
            });
        builder.Services.AddBottomSheet<ShowCasePage>("Showcase",
            (sheet, page) =>
            {
                sheet.States = [BottomSheetState.Medium, BottomSheetState.Large];
                sheet.CurrentState = BottomSheetState.Large;
                sheet.ShowHeader = true;
                sheet.Header = new BottomSheetHeader()
                {
                    TitleText = page.Title,
                };
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}