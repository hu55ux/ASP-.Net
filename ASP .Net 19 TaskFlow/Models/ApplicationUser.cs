using Microsoft.AspNetCore.Identity;

namespace ASP_.Net_19_TaskFlow.Models;

public class ApplicationUser: IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTimeOffset? UpdatedAt { get; set; } = null!;

    public IEnumerable<ProjectMember> ProjectMemberships { get; set; }
        = new List<ProjectMember>();
}
