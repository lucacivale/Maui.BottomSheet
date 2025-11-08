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
    
    private IWebElement ContentElement => Wait.Until(d => d.FindElement(BottomSheetTestsAutomationIds.Content));
    
    private IWebElement DesignContentElement => Wait.Until(d => d.FindElement(BottomSheetTestsAutomationIds.DesignBottomSheet));
    
    private IWebElement HandleElement => Wait.Until(d => d.FindElement(AutomationIds.Handle));

    public Size HandleSize()
    {
        return HandleElement.Size;   
    }

    public Size ContentSize()
    {
        return ContentElement.Size;
    }

    public Size Size()
    {
        return DesignContentElement.Size;
    }
}