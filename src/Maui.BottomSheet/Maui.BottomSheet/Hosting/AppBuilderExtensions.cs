using Maui.BottomSheet.SheetBuilder;
using Maui.BottomSheet.Navigation;

namespace Maui.BottomSheet;

public static class AppBuilderExtensions
{
	public static MauiAppBuilder UseMauiBottomSheet(this MauiAppBuilder builder)
	{
		builder.ConfigureMauiHandlers(h =>
		{
			h.AddHandler<BottomSheet, BottomSheetHandler>();
		});

		builder.Services.AddSingleton<IBottomSheetNavigationService, BottomSheetNavigationService>();
		builder.Services.AddSingleton<IBottomSheetBuilderFactory, BottomSheetBuilderFactory>();
		builder.Services.AddSingleton<IBottomSheetBuilder, BottomSheetBuilder>();

		return builder;
	}
}

