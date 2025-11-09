using DeviceRunners.UITesting;
using DeviceRunners.VisualRunners;
using Microsoft.Extensions.Logging;
using Plugin.Maui.BottomSheet.Hosting;
#if MODE_XHARNESS
using DeviceRunners.XHarness;
#endif

namespace Plugin.BottomSheet.Tests.Maui.Unit.Application;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        Console.WriteLine("Creating the test runner application:");
        Console.WriteLine(" - Visual test runner");
#if MODE_NON_INTERACTIVE_VISUAL
		Console.WriteLine(" - Non-interactive visual test runner");
#endif
#if MODE_XHARNESS
		Console.WriteLine(" - XHarness test runner");
#endif

        var builder = MauiApp.CreateBuilder();
        builder
            .ConfigureUITesting()
#if MODE_XHARNESS
			.UseXHarnessTestRunner(conf => conf
                .AddTestAssemblies(typeof(MauiProgram).Assembly, typeof(BottomSheetTests).Assembly)
				.AddXunit())
#endif
            .UseVisualTestRunner(conf => conf
#if MODE_NON_INTERACTIVE_VISUAL
				.EnableAutoStart(true)
				.AddTcpResultChannel(new TcpResultChannelOptions
				{
					HostNames = ["localhost", "10.0.2.2"],
					Port = 16384,
					Formatter = new TextResultChannelFormatter(),
					Required = false,
					Retries = 3,
					RetryTimeout = TimeSpan.FromSeconds(5),
					Timeout = TimeSpan.FromSeconds(30)
				})
#endif
                .AddConsoleResultChannel()
                .AddTestAssemblies(typeof(MauiProgram).Assembly, typeof(BottomSheetTests).Assembly)
                .AddXunit());

#if DEBUG
        builder.Logging.AddDebug();
#else
		builder.Logging.AddConsole();
#endif

	    builder.UseBottomSheet();
        
        return builder.Build();
    }
}