using ASP_.NET_16_HW.Models;
using TaskStatus = ASP_.NET_16_HW.Models.TaskStatus;

namespace ASP_.NET_16_HW.DTOs.Task_Items_DTOs;

public class TaskStatusUpdateRequest
{
    public TaskStatus Status { get; set; } = TaskStatus.ToDo;
}
