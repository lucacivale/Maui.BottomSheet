using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using Plugin.BottomSheet.Tests.Maui.Ui.Application.Shared;

// ReSharper disable once CheckNamespace
namespace Plugin.BottomSheet.Tests.Maui.Ui.Shared;

public class BottomSheetHeaderTestsPage : PomBase
{
    public BottomSheetHeaderTestsPage(AppiumDriver driver)
        : base(driver)
    {
    }
    
    private IWebElement OpenBottomSheetBuiltInHeaderElement => Wait.Until(d => d.FindElement(BottomSheetHeaderTestsAutomationIds.OpenBottomSheetBuiltInHeader));
    
    private IWebElement OpenBottomSheetCustomHeaderElement => Wait.Until(d => d.FindElement(BottomSheetHeaderTestsAutomationIds.OpenBottomSheetCustomHeader));
    
    public bool IsOpen()
    {
        return OpenBottomSheetBuiltInHeaderElement.Displayed;
    }
    
    public async Task<BottomSheetHeaderTestsBottomSheetBuiltInHeader> OpenBottomSheetAsync()
    {
        OpenBottomSheetBuiltInHeaderElement.Click();
        
        await WaitAsync();
        
        return new BottomSheetHeaderTestsBottomSheetBuiltInHeader(App);
    }
    
    public async Task<BottomSheetHeaderTestsBottomSheetCustomHeader> OpenBottomSheetCustomHeaderAsync()
    {
        OpenBottomSheetCustomHeaderElement.Click();
        
        await WaitAsync();
        
        return new BottomSheetHeaderTestsBottomSheetCustomHeader(App);
    }
}