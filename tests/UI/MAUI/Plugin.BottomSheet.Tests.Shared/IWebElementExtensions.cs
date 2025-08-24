using OpenQA.Selenium;

namespace Plugin.BottomSheet.Tests.Shared;

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