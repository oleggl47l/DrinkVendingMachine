using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace DrinkVendingMachine.Api.Handlers;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var (statusCode, title) = exception switch
        {
            ArgumentException => (StatusCodes.Status400BadRequest, "Invalid input"),
            ValidationException => (StatusCodes.Status400BadRequest, "Validation failed"),
            FormatException => (StatusCodes.Status400BadRequest, "Invalid format"),
            InvalidDataException => (StatusCodes.Status400BadRequest, "Invalid data"),
            NotSupportedException => (StatusCodes.Status400BadRequest, "Operation not supported"),
            InvalidOperationException => (StatusCodes.Status400BadRequest, "Operation is not valid"),
            KeyNotFoundException => (StatusCodes.Status404NotFound, "Resource not found"),
            TimeoutException or TaskCanceledException => (StatusCodes.Status408RequestTimeout, "Request timed out"),
            IOException => (StatusCodes.Status500InternalServerError, "I/O error occurred"),
            _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred")
        };

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = exception.Message,
            Instance = httpContext.Request.Path
        };

        logger.Log(LogLevelFromStatus(statusCode), exception, "Unhandled exception");

        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }

    private static LogLevel LogLevelFromStatus(int status) =>
        status >= 500 ? LogLevel.Error : LogLevel.Warning;
}
