using TaskManager.Models;

namespace TaskManager.Services;

using TaskStatus = Models.TaskStatus;

public class TaskService : ITaskService
{
    private List<TaskItem> _tasks;
    private int _nextId;

    public TaskService()
    {
        _tasks = GenerateDummyData();
        _nextId = _tasks.Count + 1;
    }

    public Task<List<TaskItem>> GetTasksAsync()
    {
        return Task.FromResult(_tasks.OrderByDescending(t => t.CreatedDate).ToList());
    }

    public Task<TaskItem?> GetTaskByIdAsync(int id)
    {
        var task = _tasks.FirstOrDefault(t => t.Id == id);
        return Task.FromResult(task);
    }

    public Task<int> CreateTaskAsync(TaskItem task)
    {
        task.Id = _nextId++;
        task.CreatedDate = DateTime.Now;
        _tasks.Add(task);
        return Task.FromResult(task.Id);
    }

    public Task<bool> UpdateTaskAsync(TaskItem task)
    {
        var index = _tasks.FindIndex(t => t.Id == task.Id);
        if (index >= 0)
        {
            _tasks[index] = task;
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }

    public Task<bool> DeleteTaskAsync(int id)
    {
        var task = _tasks.FirstOrDefault(t => t.Id == id);
        if (task != null)
        {
            _tasks.Remove(task);
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }

    public Task<List<TaskItem>> FilterTasksAsync(TaskFilter filter)
    {
        var query = _tasks.AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.SearchText))
        {
            query = query.Where(t => t.Title.Contains(filter.SearchText, StringComparison.OrdinalIgnoreCase) ||
                                   t.Description.Contains(filter.SearchText, StringComparison.OrdinalIgnoreCase));
        }

        if (filter.Status.HasValue)
        {
            query = query.Where(t => t.Status == filter.Status.Value);
        }

        if (filter.Priority.HasValue)
        {
            query = query.Where(t => t.Priority == filter.Priority.Value);
        }

        if (!string.IsNullOrWhiteSpace(filter.Category))
        {
            query = query.Where(t => t.Category.Equals(filter.Category, StringComparison.OrdinalIgnoreCase));
        }

        if (filter.DueDateFrom.HasValue)
        {
            query = query.Where(t => t.DueDate >= filter.DueDateFrom.Value);
        }

        if (filter.DueDateTo.HasValue)
        {
            query = query.Where(t => t.DueDate <= filter.DueDateTo.Value);
        }

        return Task.FromResult(query.OrderByDescending(t => t.CreatedDate).ToList());
    }

    public Task<List<string>> GetCategoriesAsync()
    {
        var categories = _tasks.Select(t => t.Category)
                              .Where(c => !string.IsNullOrWhiteSpace(c))
                              .Distinct()
                              .OrderBy(c => c)
                              .ToList();
        return Task.FromResult(categories);
    }

    private List<TaskItem> GenerateDummyData()
    {
        var random = new Random();
        var categories = new[] { "Work", "Personal", "Shopping", "Health", "Education", "Travel" };
        var titles = new[]
        {
            "Complete project proposal", "Buy groceries", "Schedule dentist appointment",
            "Prepare presentation", "Exercise routine", "Learn new programming language",
            "Plan vacation", "Clean house", "Pay bills", "Call mom",
            "Update resume", "Read book", "Fix car", "Organize files",
            "Team meeting", "Code review", "Database backup", "Client call"
        };

        var descriptions = new[]
        {
            "Important task that needs attention",
            "Don't forget to complete this",
            "High priority item",
            "Regular maintenance task",
            "Follow up required",
            "Deadline approaching",
            "Review and update as needed",
            "Coordinate with team members"
        };

        var tasks = new List<TaskItem>();

        for (int i = 1; i <= 25; i++)
        {
            var dueDate = DateTime.Now.AddDays(random.Next(-30, 60));
            var createdDate = DateTime.Now.AddDays(random.Next(-45, 0));

            tasks.Add(new TaskItem
            {
                Id = i,
                Title = titles[random.Next(titles.Length)],
                Description = descriptions[random.Next(descriptions.Length)],
                DueDate = dueDate,
                CreatedDate = createdDate,
                Priority = (TaskPriority)random.Next(0, 3),
                Status = (TaskStatus)random.Next(0, 4),
                Category = categories[random.Next(categories.Length)]
            });
        }

        return tasks;
    }
}