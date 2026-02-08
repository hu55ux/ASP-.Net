namespace ASP_.NET_InvoiceManagment.Common;
/// <summary>
/// Generic API Response for all classes
/// </summary>
public class ApiResponse<T>
{
    /// <summary>
    /// 
    /// </summary>
    public bool Success { get; set; } = true;
    /// <summary>
    /// 
    /// </summary>
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public long? ExecutionTimeNs { get; set; }

    public static ApiResponse<T> SuccessResponse(T? data, string message = "Operation Executed successfully!")
    {
        return new()
        {
            Success = true,
            Message = message,
            Data = data
        };
    }
}
