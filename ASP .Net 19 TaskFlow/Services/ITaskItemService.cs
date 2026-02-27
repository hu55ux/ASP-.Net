using ASP_.Net_19_TaskFlow.Common;
using ASP_.Net_19_TaskFlow.DTOs;
using ASP_.Net_19_TaskFlow.Models;

namespace ASP_.Net_19_TaskFlow.Services;

public interface ITaskItemService
{
    Task<IEnumerable<TaskItemResponseDto>> GetAllAsync();
    Task<PagedResult<TaskItemResponseDto>> GetPagedAsync(TaskItemQueryParams queryParams);
    Task<TaskItemResponseDto?> GetByIdAsync(int id);
    Task<TaskItem?> GetTaskEntityAsync(int id);
    Task<IEnumerable<TaskItemResponseDto>> GetByProjectIdAsync(int projectId);
    Task<TaskItemResponseDto> CreateAsync(CreateTaskItemRequest createTaskItemRequest);
    Task<TaskItemResponseDto?> UpdateAsync(int id, UpdateTaskItemRequest updateTaskItemRequest);
    Task<TaskItemResponseDto?> UpdateStatusAsync(int id, TaskStatusUpdateRequest request);
    Task<bool> DeleteAsync(int id);
}
