namespace Plugin.BottomSheet.Tests.Shared;

using OpenQA.Selenium.Appium;
using Xunit.Abstractions;

public abstract class BaseTest : IClassFixture<AppiumSetupFixture>
{
    protected const int DefaultDelayInMs = 500;
    
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

    protected IReadOnlyList<string> PrintAllElementIds()
    {
        var elements = App.FindAllElements();

        return elements
            .Select(x => {
                var resourceId = x.GetAttribute("resource-id");
                var contentDesc = x.GetAttribute("content-desc");
                var className = x.GetAttribute("class");
               
                // Return the first non-null identifier
                return $"{resourceId ?? string.Empty} -- {contentDesc ?? string.Empty} -- {className ?? string.Empty}";
            })
            .ToList();
    }
}