using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Plugin.BottomSheet.Tests.Maui.Ui.Application.ViewModels;

public partial class BottomSheetTestsViewModel : ObservableObject
{
    [ObservableProperty]
    public partial bool IsOpen { get; set; }
    
    [ObservableProperty]
    public partial float CornerRadius { get; set; } = 50;
    
    [ObservableProperty]
    public partial float Margin { get; set; }
    
    [ObservableProperty]
    public partial float Padding { get; set; } = 20;
    
    [ObservableProperty]
    public partial float HalfExpandedRatio { get; set; } = 0.5f;
    
    [ObservableProperty]
    public partial bool HasHandle { get; set; } = true;
    
    [ObservableProperty]
    public partial bool IsCancelable { get; set; } = true;
    
    [ObservableProperty]
    public partial bool IsDraggable { get; set; } = true;
    
    [ObservableProperty]
    public partial bool IsModal { get; set; } = false;

    [ObservableProperty]
    public partial List<BottomSheetState>States { get; set; } = [BottomSheetState.Medium, BottomSheetState.Large];

    [ObservableProperty]
    public partial Color? WindowBackgroundColor { get; set; } = Color.FromArgb("#80000000");
    
    [ObservableProperty]
    public partial Color? BackgroundColor { get; set; }

    public BottomSheetTestsViewModel()
    {
        States = [BottomSheetState.Medium, BottomSheetState.Large];
        CurrentState = States.First();
    }

    [ObservableProperty]
    public partial BottomSheetState CurrentState { get; set; }

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