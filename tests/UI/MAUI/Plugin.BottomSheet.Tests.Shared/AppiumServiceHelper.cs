namespace Plugin.BottomSheet.Tests.Shared;

using OpenQA.Selenium.Appium.Service;

public sealed class AppiumServiceHelper : IDisposable
{
    private const string DefaultHostAddress = "127.0.0.1";
    private const int DefaultHostPort = 4723;

    private readonly AppiumLocalService _appiumLocalService;

    public AppiumServiceHelper(string host = DefaultHostAddress, int port = DefaultHostPort)
    {
        var builder = new AppiumServiceBuilder()
            .WithIPAddress(host)
            .UsingPort(port);

        _appiumLocalService = builder.Build();
    }

    public void StartAppiumLocalServer()
    {
        if (_appiumLocalService.IsRunning)
        {
            return;
        }
		
        _appiumLocalService.Start();
    }

    public void Dispose()
    {
        _appiumLocalService.Dispose();
    }
}