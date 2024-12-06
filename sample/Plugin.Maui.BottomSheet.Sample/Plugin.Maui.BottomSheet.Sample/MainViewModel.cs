namespace Plugin.Maui.BottomSheet.Sample;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _isBottomSheetHeaderSampleOpen;
    
    [ObservableProperty]
    private List<BottomSheetState> _states = [];

    [ObservableProperty]
    private BottomSheetState _currentState;

    [RelayCommand]
    private void PeekClicked()
    {
        CurrentState = BottomSheetState.Peek;
    }
    [RelayCommand]
    private void MediumClicked()
    {
        CurrentState = BottomSheetState.Medium;
    }
    
    [RelayCommand]
    private void LargeClicked()
    {
        CurrentState = BottomSheetState.Large;
    }
    
    [RelayCommand]
    private void OpenBottomSheetHeaderSample()
    {
        IsBottomSheetHeaderSampleOpen = true;
    }
    
    [RelayCommand]
    private void PeekState()
    {
        States = [BottomSheetState.Peek];
    }
    
    [RelayCommand]
    private void MediumState()
    {
        States = [BottomSheetState.Medium];
    }
    
    [RelayCommand]
    private void LargeState()
    {
        States = [BottomSheetState.Large];
    }
    
    [RelayCommand]
    private void PeekAndMediumState()
    {
        States = [BottomSheetState.Peek, BottomSheetState.Medium];
    }
    
    [RelayCommand]
    private void PeekAndLargeState()
    {
        States = [BottomSheetState.Peek, BottomSheetState.Large];
    }
    
    [RelayCommand]
    private void PeekAndMediumAndLargeState()
    {
        States = [BottomSheetState.Peek, BottomSheetState.Medium, BottomSheetState.Large];
    }
    
    [RelayCommand]
    private void MediumAndLargeState()
    {
        States = [BottomSheetState.Medium, BottomSheetState.Large];
    }
}