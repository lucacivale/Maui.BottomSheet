namespace Maui.BottomSheet.Samples;

public static class AppBuilderExtensions
{
	public static MauiAppBuilder PlatformServices(this MauiAppBuilder builder)
	{
		RegisterPlatformServices.RegisterServices(builder.Services);

		return builder;
	}

	public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder builder)
	{
		builder.Services.AddSingleton<MainViewModel>();

		return builder;
	}

	public static MauiAppBuilder RegisterPages(this MauiAppBuilder builder)
	{
		builder.Services.AddSingleton<MainPage>();

		return builder;
	}
}

