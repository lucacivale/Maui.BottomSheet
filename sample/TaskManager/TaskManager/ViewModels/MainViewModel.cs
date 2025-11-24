using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TaskManager.Models;
using TaskManager.Services;

namespace TaskManager.ViewModels;

using Plugin.Maui.BottomSheet.Navigation;
using TaskStatus = Models.TaskStatus;

public partial class MainViewModel : ObservableObject, INavigationAware
{
    private readonly IBottomSheetNavigationService _bottomSheetNavigationService;
    private readonly ITaskService _taskService;

    [ObservableProperty]
    private ObservableCollection<TaskItem> _tasks = new();

    [ObservableProperty]
    private string _searchText = string.Empty;

    [ObservableProperty]
    private TaskFilter _currentFilter = new();

    [ObservableProperty]
    private int _totalTasks;

    [ObservableProperty]
    private int _completedTasks;

    [ObservableProperty]
    private int _pendingTasks;

    public MainViewModel(ITaskService taskService, IBottomSheetNavigationService bottomSheetNavigationService)
    {
        _taskService = taskService;
        _bottomSheetNavigationService = bottomSheetNavigationService;
    }

    public async Task InitializeAsync()
    {
        await LoadTasksAsync();
    }

    [RelayCommand]
    private async Task LoadTasksAsync()
    {
        try
        {
            await ApplyFilterAsync();
            await UpdateStatistics();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Error", $"Failed to load tasks: {ex.Message}", "OK");
        }
    }

    [RelayCommand]
    private async Task SearchTasksAsync()
    {
        CurrentFilter.SearchText = SearchText;
        await ApplyFilterAsync();
    }

    [RelayCommand]
    private async Task ApplyFilterAsync()
    {
        try
        {
            Tasks = new(await _taskService.FilterTasksAsync(CurrentFilter));
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Error", $"Failed to filter tasks: {ex.Message}", "OK");
        }
    }

    [RelayCommand]
    private async Task NavigateToAddTaskAsync()
    {
        await _bottomSheetNavigationService.NavigateToAsync("AddEditTask");
    }

    [RelayCommand]
    private async Task NavigateToTaskDetailAsync(TaskItem task)
    {
        await _bottomSheetNavigationService.NavigateToAsync("AddEditTask", new BottomSheetNavigationParameters()
        {
            { "id", task.Id },
        });
    }

    [RelayCommand]
    private async Task ToggleTaskStatusAsync(TaskItem task)
    {
        try
        {
            task.Status = task.Status == TaskStatus.Completed ? TaskStatus.Todo : TaskStatus.Completed;
            await _taskService.UpdateTaskAsync(task);
            await UpdateStatistics();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Error", $"Failed to update task: {ex.Message}", "OK");
        }
    }

    [RelayCommand]
    private async Task DeleteTaskAsync(TaskItem task)
    {
        try
        {
            bool confirm = await Shell.Current.DisplayAlertAsync("Confirm Delete", 
                $"Are you sure you want to delete '{task.Title}'?", "Yes", "No");
            
            if (confirm)
            {
                await _taskService.DeleteTaskAsync(task.Id);
                Tasks.Remove(task);
                await UpdateStatistics();
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Error", $"Failed to delete task: {ex.Message}", "OK");
        }
    }

    [RelayCommand]
    private async Task ShowFilterOptionsAsync()
    {
        await _bottomSheetNavigationService.NavigateToAsync("Filter", new BottomSheetNavigationParameters()
        {
            { "Filter", CurrentFilter },
        });
    }

    private async Task UpdateStatistics()
    {
        List<TaskItem> allTasks = await _taskService.GetTasksAsync();
        TotalTasks = allTasks.Count;
        CompletedTasks = allTasks.Count(t => t.Status == TaskStatus.Completed);
        PendingTasks = allTasks.Count(t => t.Status != TaskStatus.Completed);
    }

    partial void OnSearchTextChanged(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            CurrentFilter.SearchText = string.Empty;
            _ = ApplyFilterAsync();
        }
    }

    public void OnNavigatedFrom(IBottomSheetNavigationParameters parameters)
    {
    }

    public async void OnNavigatedTo(IBottomSheetNavigationParameters parameters)
    {
        if (parameters.TryGetValue("filter", out object? filter)
            && filter is TaskFilter taskFilter)
        {
            CurrentFilter = taskFilter;
            
            await ApplyFilterAsync();
        }
        
        if (parameters.TryGetValue("Task", out object? task)
            && task is TaskItem taskItem)
        {
            if (Tasks.FirstOrDefault(x => x.Id == taskItem.Id) is TaskItem existingTask)
            {
                Tasks[Tasks.IndexOf(existingTask)] = taskItem;
            }
            else
            {
                Tasks.Add(taskItem);
            }
        }
    }
}