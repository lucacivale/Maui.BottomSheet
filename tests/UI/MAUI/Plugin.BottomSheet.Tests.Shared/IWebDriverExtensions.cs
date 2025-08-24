using OpenQA.Selenium;
using OpenQA.Selenium.Appium;

namespace Plugin.BottomSheet.Tests.Shared;

// ReSharper disable once InconsistentNaming
public static class IWebDriverExtensions
{
    public static IWebElement FindElement(this IWebDriver app, string id)
    {
        return app.FindElement(MobileBy.Id(id));
    }
    
    public static IWebElement FindElementByAutomationId(this IWebDriver app, string id)
    {
        return app.FindElement(MobileBy.AccessibilityId(id));
    }
}