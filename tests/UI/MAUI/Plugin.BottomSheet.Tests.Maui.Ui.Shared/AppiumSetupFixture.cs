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

[CollectionDefinition("UICollection")]
// ReSharper disable once InconsistentNaming
public class UICollection : ICollectionFixture<AppiumSetupFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}