// ReSharper disable once CheckNamespace
namespace Plugin.BottomSheet.Tests.Maui.Ui.Shared;

public sealed partial class AndroidFact
{
    partial void Init()
    {
        Skip = "Test is not supported on iOS";
    }
}