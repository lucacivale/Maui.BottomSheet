namespace Plugin.Maui.BottomSheet.Sample;

public partial class ShowCasePage : ContentPage
{
    public ShowCasePage(ShowCaseViewModel viewModel)
    {
        InitializeComponent();
        
        BindingContext = viewModel;
    }
}