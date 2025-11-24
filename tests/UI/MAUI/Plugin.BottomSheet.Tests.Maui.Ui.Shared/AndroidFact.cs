// ReSharper disable once CheckNamespace
namespace Plugin.BottomSheet.Tests.Maui.Ui.Shared;

public sealed partial class AndroidFact : FactAttribute
{
    public AndroidFact()
    {
        Init();
    }

    partial void Init();
}