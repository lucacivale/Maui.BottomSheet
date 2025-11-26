
using OpenQA.Selenium.Appium;

// ReSharper disable once CheckNamespace
namespace Plugin.BottomSheet.Tests.Maui.Ui.Shared;

public static partial class AppiumDriverExtensions
{
    public static partial void CloseKeyboard(this AppiumDriver app)
    {
        AppiumElement? submitButton = null;
        
        if (app.TryFindElement("Return", out AppiumElement? returnButton))
        {
            submitButton = returnButton;
        }
        else if (app.TryFindElement("selected", out AppiumElement? selectedButton))
        {
            submitButton = selectedButton;
        }
        
        submitButton?.Click();
    }
}