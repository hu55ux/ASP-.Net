using System.Text.Json;
using ASP_.NET_InvoiceManagment.Validators.CustomerValidators;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace ASP_.NET_InvoiceManagment.Middleware;

/// <summary>
/// Class for handling global exceptions in the application. This middleware will catch 
/// any unhandled exceptions that occur during the processing of HTTP requests and provide 
/// a consistent response to the client, as well as logging the exception details for further analysis.
/// </summary>
public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    /// <summary>
    /// Constructor for the GlobalExceptionMiddleware class. It takes a RequestDelegate and an ILogger as parameters.
    /// </summary>
    /// <param name="next"></param>
    /// <param name="logger"></param>
    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Invokes the middleware to handle exceptions. It wraps the next
    /// middleware in a try-catch block to catch any unhandled exceptions 
    /// that occur during the processing of the request. If an exception is caught,
    /// it calls the HandleExceptionAsync method to log the error and return a standardized error response to the client.
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    /// <summary>
    /// Handles the exception by logging the error and returning a standardized error response to the client.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="ex"></param>
    /// <returns></returns>
    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        _logger.LogError(ex, "An unhandled exception occurred while processing the request.");
        context.Response.ContentType = "application/json";

        ProblemDetails problem;
        int statusCode;
        switch (ex)
        {
            case ValidationException validationException:
                statusCode = StatusCodes.Status400BadRequest;
                problem = CreateValidationProblemDetails(context, validationException, statusCode);
                break;
            case KeyNotFoundException:
                statusCode = StatusCodes.Status404NotFound;
                problem = CreateProblemDetails(context, statusCode, "Resource Not Found", ex.Message);
                break;
            case ArgumentException:
                statusCode = StatusCodes.Status400BadRequest;
                problem = CreateProblemDetails(context, statusCode, "Resource Not Found", ex.Message);
                break;
            default:
                statusCode = StatusCodes.Status500InternalServerError;
                problem = CreateProblemDetails(context, statusCode, "Internal Server Error", "An unexpected error occurred. Please try again later.");
                break;
        }

        context.Response.StatusCode = statusCode;
        var json = JsonSerializer.Serialize(problem, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        await context.Response.WriteAsync(json);
    }

    /// <summary>
    /// Provides a standardized way to create a ProblemDetails object for different types of exceptions.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="statusCode"></param>
    /// <param name="title"></param>
    /// <param name="details"></param>
    /// <returns></returns>
    private ProblemDetails CreateProblemDetails(
        HttpContext context,
        int statusCode,
        string title,
        string details)
    {
        return new ProblemDetails
        {
            Type = $"https://httpstatuses.com/{statusCode}",
            Title = title,
            Status = statusCode,
            Detail = details,
            Instance = context.Request.Path
        };
    }

    /// <summary>
    /// Creates a ProblemDetails object specifically for handling validation exceptions.
    /// It extracts the validation errors from the exception and includes them in the response, 
    /// providing a clear and structured way for clients to understand what went wrong with their request.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="validationException"></param>
    /// <param name="statusCode"></param>
    /// <returns></returns>
    private ProblemDetails CreateValidationProblemDetails(HttpContext context, ValidationException validationException, int statusCode)
    {
        var errors = validationException.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
        var problem = new ProblemDetails
        {
            Type = "",
            Title = "One or more errors occured!!",
            Status = statusCode,
            Detail = "See the 'errors' property for more details!",
            Instance = context.Request.Path
        };
        problem.Extensions["errors"] = errors;
        return problem;
    }
}
