namespace Plugin.Maui.BottomSheet.Sample;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

public partial class CustomHeaderShowcaseViewModel : ObservableObject
{
    [ObservableProperty]
    private BottomSheetState _bottomSheetState;

    [RelayCommand]
    private void Medium()
    {
        BottomSheetState = BottomSheetState.Medium;
    }
    
    [RelayCommand]
    private void Large()
    {
        BottomSheetState = BottomSheetState.Large;
    }
}