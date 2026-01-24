using ASP_.Net_07_HW.Data;
using ASP_.Net_07_HW.Models;
using ASP_.Net_07_HW.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ASP_.Net_07_HW.Services;

public class ProjectService : IProjectService
{
    private readonly TaskFlowDBContext _dBContext;
    public async Task<Project> CreateAsync(Project project)
    {
        project.CreatedAt = DateTime.UtcNow;
        project.UpdatedAt = null!;
        _dBContext.Projects.Add(project);
        await _dBContext.SaveChangesAsync();

        await _dBContext.Entry(project).Collection(p => p.Tasks).LoadAsync();
        return project;
    }
    public ProjectService(TaskFlowDBContext dBContext)
    {
        _dBContext = dBContext;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var deletedProject = await _dBContext.Projects.FindAsync(id);
        if (deletedProject is null) return false;
        _dBContext.Projects.Remove(deletedProject);
        await _dBContext.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Project>> GetAllAsync()
    {
        return await _dBContext
                                .Projects
                                .Include(p => p.Tasks)
                                .ToListAsync();
    }

    public async Task<Project?> GetByIdAsync(int projectId)
    {
        return await _dBContext
                                .Projects
                                .Include(p => p.Tasks)
                                .FirstOrDefaultAsync(p => p.Id == projectId);
    }

    public async Task<Project?> UpdateAsync(int id, Project project)
    {
        var updatedProject = await _dBContext.Projects.Include(p => p.Tasks).FirstOrDefaultAsync(p => p.Id == id);
        if (updatedProject is null) return null;
        updatedProject.Name = project.Name;
        updatedProject.Description = project.Description;
        project.UpdatedAt = DateTime.UtcNow;
        await _dBContext.SaveChangesAsync();
        return project;
    }
}
