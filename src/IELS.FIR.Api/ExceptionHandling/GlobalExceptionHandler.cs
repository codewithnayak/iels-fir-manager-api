
using FluentValidation;
using IELS.FIR.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace IELS.FIR.Api.ExceptionHandling;

public sealed class GlobalExceptionHandler(
    IProblemDetailsService problemDetailsService,
    ILogger<GlobalExceptionHandler> logger)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var (statusCode, title) = exception switch
        {
            ValidationException =>
                (StatusCodes.Status400BadRequest,
                 "Validation failed"),

            DomainException =>
                (StatusCodes.Status400BadRequest,
                 "Domain validation failed"),

            _ =>
                (StatusCodes.Status500InternalServerError,
                 "An unexpected error occurred")
        };

        if (statusCode == 500)
        {
            logger.LogError(
                exception,
                "Unhandled exception. TraceId: {TraceId}",
                httpContext.TraceIdentifier);
        }

        httpContext.Response.StatusCode = statusCode;

        var problem = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = statusCode == 500
                ? "An unexpected error occurred."
                : exception.Message,
            Instance = httpContext.Request.Path
        };

        if (exception is ValidationException validationException)
        {
            var errors = validationException.Errors
                .GroupBy(x => x.PropertyName)
                .ToDictionary(
                    group => group.Key,
                    group => group.Select(x => x.ErrorMessage)
                        .ToArray());

            problem = new ValidationProblemDetails(errors)
            {
                Status = statusCode,
                Title = title,
                Instance = httpContext.Request.Path
            };
        }

        problem.Extensions["traceId"] =
            httpContext.TraceIdentifier;

        return await problemDetailsService.TryWriteAsync(
            new ProblemDetailsContext
            {
                HttpContext = httpContext,
                ProblemDetails = problem,
                Exception = exception
            });
    }
}
