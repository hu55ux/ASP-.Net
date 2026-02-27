namespace ASP_NET_19._TaskFlow_Files.Storage;

/// <summary>
/// Defines a generic contract for file storage operations.
/// This abstraction allows the application to switch between local storage, 
/// cloud providers (AWS S3, Azure Blobs), or FTP without affecting business logic.
/// </summary>
public interface IFileStorage
{
    /// <summary>
    /// Streams a file to the storage provider.
    /// </summary>
    /// <param name="stream">The readable stream of the file content.</param>
    /// <param name="originalFileName">The name of the file provided by the client.</param>
    /// <param name="contentType">The MIME type of the file.</param>
    /// <param name="folderKey">A logical grouping or directory name (e.g., "invoices" or "avatars").</param>
    /// <param name="cancellation">Token to monitor for request cancellation.</param>
    /// <returns>A <see cref="StoredFileInfo"/> object containing the generated storage key and file metadata.</returns>
    Task<StoredFileInfo> UploadAsync(
        Stream stream,
        string originalFileName,
        string contentType,
        string folderKey,
        CancellationToken cancellation = default
        );

    /// <summary>
    /// Retrieves a file from storage as a stream for downloading or processing.
    /// </summary>
    /// <param name="storageKey">The unique internal identifier/path of the file in storage.</param>
    /// <param name="cancellation">Token to monitor for request cancellation.</param>
    /// <returns>A <see cref="Stream"/> containing the file data.</returns>
    /// <exception cref="FileNotFoundException">Thrown if the storage key does not exist.</exception>
    Task<Stream> OpenReadAsync(
        string storageKey,
        CancellationToken cancellation = default
        );

    /// <summary>
    /// Permanently removes a file from the storage provider.
    /// </summary>
    /// <param name="storageKey">The unique internal identifier/path of the file to be deleted.</param>
    /// <param name="cancellation">Token to monitor for request cancellation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DeleteAsync(
        string storageKey,
        CancellationToken cancellation = default
        );
}