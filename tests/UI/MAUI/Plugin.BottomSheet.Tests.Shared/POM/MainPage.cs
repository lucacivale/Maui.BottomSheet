using OpenQA.Selenium;
using OpenQA.Selenium.Appium;

namespace Plugin.BottomSheet.Tests.Shared;

public class MainPage : PomBase
{
    public MainPage(AppiumDriver driver)
        : base(driver)
    {
    }
    
    private IWebElement OpenBottomSheetHeaderTestsPageElement => Wait.Until(d => d.FindElement(AutomationIds.OpenBottomSheetHeaderTestsPage));
    
    private IWebElement OpenBottomSheetTestsPageElement => Wait.Until(d => d.FindElement(AutomationIds.OpenBottomSheetTestsPage));
    
    public async Task<BottomSheetHeaderTestsPage> OpenBottomSheetHeaderTestsPage()
    {
        OpenBottomSheetHeaderTestsPageElement.Click();

        await WaitAsync();
        
        return new BottomSheetHeaderTestsPage(App);
    }
    
    public async Task<BottomSheetTestsPage> OpenBottomSheetTestsPage()
    {
        OpenBottomSheetTestsPageElement.Click();

        await WaitAsync();
        
        return new BottomSheetTestsPage(App);
    }
}