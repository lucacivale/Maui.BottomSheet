using OpenQA.Selenium.Appium;
using System.Drawing;
using OpenQA.Selenium.Interactions;

// ReSharper disable once CheckNamespace
namespace Plugin.BottomSheet.Tests.Maui.Ui.Shared;

public abstract partial class BaseTest
{
    protected async partial Task CloseOpenSheet()
    {
        if (App.TryFindElementByAutomationId("Plugin.BottomSheet.iOSMacCatalyst.BottomSheet", out AppiumElement? element))
        {
            Point location = element.Location;
            Size size = element.Size;
        
            int startX = 0;
            int startY = location.Y + 50;
    
            int endX = startX;
            int endY = startY + (size.Height / 2) + 100;
    
            new Actions(App)
                .MoveToLocation(startX, startY)
                .ClickAndHold()
                .MoveToLocation(endX, endY)
                .Release()
                .Perform();
        }

        await WaitAsync();
    }
    
    protected partial IReadOnlyList<string> PrintAllElementIds()
    {
        IList<AppiumElement> elements = App.FindAllElements();

        return elements
            .Select(x =>
            {
                string? name = x.GetAttribute("name");
                string? label = x.GetAttribute("label");
                string? value = x.GetAttribute("value");
                string? type = x.GetAttribute("type");

                return $"{name ?? string.Empty} -- {label ?? string.Empty} -- {value ?? string.Empty} -- {type ?? string.Empty}";
            })
            .ToList();
    }
}