using ASP_.Net_07_HW.Models;
namespace ASP_.Net_07_HW.Services.Interfaces;

public interface ITaskItemService
{
    Task<IEnumerable<TaskItem>> GetAllAsync();
    Task<TaskItem?> GetByIdAsync(int Id);
    Task<IEnumerable<TaskItem?>> GetByProjectIdAsync(int projectId);
    Task<TaskItem> CreateAsync(TaskItem taskItem);
    Task<TaskItem?> UpdateAsync(int id, TaskItem taskItem);
    Task<bool> DeleteAsync(int id);
}
