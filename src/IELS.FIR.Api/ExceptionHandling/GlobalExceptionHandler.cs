using System.Diagnostics;
using FluentValidation;
using IELS.FIR.Api.Contracts;
using IELS.FIR.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace IELS.FIR.Api.ExceptionHandling;

public sealed class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var traceId =
            Activity.Current?.Id ?? httpContext.TraceIdentifier;

        var statusCode = exception switch
        {
            ValidationException or DomainException =>
                StatusCodes.Status400BadRequest,

            _ =>
                StatusCodes.Status500InternalServerError
        };

        var errors = exception switch
        {
            ValidationException validationException =>
                validationException.Errors
                    .Select(error => new ApiError(
                        "VALIDATION_ERROR",
                        error.ErrorMessage,
                        error.PropertyName))
                    .ToArray(),

            DomainException domainException =>
            [
                new ApiError(
                    "DOMAIN_VALIDATION_ERROR",
                    domainException.Message)
            ],

            _ =>
            [
                new ApiError(
                    "INTERNAL_SERVER_ERROR",
                    "An unexpected error occurred.")
            ]
        };

        if (statusCode == StatusCodes.Status500InternalServerError)
        {
            logger.LogError(
                exception,
                "Unhandled exception. TraceId: {TraceId}",
                traceId);
        }
        else
        {
            logger.LogWarning(
                "Request rejected. StatusCode: {StatusCode}, TraceId: {TraceId}",
                statusCode,
                traceId);
        }

        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsJsonAsync(
            ApiResponse<object>.Fail(errors, traceId),
            cancellationToken);

        return true;
    }
}