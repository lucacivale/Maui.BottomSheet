namespace Maui.BottomSheet;

public static class AppBuilderExtensions
{
	public static MauiAppBuilder UseMauiBottomSheet(this MauiAppBuilder builder)
	{
		builder.ConfigureMauiHandlers(h =>
		{
			h.AddHandler<BottomSheet, BottomSheetHandler>();
		});

		return builder;
	}
}

