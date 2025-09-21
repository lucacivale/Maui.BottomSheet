using System.Drawing;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using Plugin.BottomSheet.Tests.Shared;

namespace DefaultNamespace;

public class BottomSheetTestsStaticPeek : PomBase 
{
    public BottomSheetTestsStaticPeek(AppiumDriver driver) : base(driver)
    {
    }
    
    private IWebElement DesignContentElement => Wait.Until(d => d.FindElement(BottomSheetTestsAutomationIds.DesignBottomSheet));

    public Size Size()
    {
        return DesignContentElement.Size;
    }
}