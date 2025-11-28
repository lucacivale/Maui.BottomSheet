using OpenQA.Selenium;
using OpenQA.Selenium.Appium;

// ReSharper disable once CheckNamespace
namespace Plugin.BottomSheet.Tests.Maui.Ui.Shared;

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