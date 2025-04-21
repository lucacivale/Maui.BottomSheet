#if ANDROID
using AAndroid = Microsoft.Maui.Controls.PlatformConfiguration.Android;
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
}