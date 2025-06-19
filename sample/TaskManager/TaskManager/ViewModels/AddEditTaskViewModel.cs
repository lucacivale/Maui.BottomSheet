
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TaskManager.Models;
using TaskManager.Services;

namespace TaskManager.ViewModels;

using Plugin.Maui.BottomSheet.Navigation;
using TaskStatus = Models.TaskStatus;

public partial class AddEditTaskViewModel : ObservableObject, INavigationAware, IConfirmNavigationAsync
{
    private readonly ITaskService _taskService;
    private readonly IBottomSheetNavigationService _bottomSheetNavigationService;

    private bool _isLoading;
    private bool _somethingChanged;

    [ObservableProperty]
    private int _taskId;

    [ObservableProperty]
    private string _title = string.Empty;

    [ObservableProperty]
    private string _description = string.Empty;

    [ObservableProperty]
    private DateTime? _dueDate;

    [ObservableProperty]
    private TaskPriority _priority = TaskPriority.Medium;

    [ObservableProperty]
    private TaskStatus _status = TaskStatus.Todo;

    [ObservableProperty]
    private string _category = string.Empty;

    [ObservableProperty]
    private ObservableCollection<string> _categories = new();

    [ObservableProperty]
    private bool _isEditMode;
    
    public AddEditTaskViewModel(ITaskService taskService, IBottomSheetNavigationService bottomSheetNavigationService)
    {
        _taskService = taskService;
        _bottomSheetNavigationService = bottomSheetNavigationService;
    }

    private async Task LoadCategoriesAsync()
    {
        try
        {
            var categoryList = await _taskService.GetCategoriesAsync();
            Categories.Clear();
            foreach (var cat in categoryList)
            {
                Categories.Add(cat);
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Error", $"Failed to load categories: {ex.Message}", "OK");
        }
    }

    private async Task LoadTaskAsync()
    {
        try
        {
            _isLoading = true;
            var task = await _taskService.GetTaskByIdAsync(TaskId);
            if (task != null)
            {
                Title = task.Title;
                Description = task.Description;
                DueDate = task.DueDate;
                Priority = task.Priority;
                Status = task.Status;
                Category = task.Category;

                await Task.Delay(100);
            }
            _isLoading = false;
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Error", $"Failed to load task: {ex.Message}", "OK");
        }
    }

    [RelayCommand]
    private async Task SaveTaskAsync()
    {
        if (string.IsNullOrWhiteSpace(Title))
        {
            await Shell.Current.DisplayAlertAsync("Validation Error", "Title is required", "OK");
            return;
        }

        try
        {
            var task = new TaskItem
            {
                Id = TaskId,
                Title = Title.Trim(),
                Description = Description.Trim(),
                DueDate = DueDate ?? DateTime.Now,
                Priority = Priority,
                Status = Status,
                Category = Category.Trim()
            };

            if (IsEditMode)
            {
                await _taskService.UpdateTaskAsync(task);
                await Shell.Current.DisplayAlertAsync("Success", "Task updated successfully", "OK");
            }
            else
            {
                await _taskService.CreateTaskAsync(task);
                await Shell.Current.DisplayAlertAsync("Success", "Task created successfully", "OK");
            }
            
            await _bottomSheetNavigationService.GoBackAsync(new BottomSheetNavigationParameters()
            {
                { "Task", task },
            });
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Error", $"Failed to save task: {ex.Message}", "OK");
        }
    }

    public void OnNavigatedFrom(IBottomSheetNavigationParameters parameters)
    {
    }

    public async void OnNavigatedTo(IBottomSheetNavigationParameters parameters)
    {
        await LoadCategoriesAsync();
        
        if (parameters.TryGetValue("id", out var id)
            && id is int taskId)
        {
            TaskId = taskId;
            IsEditMode = true;
            
            await LoadTaskAsync();
        }
        else
        {
            IsEditMode = false;
        }
    }
    
    // ReSharper disable once UnusedParameterInPartialMethod
    partial void OnTitleChanged(string value)
    {
        if (_isLoading)
        {
            return;
        }
        _somethingChanged = true;
    }
    
    // ReSharper disable once UnusedParameterInPartialMethod
    partial void OnDescriptionChanged(string value)
    {
        if (_isLoading)
        {
            return;
        }
        _somethingChanged = true;
    }
    
    // ReSharper disable once UnusedParameterInPartialMethod
    partial void OnDueDateChanged(DateTime? value)
    {
        if (_isLoading)
        {
            return;
        }
        _somethingChanged = true;
    }
    
    // ReSharper disable once UnusedParameterInPartialMethod
    partial void OnPriorityChanged(TaskPriority value)
    {
        if (_isLoading)
        {
            return;
        }
        _somethingChanged = true;
    }
    
    // ReSharper disable once UnusedParameterInPartialMethod
    partial void OnStatusChanged(TaskStatus value)
    {
        if (_isLoading
            || value.Equals(Status))
        {
            return;
        }
        _somethingChanged = true;
    }
    
    // ReSharper disable once UnusedParameterInPartialMethod
    partial void OnCategoryChanged(string value)
    {
        if (_isLoading
            || value.Equals(Category))
        {
            return;
        }
        _somethingChanged = true;
    }

    public async Task<bool> CanNavigateAsync(IBottomSheetNavigationParameters parameters)
    {
        bool canNavigate = true;
        if (_somethingChanged)
        {
            var confirm = await Shell.Current.DisplayAlertAsync(
                "Confirm",
                "You have unsaved changes. Do you want to discard them and exit without saving?",
                "Yes",
                "No");

            canNavigate = confirm;
        }

        return canNavigate;
    }
}