#if ANDROID
using AAndroid = Microsoft.Maui.Controls.PlatformConfiguration.Android;
using AndroidView = Android.Views.View;
using Android.Graphics.Drawables;
using Android.Views;
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
            platformView.Dialog is AppCompatDialog dialog &&
            dialog.Window?.DecorView is AndroidView decorView &&
            platformView.Context is AppCompatActivity activity)
        {
            // Show the bottom sheet above the tab bar.
            ViewCompat.SetOnApplyWindowInsetsListener(decorView, new CustomWindowInsetsListener(dialog));
            decorView.SetOnTouchListener(new CustomTouchOutsideListener(activity));
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

        WindowInsetsCompat IOnApplyWindowInsetsListener.OnApplyWindowInsets(AndroidView v, WindowInsetsCompat insets)
        {
            if (_bottomSheetDialog.Context.Resources is Android.Content.Res.Resources resources)
            {
                // Note: If we are not using the Resource.Style.ThemeOverlay_MaterialComponents_BottomSheetDialog theme
                // (see SetTheme above), height must include insets.ToWindowInsets()?.SystemWindowInsetBottom.
                // This becomes:
                // int height = insets.ToWindowInsets()?.SystemWindowInsetBottom ?? 0;
                int height = 0;

                int resourceId = resources.GetIdentifier("design_bottom_navigation_height", "dimen", _bottomSheetDialog.Context.PackageName);
                if (resourceId > 0)
                {
                    height += resources.GetDimensionPixelSize(resourceId);
                }

                _bottomSheetDialog.Window?.SetBackgroundDrawable(
                    new InsetDrawable(
                        drawable: new ColorDrawable(Colors.Transparent.ToPlatform()),
                        insetLeft: 0,
                        insetTop: 0,
                        insetRight: 0,
                        insetBottom: height));
            }

            return insets;
        }
    }

    /// <summary>
    /// Custom Touch outside listener for <see cref="BottomSheet"/>.
    /// </summary>
    internal sealed class CustomTouchOutsideListener : Java.Lang.Object, AndroidView.IOnTouchListener
    {
        private readonly AppCompatActivity _activity;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomTouchOutsideListener"/> class.
        /// </summary>
        /// <param name="activity"><see cref="AppCompatActivity"/>.</param>
        public CustomTouchOutsideListener(AppCompatActivity activity)
        {
            _activity = activity;
        }

        bool AndroidView.IOnTouchListener.OnTouch(AndroidView? v, MotionEvent? e)
        {
            _activity.DispatchTouchEvent(e);
            return false;
        }
    }
#endif
}