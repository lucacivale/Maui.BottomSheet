using Microsoft.Extensions.Logging;

namespace Maui.BottomSheet.Samples;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiBottomSheet()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			})
			.RegisterPages()
			.RegisterViewModels()
			.PlatformServices();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}

