namespace ASP_.NET_InvoiceManagementAuth.Services;

/// <summary>
/// Service responsible for handling invoice attachments, including uploading, 
/// downloading, and deleting files from both storage and the database.
/// </summary>
public class AttachmentService : IAttachmentService
{
    /// <summary> Maximum allowed file size (5 MB). </summary>
    public const long MaxFileSizeBytes = 5 * 1024 * 1024;

    /// <summary> List of allowed file extensions. </summary>
    public static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".pdf", ".txt", ".zip" };

    /// <summary> List of allowed MIME content types. </summary>
    public static readonly string[] AllowedContentTypes = {
        "image/jpeg", "image/png", "application/pdf",
        "text/plain", "application/zip", "application/x-zip-compressed"
    };

    private readonly InvoiceManagmentDbContext _context;
    private readonly IFileStorage _storage;

    /// <summary>
    /// Initializes a new instance of the <see cref="AttachmentService"/> class.
    /// </summary>
    /// <param name="context">The database context for invoice management.</param>
    /// <param name="storage">The file storage service implementation.</param>
    public AttachmentService(InvoiceManagmentDbContext context, IFileStorage storage)
    {
        _context = context;
        _storage = storage;
    }

    /// <summary>
    /// Deletes an attachment from both the database and physical storage.
    /// </summary>
    /// <param name="attachmentId">The unique identifier of the attachment.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>True if deletion was successful; otherwise, false.</returns>
    public async Task<bool> DeleteAsync(Guid attachmentId, CancellationToken cancellationToken = default)
    {
        var att = await _context.Attachments.FirstOrDefaultAsync(a => a.Id == attachmentId, cancellationToken);
        if (att is null) return false;

        var key = $"invoices/{att.InvoiceId}/{att.StoredFileName}";

        _context.Attachments.Remove(att);
        await _context.SaveChangesAsync(cancellationToken);
        await _storage.DeleteAsync(key, cancellationToken);

        return true;
    }

    /// <summary>
    /// Retrieves detailed information about an attachment, including its storage key.
    /// </summary>
    /// <param name="attachmentId">The unique identifier of the attachment.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>An <see cref="InvoiceAttachmentInfo"/> object if found; otherwise, null.</returns>
    public async Task<InvoiceAttachmentInfo?> GetAttachmentInfoAsync(Guid attachmentId, CancellationToken cancellationToken = default)
    {
        var att = await _context.Attachments
            .Include(a => a.Invoice)
            .FirstOrDefaultAsync(a => a.Id == attachmentId, cancellationToken);

        if (att is null) return null;

        return new InvoiceAttachmentInfo
        {
            Id = att.Id,
            InvoiceId = att.InvoiceId,
            CustomerId = att.Invoice.CustomerId,
            StoredFileName = att.StoredFileName,
            StorageKey = $"invoices/{att.InvoiceId}/{att.StoredFileName}",
            UploadedUserId = att.UploadedUserId
        };
    }

    /// <summary>
    /// Prepares a file for download by retrieving its stream and metadata.
    /// </summary>
    /// <param name="attachmentId">The unique identifier of the attachment.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>A tuple containing the file stream, original name, and content type.</returns>
    public async Task<(Stream stream, string fileName, string contentType)?> GetDownloadAsync(Guid attachmentId, CancellationToken cancellationToken = default)
    {
        var att = await _context.Attachments.FirstOrDefaultAsync(a => a.Id == attachmentId, cancellationToken);
        if (att is null) return null;

        var key = $"invoices/{att.InvoiceId}/{att.StoredFileName}";
        var stream = await _storage.OpenReadAsync(key, cancellationToken);

        return (stream, att.OriginalFileName, att.ContentType);
    }

    /// <summary>
    /// Validates, uploads a file to storage, and records its metadata in the database.
    /// </summary>
    /// <param name="invoiceId">The ID of the invoice the file belongs to.</param>
    /// <param name="fileStream">The content of the file as a stream.</param>
    /// <param name="originalFileName">The original name of the uploaded file.</param>
    /// <param name="contentType">The MIME type of the file.</param>
    /// <param name="length">The size of the file in bytes.</param>
    /// <param name="userId">The ID of the user performing the upload.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>An <see cref="AttachmentResponseDto"/> containing the saved file details.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the file size exceeds the limit.</exception>
    /// <exception cref="ArgumentException">Thrown when the file extension or content type is invalid.</exception>
    public async Task<AttachmentResponseDto?> UploadAsync(
        Guid invoiceId,
        Stream fileStream,
        string originalFileName,
        string contentType,
        long length,
        string userId,
        CancellationToken cancellationToken = default)
    {
        if (length > MaxFileSizeBytes)
            throw new ArgumentOutOfRangeException(nameof(length), $"File size must not exceed {MaxFileSizeBytes / 1024 / 1024} MB.");

        var ext = Path.GetExtension(originalFileName).ToLowerInvariant();
        if (string.IsNullOrEmpty(ext) || !AllowedExtensions.Contains(ext))
            throw new ArgumentException($"Allowed extensions: {string.Join(", ", AllowedExtensions)}");

        if (!AllowedContentTypes.Contains(contentType, StringComparer.OrdinalIgnoreCase))
            throw new ArgumentException($"Allowed content types: {string.Join(", ", AllowedContentTypes)}");

        var invoice = await _context.Invoices.FindAsync(new object[] { invoiceId }, cancellationToken);
        if (invoice is null) return null;

        var folderKey = $"invoices/{invoiceId}";
        var info = await _storage.UploadAsync(fileStream, originalFileName, contentType, folderKey, cancellationToken);

        var attachment = new InvoiceAttachment
        {
            InvoiceId = invoiceId,
            OriginalFileName = originalFileName,
            StoredFileName = info.StoredFileName,
            ContentType = contentType,
            Size = info.Size,
            UploadedUserId = userId,
            UploadedAt = DateTimeOffset.UtcNow
        };

        _context.Attachments.Add(attachment);
        await _context.SaveChangesAsync(cancellationToken);

        return new AttachmentResponseDto
        {
            Id = attachment.Id,
            InvoiceId = attachment.InvoiceId,
            OriginalFileName = attachment.OriginalFileName,
            ContentType = attachment.ContentType,
            Size = attachment.Size,
            UploadedUserId = userId,
            UploadedAt = attachment.UploadedAt
        };
    }
}