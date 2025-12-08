#if WINDOWS
using Plugin.Maui.BottomSheet.Platform.Windows;
using Plugin.Maui.BottomSheet.PlatformConfiguration.WindowsSpecific;
using Xunit.Abstractions;

namespace Plugin.BottomSheet.Tests.Maui.Unit.Application.Tests;

public class BottomSheetWindowsConfigurationTests : BaseTest<Mocks.EmptyContentPage, Plugin.Maui.BottomSheet.BottomSheet>
{
    public BottomSheetWindowsConfigurationTests(ITestOutputHelper testOutputHelper)
    : base(testOutputHelper)
    {
    }

    [UIFact]
    public async Task WindowsPlatformConfiguration_NoConfguation_MicrosoftDefaultsUsed()
    {
        await ((MauiBottomSheet)View.Handler!.PlatformView!).OpenAsync();

        double minWidth = ((MauiBottomSheet)View.Handler!.PlatformView!).BottomSheet!.MinWidth;
        double maxWidth = ((MauiBottomSheet)View.Handler!.PlatformView!).BottomSheet!.MaxWidth;
        double minHeight = ((MauiBottomSheet)View.Handler!.PlatformView!).BottomSheet!.MinHeight;
        double maxHeight = ((MauiBottomSheet)View.Handler!.PlatformView!).BottomSheet!.MaxHeight;

        View.ContainerView.WidthRequest = 150;
        View.ContainerView.HeightRequest = 90;

        await Task.Delay(250);

        Assert.Equal(minWidth, View.Width);
        Assert.Equal(minHeight, View.Height);

        View.ContainerView.WidthRequest = 1000;
        View.ContainerView.HeightRequest = 1400;

        await Task.Delay(250);

        Assert.InRange(maxWidth, View.Width - 1, View.Width + 1);
        Assert.InRange(maxHeight, View.Height - 1, View.Height + 1);
    }

    [UIFact]
    public async Task WindowsPlatformConfiguration_SetAndGetProperties_ReturnsExpectedValues()
    {
        double minWidth = 250;
        double maxWidth = 500;
        double minHeight = 500;
        double maxHeight = 1000;

        View.On<Microsoft.Maui.Controls.PlatformConfiguration.Windows>().SetMinWidth(minWidth);
        View.On<Microsoft.Maui.Controls.PlatformConfiguration.Windows>().SetMinHeight(minHeight);
        View.On<Microsoft.Maui.Controls.PlatformConfiguration.Windows>().SetMaxWidth(maxWidth);
        View.On<Microsoft.Maui.Controls.PlatformConfiguration.Windows>().SetMaxHeight(maxHeight);

        Assert.Equal(minWidth, View.On<Microsoft.Maui.Controls.PlatformConfiguration.Windows>().GetMinWidth());
        Assert.Equal(minHeight, View.On<Microsoft.Maui.Controls.PlatformConfiguration.Windows>().GetMinHeight());
        Assert.Equal(maxWidth, View.On<Microsoft.Maui.Controls.PlatformConfiguration.Windows>().GetMaxWidth());
        Assert.Equal(maxHeight, View.On<Microsoft.Maui.Controls.PlatformConfiguration.Windows>().GetMaxHeight());

        await ((MauiBottomSheet)View.Handler!.PlatformView!).OpenAsync();

        View.ContainerView.WidthRequest = minWidth / 2;
        View.ContainerView.HeightRequest = minHeight / 2;

        await Task.Delay(250);

        Assert.Equal(minWidth, View.Width);
        Assert.Equal(minHeight, View.Height);

        View.ContainerView.WidthRequest = maxWidth * 2;
        View.ContainerView.HeightRequest = maxHeight * 2;

        await Task.Delay(250);

        Assert.Equal(maxWidth, View.Width);
        Assert.Equal(maxHeight, View.Height);
    }

    [UIFact]
    public async Task WindowsPlatformConfiguration_ChangeConfiguration_IsApplied()
    {
        double minWidth = 250;
        double maxWidth = 500;
        double minHeight = 500;
        double maxHeight = 1000;

        await ((MauiBottomSheet)View.Handler!.PlatformView!).OpenAsync();

        View.On<Microsoft.Maui.Controls.PlatformConfiguration.Windows>().SetMinWidth(minWidth);
        View.On<Microsoft.Maui.Controls.PlatformConfiguration.Windows>().SetMinHeight(minHeight);
        View.On<Microsoft.Maui.Controls.PlatformConfiguration.Windows>().SetMaxWidth(maxWidth);
        View.On<Microsoft.Maui.Controls.PlatformConfiguration.Windows>().SetMaxHeight(maxHeight);

        Assert.Equal(minWidth, View.On<Microsoft.Maui.Controls.PlatformConfiguration.Windows>().GetMinWidth());
        Assert.Equal(minHeight, View.On<Microsoft.Maui.Controls.PlatformConfiguration.Windows>().GetMinHeight());
        Assert.Equal(maxWidth, View.On<Microsoft.Maui.Controls.PlatformConfiguration.Windows>().GetMaxWidth());
        Assert.Equal(maxHeight, View.On<Microsoft.Maui.Controls.PlatformConfiguration.Windows>().GetMaxHeight());

        View.ContainerView.WidthRequest = minWidth / 2;
        View.ContainerView.HeightRequest = minHeight / 2;

        await Task.Delay(250);

        Assert.Equal(minWidth, View.Width);
        Assert.Equal(minHeight, View.Height);

        View.ContainerView.WidthRequest = maxWidth * 2;
        View.ContainerView.HeightRequest = maxHeight * 2;

        await Task.Delay(250);

        Assert.Equal(maxWidth, View.Width);
        Assert.Equal(maxHeight, View.Height);
    }
}
#endif