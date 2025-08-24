using System.Diagnostics.CodeAnalysis;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;

namespace Plugin.BottomSheet.Tests.Shared;

public static class AppiumDriverExtensions
{
    public static AppiumElement FindElement(this AppiumDriver app, string id)
    {
        return app.FindElement(app is WindowsDriver ? MobileBy.AccessibilityId(id) : MobileBy.Id(id));
    }
    
    public static IList<AppiumElement> FindAllElements(this AppiumDriver app)
    {
        return app.FindElements(By.XPath("//*"));
    }
    
    public static bool TryFindElement(this AppiumDriver app, string id,[NotNullWhen(true)] out AppiumElement? element)
    {
        bool ret = true;
        element = null;
        
        try
        {
            element = app.FindElement(app is WindowsDriver ? MobileBy.AccessibilityId(id) : MobileBy.Id(id));
        }
        catch (NoSuchElementException)
        {
            ret = false;
        }
        catch (WebDriverException)
        {
            ret = false;
        }
        
        return ret;
    }
    
    public static bool TryFindElementByAutomationId(this AppiumDriver app, string id,[NotNullWhen(true)] out AppiumElement? element)
    {
        bool ret = true;
        element = null;
        
        try
        {
            element = app.FindElement(MobileBy.AccessibilityId(id));
        }
        catch (NoSuchElementException)
        {
            ret = false;
        }
        catch (WebDriverException)
        {
            ret = false;
        }
        
        return ret;
    }
}