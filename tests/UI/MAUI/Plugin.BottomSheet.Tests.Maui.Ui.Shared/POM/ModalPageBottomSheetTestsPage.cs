using OpenQA.Selenium.Appium;
using Plugin.BottomSheet.Tests.Maui.Ui.Application.Shared;

// ReSharper disable once CheckNamespace
namespace Plugin.BottomSheet.Tests.Maui.Ui.Shared;


public class ModalPageBottomSheetTestsPage : PomBase 
{
    public ModalPageBottomSheetTestsPage(AppiumDriver driver) : base(driver)
    {
    }
    
    public bool IsBottomSheetOpen()
    {
        return Wait.Until(d => d.FindElement(ModalPageBottomSheetTestsAutomationIds.BottomSheet)).Displayed;
    }
    
    public void Close()
    {
        Wait.Until(d => d.FindElement(ModalPageBottomSheetTestsAutomationIds.CloseModalPage)).Click();
    }
}