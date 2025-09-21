using System.Drawing;
using DefaultNamespace;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;

namespace Plugin.BottomSheet.Tests.Shared;

public class BottomSheetTestsPage : PomBase
{
    public BottomSheetTestsPage(AppiumDriver driver) : base(driver)
    {
    }
    
    private IWebElement OpenBottomSheetElement => Wait.Until(d => d.FindElement(BottomSheetTestsAutomationIds.OpenBottomSheet));
    
    private IWebElement SomeButtonElement => Wait.Until(d => d.FindElement(BottomSheetTestsAutomationIds.SomeButton));
    
    private IWebElement OpenStaticPeekBottomSheetElement => Wait.Until(d => d.FindElement(BottomSheetTestsAutomationIds.OpenStaticPeekBottomSheet));
    
    public bool IsOpen()
    {
        return OpenBottomSheetElement.Displayed;
    }
    
    public async Task<BottomSheetTestsBottomSheet> OpenBottomSheetAsync()
    {
        OpenBottomSheetElement.Click();
        
        await WaitAsync();
        
        return new BottomSheetTestsBottomSheet(App);
    }
    
    public async Task<BottomSheetTestsStaticPeek> OpenBottomSheetStaticPeekAsync()
    {
        OpenStaticPeekBottomSheetElement.Click();
        
        await WaitAsync();
        
        return new BottomSheetTestsStaticPeek(App);
    }
    
    public Point SomeButtonLocation()
    {
        return SomeButtonElement.Location;
    }
    
    public string SomeButtonText()
    {
        return SomeButtonElement.Text;
    }
}