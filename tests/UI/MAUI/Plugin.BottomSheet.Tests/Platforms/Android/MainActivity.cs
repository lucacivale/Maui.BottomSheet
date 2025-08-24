using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;

namespace Plugin.BottomSheet.Tests;

[Activity(Theme = "@style/Maui.SplashTheme",
    MainLauncher = true,
    LaunchMode = LaunchMode.SingleTop,
    ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
[Register("com.companyname.plugin.bottomsheet.tests.MainActivity")]
public class MainActivity : MauiAppCompatActivity
{
}