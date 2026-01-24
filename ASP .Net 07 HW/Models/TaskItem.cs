namespace ASP_.Net_07_HW.Models;

public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TaskStatus Status { get; set; } = TaskStatus.Todo;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public int ProjectId { get; set; }
    public Project Project { get; set; } = null!;

}

public enum TaskStatus
{
    Todo,
    InProgress,
    Done
}