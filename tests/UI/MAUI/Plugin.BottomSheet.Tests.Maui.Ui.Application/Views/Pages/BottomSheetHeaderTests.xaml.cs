using Plugin.BottomSheet.Tests.Maui.Ui.Application.Shared;
using Plugin.BottomSheet.Tests.Maui.Ui.Application.ViewModels;

namespace Plugin.BottomSheet.Tests.Maui.Ui.Application.Views.Pages;

public partial class BottomSheetHeaderTests
{
    public BottomSheetHeaderTests(BottomSheetHeaderTestsViewModel viewModel)
    {
        InitializeComponent();
        
        BindingContext = viewModel;
    }

    private async void OpenBottomSheet(object? sender, EventArgs e)
    {
        BottomSheet.IsOpen = true;
        
        await Task.Delay(TimeSpan.FromSeconds(1));
    }

    private async void ChangeBindingContext(object? sender, EventArgs e)
    {
        bool isBottomSheetCustomHeaderOpen = BottomSheetCustomHeader.IsOpen;
        BindingContext = new BottomSheetHeaderTestsViewModel();
        await Task.Delay(TimeSpan.FromSeconds(1));

        if (isBottomSheetCustomHeaderOpen)
        {
            BottomSheetCustomHeader.IsOpen = true;
        }
        else
        {
            BottomSheet.IsOpen = true;
        }
    }

    private void CloseBottomSheet(object? sender, EventArgs e)
    {
        BottomSheet.IsOpen = false;
    }

    private void OpenBottomSheetCustomHeader(object? sender, EventArgs e)
    {
        BottomSheetCustomHeader.IsOpen = true;
    }

    // .NET 10 BROKE RAIDOBUTTONS THIS IS AN UGLY HACK
    
    private void HeaderLeftButtonVersionOne_OnCheckedChanged(object? sender, CheckedChangedEventArgs e)
    {
        if (((CheckBox?)((Grid?)((View?)sender)?.Parent)?.FindByName("TopLeftButtonVersionOne"))?.IsChecked == true)
        {
            ((CheckBox)((Grid)((View)sender).Parent).FindByName("TopLeftButtonVersionTwo")).IsChecked = false;
        }

        Button button = new()
        {
            AutomationId = BottomSheetHeaderTestsAutomationIds.TopLeftButtonOne
        };
        button.SetBinding(Button.TextProperty, nameof(BottomSheetHeaderTestsViewModel.TopLeftButtonOneText));
        button.SetBinding(Button.CommandProperty, nameof(BottomSheetHeaderTestsViewModel.TopLeftButtonOneCommand));
        
        ((BottomSheetHeaderTestsViewModel)BindingContext).TopLeftButton = button;
    }
    
    private void HeaderLeftButtonVersionTwo_OnCheckedChanged(object? sender, CheckedChangedEventArgs e)
    {
        if (((CheckBox?)((Grid?)((View?)sender)?.Parent)?.FindByName("TopLeftButtonVersionTwo"))?.IsChecked == true)
        {
            ((CheckBox)((Grid)((View)sender).Parent).FindByName("TopLeftButtonVersionOne")).IsChecked = false;
        }
        
        Button button = new()
        {
            AutomationId = BottomSheetHeaderTestsAutomationIds.TopLeftButtonTwo
        };
        button.SetBinding(Button.TextProperty, nameof(BottomSheetHeaderTestsViewModel.TopLeftButtonTwoText));
        button.SetBinding(Button.CommandProperty, nameof(BottomSheetHeaderTestsViewModel.TopLeftButtonTwoCommand));
        
        ((BottomSheetHeaderTestsViewModel)BindingContext).TopLeftButton = button;
    }
    
    private void HeaderRightButtonVersionOne_OnCheckedChanged(object? sender, CheckedChangedEventArgs e)
    {
        if (((CheckBox?)((Grid?)((View?)sender)?.Parent)?.FindByName("TopRightButtonVersionOne"))?.IsChecked == true)
        {
            ((CheckBox)((Grid)((View)sender).Parent).FindByName("TopRightButtonVersionTwo")).IsChecked = false;
        }
        
        Button button = new()
        {
            AutomationId = BottomSheetHeaderTestsAutomationIds.TopRightButtonOne
        };
        button.SetBinding(Button.TextProperty, nameof(BottomSheetHeaderTestsViewModel.TopRightButtonOneText));
        button.SetBinding(Button.CommandProperty, nameof(BottomSheetHeaderTestsViewModel.TopRightButtonOneCommand));
        
        ((BottomSheetHeaderTestsViewModel)BindingContext).TopRightButton = button;
    }
    
    private void HeaderRightButtonVersionTwo_OnCheckedChanged(object? sender, CheckedChangedEventArgs e)
    {
        if (((CheckBox?)((Grid?)((View?)sender)?.Parent)?.FindByName("TopRightButtonVersionTwo"))?.IsChecked == true)
        {
            ((CheckBox)((Grid)((View)sender).Parent).FindByName("TopRightButtonVersionOne")).IsChecked = false;
        }

        Button button = new()
        {
            AutomationId = BottomSheetHeaderTestsAutomationIds.TopRightButtonTwo
        };
        button.SetBinding(Button.TextProperty, nameof(BottomSheetHeaderTestsViewModel.TopRightButtonTwoText));
        button.SetBinding(Button.CommandProperty, nameof(BottomSheetHeaderTestsViewModel.TopRightButtonTwoCommand));
        
        ((BottomSheetHeaderTestsViewModel)BindingContext).TopRightButton = button;
    }

