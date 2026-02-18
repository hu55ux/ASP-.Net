namespace ASP_.NET_InvoiceManagment.Common;
/// <summary>
/// Generic API Response for all classes
/// </summary>
public class ApiResponse<T>
{
    /// <summary>
    /// boolean value indicating the success or failure of the API operation.
    /// Defaults to true, indicating a successful operation unless explicitly set otherwise.
    /// </summary>
    public bool Success { get; set; } = true;

    /// <summary>
    /// string property that holds a message related to the API response, such as success confirmation or error details. 
    /// It defaults to an empty string, allowing for optional messages to be included in the response.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Data property of generic type T that can hold any type of data relevant to the API response.
    /// It is nullable, allowing for cases where there may be no data to return (e.g., in error
    /// responses or operations that do not produce a result).
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// SuccessResponse is a static method that creates and returns an instance of 
    /// ApiResponse<T> with the Success property set to true
    /// </summary>
    /// <param name="data"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public static ApiResponse<T> SuccessResponse(T? data, string message = "Operation Executed successfully!")
    {
        return new()
        {
            Success = true,
            Message = message,
            Data = data
        };
    }

    /// <summary>
    /// ErrorResponse is a static method that creates and returns an instance of 
    /// ApiResponse<T> with the Success property set to false.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="errors"></param>
    /// <param name="executionTimeMs"></param>
    /// <returns></returns>
    public static ApiResponse<T> ErrorResponse(string message, object? errors = null, long? executionTimeMs = null) => new()
    {
        Success = false,
        Message = message,
        Data = default,
    };




}
