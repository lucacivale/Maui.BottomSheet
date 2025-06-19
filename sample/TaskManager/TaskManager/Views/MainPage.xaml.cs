using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Views;

using ViewModels;

public partial class MainPage : ContentPage
{
    public MainPage(MainViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
    
    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        await ((MainViewModel)BindingContext).InitializeAsync();
    }

    private async void SelectableItemsView_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        await ((MainViewModel)BindingContext).NavigateToTaskDetailCommand.ExecuteAsync(e.CurrentSelection[0]);
    }
}