    private void BottomSheetHeaderButtonAppearanceModeNone_OnCheckedChanged(object? sender, CheckedChangedEventArgs e)
    {
        if (((CheckBox?)((Grid?)((View?)sender)?.Parent)?.FindByName("BottomSheetHeaderButtonAppearanceModeNone"))?.IsChecked == true)
        {
            ((CheckBox)((Grid)((View)sender).Parent).FindByName("BottomSheetHeaderButtonAppearanceModeLeft")).IsChecked = false;
            ((CheckBox)((Grid)((View)sender).Parent).FindByName("BottomSheetHeaderButtonAppearanceModeRight")).IsChecked = false;
            ((CheckBox)((Grid)((View)sender).Parent).FindByName("BottomSheetHeaderButtonAppearanceModeLeftAndRight")).IsChecked = false;
        }
        
        ((BottomSheetHeaderTestsViewModel)BindingContext).ButtonAppearanceMode = BottomSheetHeaderButtonAppearanceMode.None;
    }

    private void BottomSheetHeaderButtonAppearanceModeLeft_OnCheckedChanged(object? sender, CheckedChangedEventArgs e)
    {
        if (((CheckBox?)((Grid?)((View?)sender)?.Parent)?.FindByName("BottomSheetHeaderButtonAppearanceModeLeft"))?.IsChecked == true)
        {
            ((CheckBox)((Grid)((View)sender).Parent).FindByName("BottomSheetHeaderButtonAppearanceModeNone")).IsChecked = false;
            ((CheckBox)((Grid)((View)sender).Parent).FindByName("BottomSheetHeaderButtonAppearanceModeRight")).IsChecked = false;
            ((CheckBox)((Grid)((View)sender).Parent).FindByName("BottomSheetHeaderButtonAppearanceModeLeftAndRight")).IsChecked = false;
        }
        
        ((BottomSheetHeaderTestsViewModel)BindingContext).ButtonAppearanceMode = BottomSheetHeaderButtonAppearanceMode.LeftButton;
    }

    private void BottomSheetHeaderButtonAppearanceModeRight_OnCheckedChanged(object? sender, CheckedChangedEventArgs e)
    {
        if (((CheckBox?)((Grid?)((View?)sender)?.Parent)?.FindByName("BottomSheetHeaderButtonAppearanceModeRight"))?.IsChecked == true)
        {
            ((CheckBox)((Grid)((View)sender).Parent).FindByName("BottomSheetHeaderButtonAppearanceModeLeft")).IsChecked = false;
            ((CheckBox)((Grid)((View)sender).Parent).FindByName("BottomSheetHeaderButtonAppearanceModeNone")).IsChecked = false;
            ((CheckBox)((Grid)((View)sender).Parent).FindByName("BottomSheetHeaderButtonAppearanceModeLeftAndRight")).IsChecked = false;
        }
        
        ((BottomSheetHeaderTestsViewModel)BindingContext).ButtonAppearanceMode = BottomSheetHeaderButtonAppearanceMode.RightButton;
    }

    private void BottomSheetHeaderButtonAppearanceModeLeftAndRight_OnCheckedChanged(object? sender, CheckedChangedEventArgs e)
    {
        if (((CheckBox?)((Grid?)((View?)sender)?.Parent)?.FindByName("BottomSheetHeaderButtonAppearanceModeLeftAndRight"))?.IsChecked == true)
        {
            ((CheckBox)((Grid)((View)sender).Parent).FindByName("BottomSheetHeaderButtonAppearanceModeLeft")).IsChecked = false;
            ((CheckBox)((Grid)((View)sender).Parent).FindByName("BottomSheetHeaderButtonAppearanceModeRight")).IsChecked = false;
            ((CheckBox)((Grid)((View)sender).Parent).FindByName("BottomSheetHeaderButtonAppearanceModeNone")).IsChecked = false;
        }
        
        ((BottomSheetHeaderTestsViewModel)BindingContext).ButtonAppearanceMode = BottomSheetHeaderButtonAppearanceMode.LeftAndRightButton;
    }

    private void CloseButtonPositionLeft_OnCheckedChanged(object? sender, CheckedChangedEventArgs e)
    {
        if (((CheckBox?)((Grid?)((View?)sender)?.Parent)?.FindByName("CloseButtonPositionLeft"))?.IsChecked == true)
        {
            ((CheckBox)((Grid)((View)sender).Parent).FindByName("CloseButtonPositionRight")).IsChecked = false;
        }
        
        ((BottomSheetHeaderTestsViewModel)BindingContext).CloseButtonPosition = BottomSheetHeaderCloseButtonPosition.TopLeft;
    }

    private void CloseButtonPositionRight_OnCheckedChanged(object? sender, CheckedChangedEventArgs e)
    {
        if (((CheckBox?)((Grid?)((View?)sender)?.Parent)?.FindByName("CloseButtonPositionRight"))?.IsChecked == true)
        {
            ((CheckBox)((Grid)((View)sender).Parent).FindByName("CloseButtonPositionLeft")).IsChecked = false;
        }
        ((BottomSheetHeaderTestsViewModel)BindingContext).CloseButtonPosition = BottomSheetHeaderCloseButtonPosition.TopRight;
    }
}