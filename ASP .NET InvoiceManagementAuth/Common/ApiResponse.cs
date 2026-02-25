namespace ASP_.NET_InvoiceManagementAuth.Common;

/// <summary>
/// A standardized wrapper for all API responses. 
/// Ensures consistency across the system for both successful operations and error handling.
/// </summary>
/// <typeparam name="T">The type of the data being returned in the response payload.</typeparam>
public class ApiResponse<T>
{
    /// <summary>
    /// Indicates whether the request was processed successfully.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// A descriptive message about the result of the operation (e.g., "User registered successfully").
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// The actual data payload requested by the client. 
    /// Will be null or default in case of an error.
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// Creates a successful response containing a data payload.
    /// </summary>
    /// <param name="data">The result data to return.</param>
    /// <param name="message">A custom success message.</param>
    /// <returns>A new <see cref="ApiResponse{T}"/> instance with Success set to true.</returns>
    public static ApiResponse<T> SuccessResponse(
        T? data,
        string message = "Operation executed successfully"
       )
        => new()
        {
            Success = true,
            Message = message,
            Data = data,
        };

    /// <summary>
    /// Creates a successful response without a data payload. 
    /// Useful for operations like Delete or Revoke where no data return is necessary.
    /// </summary>
    /// <param name="message">A custom success message.</param>
    /// <returns>A new <see cref="ApiResponse{T}"/> instance with Success set to true.</returns>
    public static ApiResponse<T> SuccessResponse(
        string message = "Operation executed successfully"
       )
        => new()
        {
            Success = true,
            Message = message
        };

    /// <summary>
    /// Creates an error response when an operation fails.
    /// </summary>
    /// <param name="message">The reason for the failure.</param>
    /// <param name="errors">Optional details about specific validation errors or internal issues.</param>
    /// <param name="executionTimeMs">Optional metric for performance tracking.</param>
    /// <returns>A new <see cref="ApiResponse{T}"/> instance with Success set to false.</returns>
    public static ApiResponse<T> ErrorResponse(
        string message,
        object? errors = null,
        long? executionTimeMs = null)
        => new()
        {
            Success = false,
            Message = message,
            Data = default,
        };
}