namespace TaskManager.Models;

public class TaskFilter
{
    public string SearchText { get; set; } = string.Empty;
    public TaskStatus? Status { get; set; }
    public TaskPriority? Priority { get; set; }
    public string Category { get; set; } = string.Empty;
    public DateTime? DueDateFrom { get; set; }
    public DateTime? DueDateTo { get; set; }
}