using ASP_.Net_19_TaskFlow.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ASP_.Net_19_TaskFlow.Authorization;

public class ProjectOwnerOrAdminHandler
    : AuthorizationHandler<ProjectOwnerOrAdminRequirment, Project>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ProjectOwnerOrAdminRequirment requirement, Project resource)
    {
        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return Task.CompletedTask;

        if (context.User.IsInRole("Admin"))
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        if (context.User.IsInRole("Manager") && resource.OwnerId == userId)
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
