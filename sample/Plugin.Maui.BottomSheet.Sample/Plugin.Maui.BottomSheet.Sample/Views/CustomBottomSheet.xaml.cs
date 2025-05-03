using Plugin.Maui.BottomSheet.Sample.ViewModels;

namespace Plugin.Maui.BottomSheet.Sample.Views;

public partial class CustomBottomSheet : BottomSheet
{
	public CustomBottomSheet(CustomBottomSheetViewModel customBottomSheetViewModel)
	{
		InitializeComponent();
        BindingContext = customBottomSheetViewModel;
    }

    private void BottomSheet_Opened(object sender, EventArgs e)
    {
        NameNewusername.Focus();
    }
}