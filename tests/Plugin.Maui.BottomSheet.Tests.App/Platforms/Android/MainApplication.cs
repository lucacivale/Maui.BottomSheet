using Android.App;
using Android.Runtime;

namespace Plugin.Maui.BottomSheet.Tests.App;

using MauiApp1;

[Application]
public class MainApplication : MauiApplication
{
    public MainApplication(IntPtr handle, JniHandleOwnership ownership)
        : base(handle, ownership)
    {
    }

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}