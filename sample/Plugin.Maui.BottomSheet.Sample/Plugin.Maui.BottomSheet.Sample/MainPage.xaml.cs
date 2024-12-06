namespace Plugin.Maui.BottomSheet.Sample;

public partial class MainPage : ContentPage
{
    public MainPage(MainViewModel viewModel)
    {
        InitializeComponent();
        
        BindingContext = viewModel;
    }
}