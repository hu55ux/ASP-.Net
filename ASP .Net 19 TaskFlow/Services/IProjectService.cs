using ASP_.Net_19_TaskFlow.DTOs;
using ASP_.Net_19_TaskFlow.Models;

namespace ASP_.Net_19_TaskFlow.Services
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectResponseDto>> GetAllForUserAsync(string userId, IList<string> roles);
        Task<ProjectResponseDto?> GetByIdAsync(int id);
        Task<Project?> GetProjectEntityAsync(int id);
        Task<ProjectResponseDto> CreateAsync(CreateProjectRequest createProjectRequest, string ownerId);
        Task<ProjectResponseDto?> UpdateAsync(int id, UpdateProjectRequest updateProjectRequest);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<ProjectMemberResponseDto>> GetMembersAsync(int projectId);
        Task<IEnumerable<AvailableUserDto>> GetAvailableUsersToAddAsync(int projectId);
        Task<bool> AddMemberAsync(int projectId, string userIdOrEmail);
        Task<bool> RemoveMemberAsync(int projectId, string userId);
    }
}
