namespace ASP_.Net_19_TaskFlow.DTOs;

/// <summary>
/// Data transfer object representing the metadata of a file attachment.
/// Used to provide file information to the client after successful operations.
/// </summary>
public class AttachmentResponseDto
{
    /// <summary>
    /// The unique identifier of the attachment in the database.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The ID of the task this attachment is associated with.
    /// </summary>
    public int TaskItemId { get; set; }

    /// <summary>
    /// The original name of the file as provided by the user during upload.
    /// </summary>
    /// <example>project_specification.pdf</example>
    public string OriginalFileName { get; set; } = string.Empty;

    /// <summary>
    /// The MIME type of the file, used by the browser to handle the file correctly.
    /// </summary>
    /// <example>application/pdf</example>
    public string ContentType { get; set; } = string.Empty;

    /// <summary>
    /// The size of the file in bytes.
    /// </summary>
    /// <example>1048576</example>
    public long Size { get; set; }

    /// <summary>
    /// The unique identifier of the user who uploaded the file.
    /// </summary>
    public string UploadedUserId { get; set; } = string.Empty;

    /// <summary>
    /// The timestamp indicating when the file was uploaded.
    /// Uses <see cref="DateTimeOffset"/> for accurate time tracking across time zones.
    /// </summary>
    public DateTimeOffset UploadedAt { get; set; }
}