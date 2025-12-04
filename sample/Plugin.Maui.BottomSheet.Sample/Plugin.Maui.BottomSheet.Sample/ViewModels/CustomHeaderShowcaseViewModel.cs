using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plugin.BottomSheet;

namespace Plugin.Maui.BottomSheet.Sample.ViewModels;

public partial class CustomHeaderShowcaseViewModel : ObservableObject
{
    [ObservableProperty]
    public partial BottomSheetState BottomSheetState { get; set; }

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