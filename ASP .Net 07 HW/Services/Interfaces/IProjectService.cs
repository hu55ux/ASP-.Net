using ASP_.Net_07_HW.Models;
namespace ASP_.Net_07_HW.Services.Interfaces;

public interface IProjectService
{
    Task<IEnumerable<Project>> GetAllAsync();
    Task<Project?> GetByIdAsync(int projectId);
    Task<Project> CreateAsync(Project project);
    Task<Project?> UpdateAsync(int id, Project project);
    Task<bool> DeleteAsync(int id);
}
