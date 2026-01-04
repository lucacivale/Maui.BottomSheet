#if ANDROID
using Plugin.BottomSheet.Tests.Maui.Unit.Application.Mocks;
using Plugin.Maui.BottomSheet.Platform.Android;
using Plugin.Maui.BottomSheet.PlatformConfiguration.AndroidSpecific;
using Xunit.Abstractions;

namespace Plugin.BottomSheet.Tests.Maui.Unit.Application.Tests;

public class BottomSheetAndroidConfigurationTests : BaseTest<EmptyContentPage, Plugin.Maui.BottomSheet.BottomSheet>
{
    public BottomSheetAndroidConfigurationTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    {
    }
    
    [UIFact]
    public async Task XAMLExpandedBottomSheet_ShouldNotRemoveExpandedCorners_CornerRadiusNotZero()
    {
        View = new BottomSheetShouldRemoveExpandedCorners();
        
        await LoadContent(View);
        
        MauiBottomSheet androidBottomSheet = (MauiBottomSheet)View.Handler!.PlatformView!;
        
        await androidBottomSheet.OpenAsync();
        
        Assert.False(androidBottomSheet.BottomSheet?.ShouldRemoveExpandedCorners);
        Assert.NotEqual(0, androidBottomSheet.BottomSheet?.CornerRadius);
    }
    
    [UIFact]
    public async Task BottomSheet_ShouldNotRemoveExpandedCorners_CornerRadiusNotZero()
    {
        View = new BottomSheetShouldRemoveExpandedCorners();
        await LoadContent(View);
        
        View.On<Microsoft.Maui.Controls.PlatformConfiguration.Android>().SetShouldRemoveExpandedCorners(true);
        
        MauiBottomSheet androidBottomSheet = (MauiBottomSheet)View.Handler!.PlatformView!;
        
        await androidBottomSheet.OpenAsync();

        Assert.True(androidBottomSheet.BottomSheet?.ShouldRemoveExpandedCorners);
        
        View.On<Microsoft.Maui.Controls.PlatformConfiguration.Android>().SetShouldRemoveExpandedCorners(false);
        
        Assert.False(androidBottomSheet.BottomSheet?.ShouldRemoveExpandedCorners);
        Assert.NotEqual(0, androidBottomSheet.BottomSheet?.CornerRadius);
    }
}
#endif