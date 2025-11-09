using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using Plugin.BottomSheet;
using TaskManager.Services;
using TaskManager.ViewModels;
using TaskManager.Views;

namespace TaskManager;

using BottomSheets;
using Plugin.Maui.BottomSheet;
using Plugin.Maui.BottomSheet.Hosting;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseBottomSheet(config =>
            {

                config.CopyPagePropertiesToBottomSheet = true;
                config.FeatureFlags.ContentFillsAvailableSpace = true;
            })
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        // Register Services
        builder.Services.AddSingleton<ITaskService, TaskService>();

        // Register ViewModels
        builder.Services.AddTransient<MainViewModel>();

        // Register Views
        builder.Services.AddTransient<MainPage>();

        builder.Services.AddBottomSheet<AddEditTaskPage, AddEditTaskViewModel>("AddEditTask",
            (sheet, _) =>
            {
                sheet.ShowHeader = true;
                sheet.Header = new BottomSheetHeader()
                {
                    HeaderAppearance = BottomSheetHeaderButtonAppearanceMode.RightButton,
                    ShowCloseButton = true,
                    CloseButtonPosition = BottomSheetHeaderCloseButtonPosition.TopRight,
                };
            });
        builder.Services.AddBottomSheet<FilterBottomSheet, FilterViewModel>("Filter");

        return builder.Build();
    }
}