using OpenQA.Selenium.Appium;

namespace Plugin.BottomSheet.Tests.Shared;


public class ModalPageBottomSheetTestsPage : PomBase 
{
    public ModalPageBottomSheetTestsPage(AppiumDriver driver) : base(driver)
    {
    }
    
    public bool IsBottomSheetOpen()
    {
        return Wait.Until(d => d.FindElement(BottomSheetTestsAutomationIds.DesignBottomSheet)).Displayed;
    }
}