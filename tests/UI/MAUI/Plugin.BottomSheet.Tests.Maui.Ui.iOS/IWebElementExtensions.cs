using OpenQA.Selenium;

// ReSharper disable once CheckNamespace
namespace Plugin.BottomSheet.Tests.Maui.Ui.Shared;

public static partial class IWebElementExtensions
{
    public static partial bool IsChecked(this IWebElement element)
    {
        return element.GetAttribute("value") == "1"
               || element.Selected; 
    }
}