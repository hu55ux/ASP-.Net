namespace ASP_NET_19._TaskFlow_Files.Storage;

/// <summary>
/// Represents the result of a successful file storage operation.
/// Contains the internal identifiers and physical attributes required to retrieve or manage the file later.
/// </summary>
public class StoredFileInfo
{
    /// <summary>
    /// The unique relative path or identifier used by the storage provider to locate the file.
    /// Typically stored in the database for later retrieval.
    /// </summary>
    /// <example>tasks/2024/7f9a8b1c-3d2e.pdf</example>
    public string StorageKey { get; set; } = string.Empty;

    /// <summary>
    /// The physical name of the file as it exists on the storage medium.
    /// This is usually a generated unique identifier (GUID) to prevent collisions.
    /// </summary>
    /// <example>7f9a8b1c-3d2e.pdf</example>
    public string StoredFileName { get; set; } = string.Empty;

    /// <summary>
    /// The total size of the stored file in bytes.
    /// </summary>
    /// <example>1048576</example>
    public long Size { get; set; }
}