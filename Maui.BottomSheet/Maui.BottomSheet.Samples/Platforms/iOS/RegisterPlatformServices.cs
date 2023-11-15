using Maui.BottomSheet.Samples.Services;

namespace Maui.BottomSheet.Samples;

public static class RegisterPlatformServices
{
	public static void RegisterServices(IServiceCollection collection)
	{
		collection.AddSingleton<IDialogService, DialogService>();
	}
}

