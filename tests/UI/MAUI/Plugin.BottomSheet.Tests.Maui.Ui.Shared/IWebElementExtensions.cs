using OpenQA.Selenium;

// ReSharper disable once CheckNamespace
namespace Plugin.BottomSheet.Tests.Maui.Ui.Shared;

// ReSharper disable once InconsistentNaming
public static class IWebElementExtensions
{
    public static bool IsChecked(this IWebElement element)
    {
        return element.GetAttribute("checked") == "true"
            || element.GetAttribute("selected") == "true"
            || element.Selected;
    }
}