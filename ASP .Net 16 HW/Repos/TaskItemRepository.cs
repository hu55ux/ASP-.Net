using ASP_.NET_16_HW.Models;

namespace ASP.NET_16_HW.Repos;

public class TaskRepository : ITaskRepository
{
    private readonly TaskFlowDbContext _context;

    public TaskRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<TaskItem>> GetAllAsync()
        => await _context.Tasks.ToListAsync();

    public async Task<TaskItem?> GetByIdAsync(Guid id)
        => await _context.Tasks.FindAsync(id);

    public async Task<List<TaskItem>> GetByProjectIdAsync(Guid projectId)
        => await _context.Tasks
            .Where(x => x.ProjectId == projectId)
            .ToListAsync();

    public async Task<List<TaskItem>> GetPagedAsync(int pageNumber, int pageSize)
        => await _context.Tasks
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

    public async Task AddAsync(TaskItem task)
        => await _context.Tasks.AddAsync(task);

    public void Update(TaskItem task)
        => _context.Tasks.Update(task);

    public void Delete(TaskItem task)
        => _context.Tasks.Remove(task);

    public async Task SaveChangesAsync()
        => await _context.SaveChangesAsync();

    Task<List<TaskItem>> ITaskRepository.GetAllAsync()
    {
        throw new NotImplementedException();
    }

    Task<TaskItem?> ITaskRepository.GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    Task<List<TaskItem>> ITaskRepository.GetByProjectIdAsync(Guid projectId)
    {
        throw new NotImplementedException();
    }

    Task<List<TaskItem>> ITaskRepository.GetPagedAsync(int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public Task AddAsync(TaskItem task)
    {
        throw new NotImplementedException();
    }

    public void Update(TaskItem task)
    {
        throw new NotImplementedException();
    }

    public void Delete(TaskItem task)
    {
        throw new NotImplementedException();
    }
}
