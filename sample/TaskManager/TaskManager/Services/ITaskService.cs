using TaskManager.Models;

namespace TaskManager.Services;

public interface ITaskService
{
    Task<List<TaskItem>> GetTasksAsync();
    Task<TaskItem?> GetTaskByIdAsync(int id);
    Task<int> CreateTaskAsync(TaskItem task);
    Task<bool> UpdateTaskAsync(TaskItem task);
    Task<bool> DeleteTaskAsync(int id);
    Task<List<TaskItem>> FilterTasksAsync(TaskFilter filter);
    Task<List<string>> GetCategoriesAsync();
}