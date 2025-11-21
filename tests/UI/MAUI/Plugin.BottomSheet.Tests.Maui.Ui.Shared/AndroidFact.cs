// ReSharper disable once CheckNamespace
namespace Plugin.BottomSheet.Tests.Maui.Ui.Shared;

public sealed class AndroidFact : FactAttribute
{
    public AndroidFact()
    {
        Skip = null;
        
        if (AppiumSetup.Platform != "Android")
#pragma warning disable CS0162 // Unreachable code detected
        {
            Skip = "Test can only run on Android.";
        }
#pragma warning restore CS0162 // Unreachable code detected
    }
}