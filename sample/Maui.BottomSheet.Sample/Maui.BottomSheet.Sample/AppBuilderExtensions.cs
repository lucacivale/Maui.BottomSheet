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
		builder.Services.AddTransient<MainViewModel>();
		builder.Services.AddTransient<BottomSheetVMViewModel>();
		builder.Services.AddTransient<BottomSheetGoBackViewModel>();
		builder.Services.AddTransient<NewPageAViewModel>();
		builder.Services.AddTransient<ContentAViewModel>();

		return builder;
	}

	public static MauiAppBuilder RegisterPages(this MauiAppBuilder builder)
	{
		builder.Services.AddTransient<MainPage>();
		builder.Services.AddTransient<NewPageA>();
		builder.Services.AddTransient<MainPage>();
		builder.Services.AddTransient<IBottomSheet, BottomSheetNoVM>();
		builder.Services.AddTransient<IBottomSheet, BottomSheetVM>();
		builder.Services.AddTransient<IBottomSheet, BottomSheetGoBack>();

		return builder;
	}
}

