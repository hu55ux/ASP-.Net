using ASP_.NET_16_HW.Models;

namespace ASP.NET_16_HW.Repos;

    public interface ITaskRepository
{
    Task<List<TaskItem>> GetAllAsync();
    Task<TaskItem?> GetByIdAsync(Guid id);
    Task<List<TaskItem>> GetByProjectIdAsync(Guid projectId);
    Task<List<TaskItem>> GetPagedAsync(int pageNumber, int pageSize);

    Task AddAsync(TaskItem task);
    void Update(TaskItem task);
    void Delete(TaskItem task);

    Task SaveChangesAsync();
}