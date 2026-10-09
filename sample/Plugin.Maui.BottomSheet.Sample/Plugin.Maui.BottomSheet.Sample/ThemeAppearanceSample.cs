using Plugin.Maui.BottomSheet.Navigation;
using System.Text.Json;
using System.Runtime.InteropServices;
#if IOS || MACCATALYST
using Plugin.Maui.BottomSheet.Platform.MaciOS;
using UIKit;
#endif

namespace Plugin.Maui.BottomSheet.Sample;

internal static class ThemeAppearanceSample
{
    internal static async Task OpenAsync(ContentPage page, bool automated)
    {
        Label label = new() { Text = "Native background: no app theme colors configured", Margin = new Thickness(0, 12) };
        VerticalStackLayout content = new() { Spacing = 12, Children = { label } };
        foreach (AppTheme theme in new[] { AppTheme.Light, AppTheme.Dark, AppTheme.Unspecified })
        {
            Button button = new() { Text = theme == AppTheme.Unspecified ? "Follow system" : theme.ToString() };
            button.Clicked += (_, _) => Application.Current!.UserAppTheme = theme;
            content.Children.Add(button);
        }

        BottomSheet sheet = new()
        {
            HasHandle = true,
            Padding = new Thickness(24),
            SizeMode = Plugin.BottomSheet.BottomSheetSizeMode.FitToContent,
            Content = new BottomSheetContent { Content = content },
        };
#if ANDROID
        Button checks = new() { Text = "Run Android theme checks" };
        content.Children.Add(checks);
        checks.Clicked += async (_, _) =>
        {
            checks.IsEnabled = false;
            List<object> results = new();
            string output = Path.Combine(FileSystem.AppDataDirectory, "theme-appearance-results.json");
            try
            {
                Application app = Application.Current!;
                Plugin.Maui.BottomSheet.Platform.Android.MauiBottomSheet platform = (Plugin.Maui.BottomSheet.Platform.Android.MauiBottomSheet)sheet.Handler!.PlatformView!;
                async Task<int> ColorAsync(AppTheme theme)
                {
                    app.UserAppTheme = theme;
                    await Task.Delay(500);
                    return platform.BottomSheet!.BackgroundColor.ToArgb();
                }

                void Check(string name, bool passed)
                {
                    results.Add(new { name, passed });
                    if (!passed) throw new InvalidOperationException(name);
                }

                int light = await ColorAsync(AppTheme.Light);
                int dark = await ColorAsync(AppTheme.Dark);
                Check("app switch changes native background", light != dark);
                Check("switch back restores light", await ColorAsync(AppTheme.Light) == light);
                sheet.BackgroundColor = Colors.Red;
                int red = global::Android.Graphics.Color.Red.ToArgb();
                Check("explicit color survives dark", await ColorAsync(AppTheme.Dark) == red);
                Check("explicit color survives light", await ColorAsync(AppTheme.Light) == red);
                sheet.ClearValue(VisualElement.BackgroundColorProperty);
                Check("cleared override restores native light", await ColorAsync(AppTheme.Light) == light);
                Check("cleared override follows dark", await ColorAsync(AppTheme.Dark) == dark);
                IBottomSheetNavigationService service = page.Handler!.MauiContext!.Services.GetRequiredService<IBottomSheetNavigationService>();
                await service.GoBackAsync();
                Check("close releases subscription", typeof(Plugin.Maui.BottomSheet.Platform.Android.MauiBottomSheet).GetField("_themeApplication", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)!.GetValue(platform) is null);
                sheet = new BottomSheet
                {
                    HasHandle = true,
                    Padding = new Thickness(24),
                    SizeMode = Plugin.BottomSheet.BottomSheetSizeMode.FitToContent,
                    Content = new BottomSheetContent { Content = new Label { Text = "Opened with existing dark preference" } },
                };
                INavigationResult reopened = await service.NavigateToAsync(sheet);
                if (!reopened.Success) throw reopened.Exception ?? new InvalidOperationException("Reopen failed");
                platform = (Plugin.Maui.BottomSheet.Platform.Android.MauiBottomSheet)sheet.Handler!.PlatformView!;
                Check("new sheet applies existing dark preference", await ColorAsync(AppTheme.Dark) == dark);
                await ColorAsync(AppTheme.Unspecified);
                await File.WriteAllTextAsync(output, JsonSerializer.Serialize(new { status = "passed", light, dark, results }));
                await service.GoBackAsync();
                await OpenAsync(page, false);
            }
            catch (Exception exception)
            {
                await File.WriteAllTextAsync(output, JsonSerializer.Serialize(new { status = "failed", error = exception.ToString(), results }));
                checks.Text = "Android theme checks failed";
            }
            finally
            {
                checks.IsEnabled = true;
            }
        };
#endif
        IBottomSheetNavigationService navigation = page.Handler!.MauiContext!.Services.GetRequiredService<IBottomSheetNavigationService>();
        INavigationResult result = await navigation.NavigateToAsync(sheet);
        if (!result.Success)
        {
            throw result.Exception ?? new InvalidOperationException("Unable to open theme sample.");
        }

#if IOS || MACCATALYST
        if (automated)
        {
            List<object> results = new();
            string output = Path.Combine(FileSystem.AppDataDirectory, "theme-appearance-results.json");
            try
            {
                Application app = Application.Current!;
                MauiBottomSheet platform = (MauiBottomSheet)sheet.Handler!.PlatformView!;
                async Task CheckAsync(string name, AppTheme theme, bool custom = false, UIUserInterfaceStyle? expectedStyle = null)
                {
                    app.UserAppTheme = theme;
                    await Task.Delay(600);
                    UIKit.UIViewController native = platform.BottomSheet!;
                    UIUserInterfaceStyle expected = expectedStyle ?? (app.RequestedTheme == AppTheme.Dark ? UIUserInterfaceStyle.Dark : UIUserInterfaceStyle.Light);
                    UIUserInterfaceStyle actual = native.View!.TraitCollection.UserInterfaceStyle;
                    using UIColor resolved = native.View.BackgroundColor!.GetResolvedColor(native.View.TraitCollection);
                    using UIColor expectedColor = (custom ? UIColor.Red : UIColor.SystemBackground).GetResolvedColor(native.View.TraitCollection);
                    resolved.GetRGBA(out NFloat red, out NFloat green, out NFloat blue, out NFloat alpha);
                    expectedColor.GetRGBA(out NFloat er, out NFloat eg, out NFloat eb, out NFloat ea);
                    bool passed = actual == expected && (expectedStyle is null || app.RequestedTheme == (expected == UIUserInterfaceStyle.Dark ? AppTheme.Dark : AppTheme.Light)) && Math.Abs((double)(red - er)) < 0.01
                        && Math.Abs((double)(green - eg)) < 0.01 && Math.Abs((double)(blue - eb)) < 0.01
                        && Math.Abs((double)(alpha - ea)) < 0.01;
                    results.Add(new { name, passed, requested = app.RequestedTheme.ToString(), native = actual.ToString(), appearanceOverride = native.OverrideUserInterfaceStyle.ToString(), rgba = new[] { (double)red, (double)green, (double)blue, (double)alpha } });
                    if (!passed) throw new InvalidOperationException(name + " failed");
                }

                await CheckAsync("open sheet switches dark", AppTheme.Dark);
                await CheckAsync("open sheet switches light", AppTheme.Light);
                sheet.BackgroundColor = Colors.Red;
                await CheckAsync("custom background survives dark", AppTheme.Dark, true);
                await CheckAsync("custom background survives light", AppTheme.Light, true);
                sheet.ClearValue(VisualElement.BackgroundColorProperty);
                await CheckAsync("cleared override restores native default", AppTheme.Dark);
                await CheckAsync("follow system", AppTheme.Unspecified);
                await navigation.GoBackAsync();
                object? subscription = typeof(MauiBottomSheet).GetField("_themeApplication", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)!.GetValue(platform);
                bool cleanupPassed = subscription is null;
                results.Add(new { name = "close releases theme subscription", passed = cleanupPassed });
                if (!cleanupPassed) throw new InvalidOperationException("Theme subscription remains after closing");
                app.UserAppTheme = AppTheme.Dark;
                result = await navigation.NavigateToAsync(sheet);
                if (!result.Success) throw result.Exception ?? new InvalidOperationException("Unable to reopen theme sample");
                platform = (MauiBottomSheet)sheet.Handler!.PlatformView!;
                await CheckAsync("reopen applies existing dark preference", AppTheme.Dark);
                if (Environment.GetEnvironmentVariable("BOTTOMSHEET_THEME_TEST_SYSTEM") == "1")
                {
                    await CheckAsync("force current system appearance", app.PlatformAppTheme);
                    await CheckAsync("same-theme return to system", AppTheme.Unspecified);
                    UIUserInterfaceStyle initialSystemStyle = platform.BottomSheet!.View!.TraitCollection.UserInterfaceStyle;
                    await File.WriteAllTextAsync(output, JsonSerializer.Serialize(new { status = "waiting-for-os-switch", results }));
                    for (int attempt = 0; attempt < 100 && platform.BottomSheet!.View!.TraitCollection.UserInterfaceStyle == initialSystemStyle; attempt++)
                    {
                        await Task.Delay(200);
                    }

                    if (platform.BottomSheet!.View!.TraitCollection.UserInterfaceStyle == initialSystemStyle) throw new InvalidOperationException("OS appearance did not change during the system-follow test");
                    UIUserInterfaceStyle expectedSystemStyle = initialSystemStyle == UIUserInterfaceStyle.Light ? UIUserInterfaceStyle.Dark : UIUserInterfaceStyle.Light;
                    await CheckAsync("open sheet follows OS switch", AppTheme.Unspecified, expectedStyle: expectedSystemStyle);
                }

                await File.WriteAllTextAsync(output, JsonSerializer.Serialize(new { passed = true, results }, new JsonSerializerOptions { WriteIndented = true }));
            }
            catch (Exception exception)
            {
                await File.WriteAllTextAsync(output, JsonSerializer.Serialize(new { passed = false, error = exception.ToString(), results }, new JsonSerializerOptions { WriteIndented = true }));
            }
        }
#endif
    }
}
