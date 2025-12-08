#if ANDROID
using AAndroid = Microsoft.Maui.Controls.PlatformConfiguration.Android;
using AndroidView = Android.Views.View;
using Android.Graphics.Drawables;
using AndroidX.AppCompat.App;
using AndroidX.Core.View;
using Microsoft.Maui.Platform;
using Plugin.Maui.BottomSheet.Platform.Android;
using Plugin.Maui.BottomSheet.PlatformConfiguration.AndroidSpecific;
#endif

using Plugin.Maui.BottomSheet.Sample.ViewModels;

namespace Plugin.Maui.BottomSheet.Sample.Views;

public partial class ShowCasePage : ContentPage
{
    public ShowCasePage(ShowCaseViewModel showCaseViewModel)
    {
        InitializeComponent();
        BindingContext = showCaseViewModel;

#if ANDROID
        NonModalBottomSheetNonEdgeToEdge.On<AAndroid>().SetTheme(Resource.Style.ThemeOverlay_MaterialComponents_BottomSheetDialog);
        NonModalBottomSheetNonEdgeToEdge.On<AAndroid>().SetHalfExpandedRatio(0.2f);
#endif
    }

    /// <summary>
    /// Toggles the Shell tab bar visibility.
    /// </summary>
    private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (sender is CheckBox checkbox)
        {
            Shell.SetTabBarIsVisible(this, checkbox.IsChecked);
        }
    }

    /// <summary>
    /// If <see cref="ShowCaseViewModel.AboveTabBar"/> is set, shows the non-modal bottom sheet
    /// with an offset such that it is above the TabBar and not in-front of it.
    /// </summary>
    private void NonModalBottomSheetNonEdgeToEdge_Opened(object sender, EventArgs e)
    {
#if ANDROID
        if (BindingContext is ShowCaseViewModel showCaseViewModel &&
            showCaseViewModel.AboveTabBar &&
            sender is BottomSheet bottomSheet &&
            bottomSheet.Handler?.PlatformView is MauiBottomSheet platformView &&
            platformView.BottomSheet is AppCompatDialog dialog &&
            dialog.Window?.DecorView is AndroidView decorView)
        {
            // Show the bottom sheet above the tab bar.
            ViewCompat.SetOnApplyWindowInsetsListener(decorView, new CustomWindowInsetsListener(dialog));
        }
#endif
    }

#if ANDROID
    /// <summary>
    /// Custom class that shows the bottom sheet vertically above the tab bar.
    /// </summary>
    internal class CustomWindowInsetsListener : Java.Lang.Object, IOnApplyWindowInsetsListener
    {
        private readonly AppCompatDialog _bottomSheetDialog;

        public CustomWindowInsetsListener(AppCompatDialog bottomSheetDialog)
        {
            _bottomSheetDialog = bottomSheetDialog;
        }

        WindowInsetsCompat? IOnApplyWindowInsetsListener.OnApplyWindowInsets(AndroidView? v, WindowInsetsCompat? insets)
        {
            if (_bottomSheetDialog.Context.Resources is Android.Content.Res.Resources resources)
            {
                int resourceId = resources.GetIdentifier("design_bottom_navigation_height", "dimen", _bottomSheetDialog.Context.PackageName);
                if (resourceId > 0)
                {
                    int height = resources.GetDimensionPixelSize(resourceId);
                    _bottomSheetDialog.Window?.SetBackgroundDrawable(
                        new InsetDrawable(
                            drawable: new ColorDrawable(Colors.Transparent.ToPlatform()),
                            insetLeft: 0,
                            insetTop: 0,
                            insetRight: 0,
                            insetBottom: height));
                }
            }

            return insets;
        }
    }
#endif
}