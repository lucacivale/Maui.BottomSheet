// ReSharper disable once CheckNamespace

using OpenQA.Selenium;

namespace Plugin.BottomSheet.Tests.Maui.Ui.Shared;

public static class WebDriverExtensions
{
    public static double ToAndroidPixels(this WebDriver app, double value)
    {
        double px = -1;

        var deviceInfoObject = app.ExecuteScript("mobile: deviceInfo");
        
        if (deviceInfoObject is Dictionary<string, object> deviceInfo
            && deviceInfo.TryGetValue("displayDensity", out var density))
        {
            px = value * Convert.ToDouble(density) / 160;
        }

        return px;
    }
}