// ReSharper disable once CheckNamespace
namespace Plugin.BottomSheet.Tests.Maui.Ui.Shared;

using OpenQA.Selenium.Appium;
using Xunit.Abstractions;

[Collection("UICollection")]
public abstract class BaseTest
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

    protected IReadOnlyList<string> PrintAllElementIds()
    {
        IList<AppiumElement> elements = App.FindAllElements();

        return elements
            .Select(x => {
                string? resourceId = x.GetAttribute("resource-id");
                string? contentDesc = x.GetAttribute("content-desc");
                string? className = x.GetAttribute("class");
               
                // Return the first non-null identifier
                return $"{resourceId ?? string.Empty} -- {contentDesc ?? string.Empty} -- {className ?? string.Empty}";
            })
            .ToList();
    }
}