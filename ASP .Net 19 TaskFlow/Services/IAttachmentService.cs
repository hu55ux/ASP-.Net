using ASP_.Net_19_TaskFlow.DTOs;

namespace ASP_.Net_19_TaskFlow.Services;

public interface IAttachmentService
{
    Task<AttachmentResponseDto?> UploadAsync(
        int taskId,
        Stream fileStream,
        string originalFileName,
        string contentType,
        long length,
        string userId,
        CancellationToken cancellationToken = default);


    Task<(Stream stream, string fileName, string contentType)?> GetDownloadAsync(
        int attachmentId, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(int attachmentId, CancellationToken cancellationToken = default);

    Task<TaskAttachmentInfo?> GetAttachmentInfoAsync(int attachmentId, CancellationToken cancellationToken = default);
}

public class TaskAttachmentInfo
{
    public int Id { get; set; }
    public int TaskItemId { get; set; }
    public int ProjectId { get; set; }
    public string StoredFileName { get; set; } = string.Empty;
    public string StorageKey { get; set; } = string.Empty;
    public string UploadedUserId { get; set; } = string.Empty;
}