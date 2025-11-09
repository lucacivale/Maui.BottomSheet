using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using Plugin.BottomSheet.Tests.Maui.Ui.Application.Shared;

// ReSharper disable once CheckNamespace
namespace Plugin.BottomSheet.Tests.Maui.Ui.Shared;

public class BottomSheetHeaderTestsBottomSheetCustomHeader : PomBase
{
    public BottomSheetHeaderTestsBottomSheetCustomHeader(AppiumDriver driver)
        : base(driver)
    {
    }
    
    private IWebElement ShowHeaderElement => Wait.Until(d => d.FindElement(BottomSheetHeaderTestsAutomationIds.ShowHeader));
    
    private IWebElement ChangeBindingContextElement => Wait.Until(d => d.FindElement(BottomSheetHeaderTestsAutomationIds.ChangeBindingContext));
    
    private IWebElement ButtonElement => Wait.Until(d => d.FindElement(BottomSheetHeaderTestsAutomationIds.CustomHeaderButton));
    
    public bool IsOpen()
    {
        return App.TryFindElementByAutomationId(BottomSheetHeaderTestsAutomationIds.BottomSheetCustomHeader, out AppiumElement? element)
            && element.Displayed;
    }
    
    public async Task ShowHeaderAsync()
    {
        if (ShowHeaderElement.IsChecked() == false)
        {
            ShowHeaderElement.Click();
            await WaitShortAsync();
        }
    }
    
    public async Task HideHeaderAsync()
    {
        if (ShowHeaderElement.IsChecked())
        {
            ShowHeaderElement.Click();
            await WaitShortAsync();
        }
    }

    public bool HeaderDisplayed()
    {
        return App.TryFindElement(BottomSheetHeaderTestsAutomationIds.CustomHeader, out AppiumElement? element)
            && element.Displayed;
    }
    
    public async Task ChangeBindingContextAsync()
    {
        ChangeBindingContextElement.Click();
        
        await WaitAsync();
    }
    
    public void ClickButton()
    {
        ButtonElement.Click();
    }
    
    public bool ButtonClicked()
    {
        return ButtonElement.Text == "Clicked";
    }
}