using ASP_.Net_19_TaskFlow.Common;
using ASP_.Net_19_TaskFlow.DTOs;
using ASP_.Net_19_TaskFlow.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ASP_.Net_19_TaskFlow.Controllers;

/// <summary>
/// Controller for managing file attachments related to tasks.
/// Implements secure upload, download, and deletion with project-level authorization.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = "UserOrAbove")]
public class AttachmentsController : ControllerBase
{
    private readonly IAttachmentService _attachmentService;
    private readonly IProjectService _projectService;
    private readonly ITaskItemService _taskItemService;
    private readonly IAuthorizationService _authorizationService;

    /// <summary>
    /// Retrieves the unique identifier of the currently authenticated user from claims.
    /// </summary>
    private string? UserId => User.FindFirstValue(ClaimTypes.NameIdentifier);

    public AttachmentsController(
        IAttachmentService attachmentService,
        IProjectService projectService,
        ITaskItemService taskItemService,
        IAuthorizationService authorizationService)
    {
        _attachmentService = attachmentService;
        _projectService = projectService;
        _taskItemService = taskItemService;
        _authorizationService = authorizationService;
    }

    /// <summary>
    /// Uploads a file and associates it with a specific task.
    /// Verifies task existence and ensures the user has membership permissions in the associated project.
    /// </summary>
    /// <param name="taskId">The ID of the task to which the file will be attached.</param>
    /// <param name="file">The file content provided via multipart/form-data.</param>
    /// <param name="cancellationToken">Token to observe for request cancellation.</param>
    /// <returns>An <see cref="ApiResponse{T}"/> containing the uploaded file metadata.</returns>
    [HttpPost("~/api/tasks/{taskId}/attachments")]
    [ProducesResponseType(typeof(ApiResponse<AttachmentResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<AttachmentResponseDto>>> Upload(
        int taskId,
        IFormFile file,
        CancellationToken cancellationToken)
    {
        var task = await _taskItemService.GetTaskEntityAsync(taskId);
        if (task is null)
            return NotFound();

        var project = await _projectService.GetProjectEntityAsync(task.ProjectId);
        if (project is null)
            return NotFound();

        // Security Check: Ensure user belongs to the project
        var authResult = await _authorizationService.AuthorizeAsync(User, project, "ProjectMemberOrHigher");
        if (!authResult.Succeeded)
            return Forbid();

        if (file is null || file.Length == 0)
            return BadRequest("File is required");

        await using var stream = file.OpenReadStream();
        var attachment = await _attachmentService.UploadAsync(
            taskId,
            stream,
            file.FileName,
            file.ContentType,
            file.Length,
            UserId!,
            cancellationToken
            );

        if (attachment is null)
            return NotFound();

        return Ok(ApiResponse<AttachmentResponseDto>.SuccessResponse(attachment, "File uploaded successfully"));
    }

    /// <summary>
    /// Downloads a specific attachment by its ID.
    /// Performs a permission check to ensure the user is authorized to view project files.
    /// </summary>
    /// <param name="id">The unique identifier of the attachment.</param>
    /// <param name="cancellationToken">Token to observe for request cancellation.</param>
    /// <returns>A physical file stream with the correct MIME type.</returns>
    [HttpGet("{id}/download")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Download(int id, CancellationToken cancellationToken)
    {
        var info = await _attachmentService.GetAttachmentInfoAsync(id, cancellationToken);
        if (info is null)
            return NotFound();

        var project = await _projectService.GetProjectEntityAsync(info.ProjectId);

        var authResult = await _authorizationService.AuthorizeAsync(User, project, "ProjectMemberOrHigher");
        if (!authResult.Succeeded)
            return Forbid();

        var result = await _attachmentService.GetDownloadAsync(id, cancellationToken);
        if (result is null)
            return NotFound();

        return File(result.Value.stream, result.Value.contentType, result.Value.fileName);
    }

    /// <summary>
    /// Deletes an attachment and removes its physical file from storage.
    /// Requires "Project Owner" or "Admin" privileges.
    /// </summary>
    /// <param name="id">The unique identifier of the attachment to delete.</param>
    /// <param name="cancellationToken">Token to observe for request cancellation.</param>
    /// <returns>No content on success.</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var info = await _attachmentService.GetAttachmentInfoAsync(id, cancellationToken);
        if (info is null)
            return NotFound();

        var project = await _projectService.GetProjectEntityAsync(info.ProjectId);

        // Elevated Security Check: Only owners/admins can delete
        var authResult = await _authorizationService.AuthorizeAsync(User, project, "ProjectOwnerOrAdmin");
        if (!authResult.Succeeded)
            return Forbid();

        var deleted = await _attachmentService.DeleteAsync(id, cancellationToken);
        if (!deleted)
            return NotFound();

        return NoContent();
    }
}