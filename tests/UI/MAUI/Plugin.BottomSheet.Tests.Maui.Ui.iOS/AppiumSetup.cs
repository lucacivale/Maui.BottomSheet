using Plugin.BottomSheet.Tests.Maui.Ui.Shared;

// ReSharper disable once CheckNamespace
namespace Plugin.BottomSheet.Tests.Maui.Ui;

using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.iOS;

public sealed partial class AppiumSetup : IDisposable
{
    public const string Platform = "iOS";
    
	private const string ApplicationId = "Plugin.BottomSheet.Tests.Maui.Ui.Application";
 
    private readonly AppiumServiceHelper _appiumService;

    public AppiumDriver App { get; }

    public AppiumSetup()
    {
        _appiumService = new AppiumServiceHelper();
        _appiumService.StartAppiumLocalServer();

        AppiumOptions options = new AppiumOptions
        {
            AutomationName = "XCUITest",
            PlatformName = Platform,
            PlatformVersion = "26.1",
            DeviceName = "iPhone 17 Pro",
            App = Path.Combine(GetPlatformOutputFolderPath(Platform.ToLower()), "iossimulator-arm64", $"{ApplicationId}.app")
        };

        App = new IOSDriver(options);
    }

    public void Dispose()
    {
        App.Quit();
        _appiumService.Dispose();
    }
}