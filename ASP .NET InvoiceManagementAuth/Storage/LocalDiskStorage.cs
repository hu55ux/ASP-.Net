namespace ASP_NET_19._TaskFlow_Files.Storage;

/// <summary>
/// Implements <see cref="IFileStorage"/> for managing files on the local server disk.
/// Files are stored outside the public web root for enhanced security.
/// </summary>
public class LocalDiskStorage : IFileStorage
{
    private readonly string _basePath;
    private readonly ILogger<LocalDiskStorage> _logger;

    /// <summary>
    /// Initializes a new instance of <see cref="LocalDiskStorage"/>.
    /// Configures the base storage path within the application's root directory.
    /// </summary>
    /// <param name="env">The hosting environment to locate the content root.</param>
    /// <param name="logger">The logger instance for tracking storage operations.</param>
    public LocalDiskStorage(IWebHostEnvironment env, ILogger<LocalDiskStorage> logger)
    {
        // Stores files in a "Storage" folder in the project root
        _basePath = Path.Combine(env.ContentRootPath, "Storage");
        _logger = logger;
    }

    /// <summary>
    /// Saves a stream to the local disk with a unique filename to prevent overwriting.
    /// Creates the target directory if it does not exist.
    /// </summary>
    /// <param name="stream">The source file stream.</param>
    /// <param name="originalFileName">The user's filename (used to extract extension).</param>
    /// <param name="contentType">The MIME type of the file.</param>
    /// <param name="folderKey">The sub-directory within the storage root.</param>
    /// <param name="cancellation">Cancellation token.</param>
    /// <returns>Metadata about the stored file, including the relative storage key.</returns>
    public async Task<StoredFileInfo> UploadAsync(Stream stream, string originalFileName, string contentType, string folderKey, CancellationToken cancellation = default)
    {
        var ext = Path.GetExtension(originalFileName);

        if (string.IsNullOrEmpty(ext))
            ext = ".bin";

        // Generate a unique name to avoid conflicts (e.g., "7f9a... .jpg")
        var storedFileName = $"{Guid.NewGuid():N}{ext}";
        var relativePath = Path.Combine(folderKey, storedFileName);
        var fullPath = Path.Combine(_basePath, relativePath);

        // Ensure the sub-folder (folderKey) exists
        var dir = Path.GetDirectoryName(fullPath);
        Directory.CreateDirectory(dir!);

        // High-performance async write stream
        await using (var fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true))
        {
            await stream.CopyToAsync(fs, cancellation);
        }

        var size = new FileInfo(fullPath).Length;
        _logger.LogInformation("Uploaded file to {Path}, size {Size}", fullPath, size);

        return new StoredFileInfo
        {
            // Normalize slashes for database storage (cross-platform compatibility)
            StorageKey = relativePath.Replace('\\', '/'),
            StoredFileName = storedFileName,
            Size = size
        };
    }

    /// <summary>
    /// Opens a read stream for a file stored on disk.
    /// </summary>
    /// <param name="storageKey">The relative path/key of the file.</param>
    /// <param name="cancellation">Cancellation token.</param>
    /// <returns>A readable <see cref="FileStream"/>.</returns>
    /// <exception cref="FileNotFoundException">Thrown if the file is missing from the disk.</exception>
    public Task<Stream> OpenReadAsync(string storageKey, CancellationToken cancellation = default)
    {
        var fullPath = Path.Combine(_basePath, storageKey.Replace('/', Path.DirectorySeparatorChar));

        if (!File.Exists(fullPath))
            throw new FileNotFoundException("File not found in storage", storageKey);

        // Open with FileShare.Read to allow multiple concurrent reads
        var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read);

        return Task.FromResult<Stream>(stream);
    }

    /// <summary>
    /// Deletes a file from the local disk if it exists.
    /// </summary>
    /// <param name="storageKey">The relative path/key of the file to remove.</param>
    /// <param name="cancellation">Cancellation token.</param>
    public Task DeleteAsync(string storageKey, CancellationToken cancellation = default)
    {
        var fullPath = Path.Combine(_basePath, storageKey.Replace('/', Path.DirectorySeparatorChar));

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
            _logger.LogInformation("Deleted file {Path}", fullPath);
        }

        return Task.CompletedTask;
    }
}