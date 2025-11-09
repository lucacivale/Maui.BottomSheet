// ReSharper disable once CheckNamespace
namespace Plugin.BottomSheet.Tests.Maui.Ui.Shared;

using OpenQA.Selenium.Appium;

public sealed class AppiumSetupFixture : IDisposable
{
    private readonly AppiumSetup _appiumSetup = new();

    public AppiumDriver App => _appiumSetup.App;

    public void Dispose()
    {
        App.Quit();
        App.Dispose();
        
        _appiumSetup.Dispose();
    }
}