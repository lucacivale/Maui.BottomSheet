// ReSharper disable once CheckNamespace
namespace Plugin.BottomSheet.Tests.Maui.Ui.Shared;

public sealed class AndroidFact : FactAttribute
{
    public AndroidFact()
    {
        Skip = null;
        
        if (AppiumSetup.Platform != "Android")
        {
            Skip = "Test can only run on Android.";
        }
    }
}