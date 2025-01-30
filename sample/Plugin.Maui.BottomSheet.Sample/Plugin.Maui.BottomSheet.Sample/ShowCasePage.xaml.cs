#if ANDROID
using AAndroid = Microsoft.Maui.Controls.PlatformConfiguration.Android;
using Plugin.Maui.BottomSheet.PlatformConfiguration.AndroidSpecific;
#endif

namespace Plugin.Maui.BottomSheet.Sample;

public partial class ShowCasePage : ContentPage
{
    public ShowCasePage(ShowCaseViewModel viewModel)
    {
        InitializeComponent();
        
        BindingContext = viewModel;


#if ANDROID
        NonModalBottomSheetNonEdgeToEdge.On<AAndroid>().SetTheme(Resource.Style.ThemeOverlay_MaterialComponents_BottomSheetDialog);
#endif
    }
}