
using FluentValidation;
using IELS.FIR.Application.Features.RegisterFir;
using IELS.FIR.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace IELS.FIR.Api.Controllers;

[ApiController]
[Route("api/firs")]
public sealed class FirsController : ControllerBase
{
    private readonly RegisterFirHandler _handler;

    public FirsController(RegisterFirHandler handler)
    {
        _handler = handler;
    }

    [HttpPost]
    [ProducesResponseType(
        typeof(RegisterFirResponse),
        StatusCodes.Status201Created)]
    [ProducesResponseType(
        typeof(ValidationProblemDetails),
        StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(
        [FromBody] RegisterFirCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            var id = await _handler.HandleAsync(
                command,
                cancellationToken);

            return StatusCode(
                StatusCodes.Status201Created,
                new RegisterFirResponse(id));
        }
        catch (ValidationException exception)
        {
            var errors = exception.Errors
                .GroupBy(x => x.PropertyName)
                .ToDictionary(
                    group => group.Key,
                    group => group.Select(x => x.ErrorMessage)
                        .ToArray());

            return BadRequest(new ValidationProblemDetails(errors));
        }
        catch (DomainException exception)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Domain validation failed",
                Detail = exception.Message,
                Status = StatusCodes.Status400BadRequest
            });
        }
    }
}

public sealed record RegisterFirResponse(Guid Id);
