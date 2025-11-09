using Plugin.BottomSheet.Tests.Maui.Ui.Application.ViewModels;

namespace Plugin.BottomSheet.Tests.Maui.Ui.Application.Views.Pages;

public partial class BottomSheetTests
{
    public BottomSheetTests(BottomSheetTestsViewModel viewModel)
    {
        InitializeComponent();
        
        BindingContext = viewModel;
    }

    private async void OpenBottomSheet(object? sender, EventArgs e)
    {
        BottomSheet.IsOpen = true;
        
        await Task.Delay(TimeSpan.FromSeconds(1));

        RadioButtonGroup.SetSelectedValue(CurrentState, BottomSheet.CurrentState);
    }

    private void Medium_OnCheckedChanged(object? sender, CheckedChangedEventArgs e)
    {
        BottomSheetTestsViewModel vm = (BottomSheetTestsViewModel)BindingContext;

        if (Medium.IsChecked)
        {
            if (vm.States.Contains(BottomSheetState.Medium) == false)
            {
                vm.States.Add(BottomSheetState.Medium);
            }
        }
        else
        {
            vm.States.Remove(BottomSheetState.Medium);
        }
        
        vm.States = new(vm.States);
    }

    private void Large_OnCheckedChanged(object? sender, CheckedChangedEventArgs e)
    {
        BottomSheetTestsViewModel vm = (BottomSheetTestsViewModel)BindingContext;

        if (Large.IsChecked)
        {
            if (vm.States.Contains(BottomSheetState.Large) == false)
            {
                vm.States.Add(BottomSheetState.Large);
            }
        }
        else
        {
            vm.States.Remove(BottomSheetState.Large);
        }
        
        vm.States = new(vm.States);
    }

    private void SomeButton_OnClicked(object? sender, EventArgs e)
    {
        SomeButton.Text = "Clicked!";
    }

    private void OpenStaticPeekBottomSheet_OnClicked(object? sender, EventArgs e)
    {
        PeekBottomSheetStaticHeight.IsOpen = true;
    }

    private void OpenDynamicPeekBottomSheet_OnClicked(object? sender, EventArgs e)
    {
        PeekBottomSheetDynamic.IsOpen = true;
    }

    private void OpenDynamicAdvancedPeekBottomSheet_OnClicked(object? sender, EventArgs e)
    {
        PeekBottomSheetDynamicAdvanced.IsOpen = true;
    }

    private void OpenStaticPeekWithBuiltInHeaderBottomSheet_OnClicked(object? sender, EventArgs e)
    {
        PeekBottomSheetStaticHeightWithBuiltInHeader.IsOpen = true;
    }
}