using DrinkVendingMachine.Domain.Exceptions;
using DrinkVendingMachine.Domain.Exceptions.Brand;
using DrinkVendingMachine.Domain.Exceptions.Coin;
using DrinkVendingMachine.Domain.Exceptions.Drink;
using DrinkVendingMachine.Domain.Exceptions.Specific;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace DrinkVendingMachine.Api.Handlers;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    private readonly Dictionary<int, string> _statusToTypeMap = new()
    {
        [StatusCodes.Status400BadRequest] = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
        [StatusCodes.Status404NotFound] = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
        [StatusCodes.Status408RequestTimeout] = "https://tools.ietf.org/html/rfc7231#section-6.5.7",
        [StatusCodes.Status500InternalServerError] = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
    };

    private readonly Dictionary<int, List<(Type ExceptionType, string Title)>> _exceptionMap = new()
    {
        [StatusCodes.Status400BadRequest] =
        [
            (typeof(InvalidOperationException), "Invalid operation."),
            (typeof(BrandNameNotUniqueException), "Brand name must be unique."),
            (typeof(InvalidDrinkPriceException), "Invalid drink price."),
            (typeof(InvalidDrinkQuantityException), "Invalid drink quantity."),
            (typeof(NotEnoughDrinkStockException), "Not enough drink stock."),
            (typeof(UnableToGiveChangeException), "Unable to give change."),
            (typeof(NotEnoughMoneyInsertedException), "Not enough money inserted.")
        ],
        [StatusCodes.Status404NotFound] =
        [
            (typeof(KeyNotFoundException), "The requested resource was not found."),
            (typeof(BrandNotFoundException), "Brand not found."),
            (typeof(CoinNotFoundException), "Coin not found."),
            (typeof(DrinkNotFoundException), "Drink not found.")
        ],
        [StatusCodes.Status408RequestTimeout] =
        [
            (typeof(TaskCanceledException), "Request timed out."),
            (typeof(TimeoutException), "Request timed out.")
        ],
        [StatusCodes.Status500InternalServerError] =
        [
            (typeof(Microsoft.EntityFrameworkCore.DbUpdateException), "A database error occurred.")
        ]
    };

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        var problemDetails = CreateProblemDetails(httpContext, exception);
        LogException(exception, problemDetails);

        if (problemDetails.Status != null)
            httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken).ConfigureAwait(false);
        return true;
    }

    private ProblemDetails CreateProblemDetails(HttpContext httpContext, Exception exception)
    {
        var problemDetails = new ProblemDetails
        {
            Instance = httpContext.Request.Path,
            Status = StatusCodes.Status500InternalServerError,
            Title = "An unexpected error occurred.",
            Type = _statusToTypeMap[StatusCodes.Status500InternalServerError]
        };

        if (exception is CustomException custom)
        {
            problemDetails.Extensions["errorName"] = exception.GetType().Name.Replace("Exception", "");
            problemDetails.Extensions["args"] = custom.Args;
        }
        else
        {
            problemDetails.Extensions["errorName"] = "Unhandled";
            problemDetails.Extensions["args"] = new[]
            {
                new CustomExceptionArgument("Message", exception.Message),
            };
        }

        foreach (var (statusCode, definitions) in _exceptionMap)
        {
            var match = definitions.FirstOrDefault(def => def.ExceptionType.IsAssignableFrom(exception.GetType()));
            if (match.ExceptionType == null) continue;
            problemDetails.Status = statusCode;
            problemDetails.Title = match.Title;
            problemDetails.Type = _statusToTypeMap[statusCode];
            break;
        }

        return problemDetails;
    }


    private void LogException(Exception exception, ProblemDetails problemDetails)
    {
        switch (problemDetails.Status)
        {
            case >= 500:
                logger.LogError(exception, "{Title}", problemDetails.Title);
                break;
            case >= 400:
                logger.LogWarning("{Title}: {Message}", problemDetails.Title, exception.Message);
                break;
        }
    }
}