// ReSharper disable once CheckNamespace
namespace Plugin.BottomSheet.Tests.Maui.Ui;

using OpenQA.Selenium.Appium.Enums;
using Shared;
using AndroidSdk;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;

public sealed partial class AppiumSetup : IDisposable
{
    public const string Platform = "Android";

    private const string AvdName = "CI_Emulator";
    private const string PackageName = "com.companyname.plugin.bottomsheet.tests.maui.ui.application";
    
    private readonly AppiumServiceHelper _appiumService;
    private readonly Emulator.AndroidEmulatorProcess _emulatorProcess;
    private readonly AndroidSdkManager _sdk;

    private readonly string _apkPath;

    public AppiumDriver App { get; }

    public AppiumSetup()
    {
        _apkPath = GetApkPath();
        _sdk = InstallSoftware();
        _emulatorProcess = _sdk.Emulator.Start(AvdName, new Emulator.EmulatorStartOptions { NoSnapshot = true });
        _emulatorProcess.WaitForBootComplete();

        UninstallApk();
        InstallApk();
        
        _appiumService = new AppiumServiceHelper();
        _appiumService.StartAppiumLocalServer();

        var options = new AppiumOptions
        {
            AutomationName = "UIAutomator2",
            PlatformName = Platform,
            PlatformVersion = "11",
        };

        options.AddAdditionalAppiumOption(MobileCapabilityType.NoReset, "true");
        options.AddAdditionalAppiumOption(AndroidMobileCapabilityType.AppPackage, PackageName);
        options.AddAdditionalAppiumOption(AndroidMobileCapabilityType.AppActivity, $"{PackageName}.MainActivity");
        
        App = new AndroidDriver(options);
    }

    public void Dispose()
    {
        UninstallApk();
        _emulatorProcess.Shutdown();
        _appiumService.Dispose();
    }

    private static AndroidSdkManager InstallSoftware()
    {
        #if CI
		const string avdSdkId = "system-images;android-30;default;x86_64";
        #else
        const string avdSdkId = "system-images;android-30;google_apis_playstore;arm64-v8a";
        #endif

        var sdkPackages = new[]
        {
            "platforms;android-30"
        };

        var sdk = new AndroidSdkManager();
        sdk.Acquire();
        sdk.SdkManager.Install(sdkPackages);
        sdk.SdkManager.Install(avdSdkId);
        if (sdk.AvdManager.ListAvds().All(x => x.Name != AvdName))
        {
            sdk.AvdManager.Create(AvdName, avdSdkId, "pixel", force: true);
        }

        return sdk;
    }
    
    private static string GetApkPath()
    {
        return Path.Combine(GetPlatformOutputFolderPath(Platform), $"{PackageName}-Signed.apk");
    }

    private void InstallApk()
    {
        _sdk.Adb.Install(new(_apkPath));
    }

    private void UninstallApk()
    {
        try
        {
            _sdk.Adb.Uninstall(new(_apkPath));   
        }
        catch (Exception)
        {
            // ignored
        }
    }
}