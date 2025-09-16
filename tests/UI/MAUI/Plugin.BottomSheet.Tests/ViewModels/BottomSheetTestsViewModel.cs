using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Plugin.BottomSheet.Tests.ViewModels;

public partial class BottomSheetTestsViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _isOpen;
    
    [ObservableProperty]
    private float _cornerRadius = 20;
    
    [ObservableProperty]
    private float _margin;
    
    [ObservableProperty]
    private float _padding = 20;
    
    [ObservableProperty]
    private float _halfExpandedRatio = 0.5f;
    
    [ObservableProperty]
    private bool _hasHandle = true;
    
    [ObservableProperty]
    private bool _isCancelable = true;
    
    [ObservableProperty]
    private bool _isDraggable = true;
    
    [ObservableProperty]
    private bool _isModal = true;

    [ObservableProperty]
    private List<BottomSheetState> _states = [BottomSheetState.Medium, BottomSheetState.Large];

    [ObservableProperty]
    private Color? _windowBackgroundColor = Color.FromArgb("#80000000");
    
    [ObservableProperty]
    private Color? _backgroundColor;

    public BottomSheetTestsViewModel()
    {
        States = [BottomSheetState.Medium, BottomSheetState.Large];
        CurrentState = States.First();
    }

    [ObservableProperty]
    private BottomSheetState _currentState;

    [RelayCommand]
    private void ChangeWindowBackgroundColor()
    {
        WindowBackgroundColor = Colors.Orange;
    }
    
    [RelayCommand]
    private void ChangeBackgroundColor()
    {
        BackgroundColor = Colors.Green;
    }
}