using OpenQA.Selenium;

namespace Plugin.BottomSheet.Tests.Android;

public static class AndroidAppiumDriverExtensions
{
    public static double ToPixels(this WebDriver app, double value)
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