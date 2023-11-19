using Maui.BottomSheet.Samples.BottomSheets;

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
		builder.Services.AddSingleton<BottomSheetVMViewModel>();
		builder.Services.AddSingleton<BottomSheetGoBackViewModel>();

		return builder;
	}

	public static MauiAppBuilder RegisterPages(this MauiAppBuilder builder)
	{
		builder.Services.AddSingleton<MainPage>();
		builder.Services.AddSingleton<IBottomSheet, BottomSheetNoVM>();
		builder.Services.AddSingleton<IBottomSheet, BottomSheetVM>();
		builder.Services.AddSingleton<IBottomSheet, BottomSheetGoBack>();

		return builder;
	}
}

