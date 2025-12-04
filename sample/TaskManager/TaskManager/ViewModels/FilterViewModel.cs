using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TaskManager.Models;
using TaskManager.Services;

namespace TaskManager.ViewModels;

using Plugin.Maui.BottomSheet.Navigation;
using TaskStatus = Models.TaskStatus;

public partial class FilterViewModel : ObservableObject, INavigationAware
{
    private readonly IBottomSheetNavigationService _bottomSheetNavigationService;
    private readonly ITaskService _taskService;

    private bool _filterChanged;

    [ObservableProperty]
    public partial string SearchText { get; set; } = string.Empty;

    [ObservableProperty]
    public partial TaskStatus? SelectedStatus { get; set; }

    [ObservableProperty]
    public partial TaskPriority? SelectedPriority { get; set; }

    [ObservableProperty]
    public partial string SelectedCategory { get; set; } = string.Empty;

    [ObservableProperty]
    public partial DateTime? DueDateFrom { get; set; }

    [ObservableProperty]
    public partial DateTime? DueDateTo { get; set; }

    [ObservableProperty]
    public partial ObservableCollection<string> Categories { get; set; } = new();

    public FilterViewModel(ITaskService taskService, IBottomSheetNavigationService bottomSheetNavigationService)
    {
        _taskService = taskService;
        _bottomSheetNavigationService = bottomSheetNavigationService;
    }

    private async Task LoadCategoriesAsync()
    {
        try
        {
            List<string> categoryList = await _taskService.GetCategoriesAsync();
            Categories.Clear();
            Categories.Add("All Categories");
            foreach (string cat in categoryList)
            {
                Categories.Add(cat);
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Error", $"Failed to load categories: {ex.Message}", "OK");
        }
    }

    [RelayCommand]
    private async Task ApplyFilterAsync()
    {
        _filterChanged = true;
        await _bottomSheetNavigationService.GoBackAsync();
    }

    [RelayCommand]
    private async Task ClearFilterAsync()
    {
        _filterChanged = true;  

        SearchText = string.Empty;
        SelectedStatus = null;
        SelectedPriority = null;
        SelectedCategory = string.Empty;
        DueDateFrom = null;
        DueDateTo = null;
        
        await _bottomSheetNavigationService.GoBackAsync();
    }

    public void OnNavigatedFrom(IBottomSheetNavigationParameters parameters)
    {
        if (_filterChanged)
        {
            TaskFilter filter = new()
            {
                SearchText = SearchText.Trim(),
                Status = SelectedStatus,
                Priority = SelectedPriority,
                Category = SelectedCategory == "All Categories" ? string.Empty : SelectedCategory.Trim(),
                DueDateFrom = DueDateFrom,
                DueDateTo = DueDateTo,
            };
        
            parameters.Add("filter", filter);
        }
    }

    public async void OnNavigatedTo(IBottomSheetNavigationParameters parameters)
    {
        await LoadCategoriesAsync();

        if (parameters.TryGetValue("Filter", out object? filter)
            && filter is TaskFilter currentFilter)
        {
            SearchText = currentFilter.SearchText;
            SelectedStatus = currentFilter.Status;
            SelectedPriority = currentFilter.Priority;
            SelectedCategory = currentFilter.Category;
            DueDateFrom = currentFilter.DueDateFrom;
            DueDateTo = currentFilter.DueDateTo;
        }
    }
}