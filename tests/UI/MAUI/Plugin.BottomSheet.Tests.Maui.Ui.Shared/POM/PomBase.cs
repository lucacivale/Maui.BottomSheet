using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Support.UI;

// ReSharper disable once CheckNamespace
namespace Plugin.BottomSheet.Tests.Maui.Ui.Shared;

public abstract class PomBase
{
    public const int DefaultDelayInMs = 500;
    public const int DefaultShortDelayInMs = 100;
    
    private readonly WebDriverWait _wait;
    private readonly AppiumDriver _app;

    public PomBase(AppiumDriver driver)
    {
        _app = driver;
        _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
    }

    public WebDriverWait Wait => _wait;
    public AppiumDriver App => _app;

    public Task WaitAsync(int delayInMs = DefaultDelayInMs)
    {
        return Task.Delay(delayInMs);
    }
    
    public Task WaitShortAsync()
    {
        return WaitAsync(DefaultShortDelayInMs);
    }
    
    public async Task GoBackAsync()
    {
        await App.Navigate().BackAsync();

        await WaitAsync();
    }
}