namespace TaskManager.Models;

public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public TaskPriority Priority { get; set; }
    public TaskStatus Status { get; set; }
    public string Category { get; set; } = string.Empty;
    public bool IsCompleted => Status == TaskStatus.Completed;
    public string PriorityColor => Priority switch
    {
        TaskPriority.High => "#FF5252",
        TaskPriority.Medium => "#FF9800",
        TaskPriority.Low => "#4CAF50",
        _ => "#9E9E9E"
    };
    public string StatusText => Status.ToString();
}

public enum TaskPriority
{
    Low,
    Medium,
    High
}

public enum TaskStatus
{
    Todo,
    InProgress,
    Completed,
    Cancelled
}