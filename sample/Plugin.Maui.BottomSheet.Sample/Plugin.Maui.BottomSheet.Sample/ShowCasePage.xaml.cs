namespace Plugin.Maui.BottomSheet.Sample;

using Microsoft.Maui.Controls.PlatformConfiguration;
using PlatformConfiguration.AndroidSpecific;

public partial class ShowCasePage : ContentPage
{
    public ShowCasePage(ShowCaseViewModel viewModel)
    {
        InitializeComponent();
        
        BindingContext = viewModel;
        
        #if ANDROID
        NonModalBottomSheet.On<Android>().SetTheme(_Microsoft.Android.Resource.Designer.Resource.Style.ThemeOverlay_App_BottomSheetDialog);
        #endif
    }
}