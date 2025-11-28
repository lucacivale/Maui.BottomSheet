// ReSharper disable once CheckNamespace
namespace Plugin.BottomSheet.Tests.Maui.Ui.Shared;

using OpenQA.Selenium.Appium;
using Xunit.Abstractions;

[Collection("UICollection")]
public abstract partial class BaseTest
{
    private const int DefaultDelayInMs = 750;
    
    private readonly AppiumDriver _app;
    private readonly ITestOutputHelper _testOutputHelper;
    
    protected BaseTest(AppiumSetupFixture appiumSetupFixture, ITestOutputHelper testOutputHelper)
    {
        _app = appiumSetupFixture.App;
        _testOutputHelper = testOutputHelper;
    }

	protected AppiumDriver App => _app;

	protected ITestOutputHelper TestOutputHelper => _testOutputHelper;
    
    protected Task WaitAsync(int delayInMs = DefaultDelayInMs)
    {
        return Task.Delay(delayInMs);
    }
    
    protected async Task GoBackAsync()
    {
        await App.Navigate().BackAsync();

        await WaitAsync();
    }

    protected partial Task CloseOpenSheet();

    protected partial IReadOnlyList<string> PrintAllElementIds();
}