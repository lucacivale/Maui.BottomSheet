using Plugin.Maui.BottomSheet.Sample.ViewModels;

namespace Plugin.Maui.BottomSheet.Sample.Views;

public partial class CustomBottomSheet : BottomSheet
{
	public CustomBottomSheet(CustomBottomSheetViewModel customBottomSheetViewModel)
	{
		InitializeComponent();
        BindingContext = customBottomSheetViewModel;
    }
}