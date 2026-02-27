using ASP_.Net_07_HW.Data;
using ASP_.Net_07_HW.Models;
using ASP_.Net_07_HW.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

public class TaskItemService : ITaskItemService
{
    private readonly TaskFlowDBContext _dbContext;
    public TaskItemService(TaskFlowDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TaskItem> CreateAsync(TaskItem taskItem)
    {
        taskItem.CreatedAt = DateTime.UtcNow;
        taskItem.UpdatedAt = null;

        _dbContext.TaskItems.Add(taskItem);
        await _dbContext.SaveChangesAsync();
        return taskItem;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var task = await _dbContext.TaskItems.FindAsync(id);
        if (task is null) return false;

        _dbContext.TaskItems.Remove(task);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<TaskItem>> GetAllAsync()
    {
        return await _dbContext.TaskItems.Include(t=>t.Project).ToListAsync();
    }

    public async Task<TaskItem?> GetByIdAsync(int id)
    {
        return await _dbContext.TaskItems.FindAsync(id);
    }

    public async Task<IEnumerable<TaskItem>> GetByProjectIdAsync(int projectId)
    {
        return await _dbContext.TaskItems
            .Include(t=>t.Project)
            .Where(x => x.ProjectId == projectId)
            .ToListAsync();
    }

    public async Task<TaskItem?> UpdateAsync(int id, TaskItem taskItem)
    {
        var existingTask = await _dbContext.TaskItems.FindAsync(id);

        if (existingTask is null) return null;

        existingTask.Title = taskItem.Title;
        existingTask.Description = taskItem.Description;
        existingTask.Status = taskItem.Status;
        existingTask.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();
        return existingTask;
    }
}
