using Plugin.Maui.BottomSheet.Hosting;
using Microsoft.Extensions.Logging;
using Plugin.Maui.BottomSheet.Sample.ViewModels;
using Plugin.Maui.BottomSheet.Sample.Views;

namespace Plugin.Maui.BottomSheet.Sample
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .UseBottomSheet(config => config.CopyPagePropertiesToBottomSheet = true)
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddTransient<ShowCasePage>();
            builder.Services.AddTransient<ShowCaseViewModel>();
            builder.Services.AddTransient<ShellPage>();
            builder.Services.AddTransient<ShellPageViewModel>();
            builder.Services.AddTransient<CustomHeaderShowcaseViewModel>();

            builder.Services.AddBottomSheet<SomeBottomSheet, SomeViewModel>("SomeBottomSheet");
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

            builder.Services.AddBottomSheet<CustomBottomSheet, CustomBottomSheetViewModel>("CustomBottomSheet");


#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}