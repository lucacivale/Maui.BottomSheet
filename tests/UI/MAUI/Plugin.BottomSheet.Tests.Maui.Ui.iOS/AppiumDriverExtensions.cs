
using OpenQA.Selenium.Appium;

// ReSharper disable once CheckNamespace
namespace Plugin.BottomSheet.Tests.Maui.Ui.Shared;

public static partial class AppiumDriverExtensions
{
    public static partial void CloseKeyboard(this AppiumDriver app)
    {
        app.FindElement("Return").Click();
    }
}