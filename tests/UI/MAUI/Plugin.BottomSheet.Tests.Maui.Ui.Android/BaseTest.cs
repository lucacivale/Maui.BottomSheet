using OpenQA.Selenium.Appium;

// ReSharper disable once CheckNamespace
namespace Plugin.BottomSheet.Tests.Maui.Ui.Shared;

public abstract partial class BaseTest
{
    protected partial Task CloseOpenSheet()
    {
        return GoBackAsync();
    }
    
    protected partial IReadOnlyList<string> PrintAllElementIds()
    {
        IList<AppiumElement> elements = App.FindAllElements();

        return elements
            .Select(x =>
            {
                string? resourceId = x.GetAttribute("resource-id");
                string? contentDesc = x.GetAttribute("content-desc");
                string? className = x.GetAttribute("class");

                return
                    $"{resourceId ?? string.Empty} -- {contentDesc ?? string.Empty} -- {className ?? string.Empty}";
            })
            .ToList();
    }
}