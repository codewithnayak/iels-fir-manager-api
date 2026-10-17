
using FluentValidation;
using IELS.FIR.Application.Features.RegisterFir;
using IELS.FIR.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace IELS.FIR.Api.Controllers;

[ApiController]
[Route("api/firs")]
public sealed class FirsController(RegisterFirHandler handler) : ControllerBase
{
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
        var id = await handler.HandleAsync(
            command,
            cancellationToken);

        return StatusCode(
            StatusCodes.Status201Created,
            new RegisterFirResponse(id));
    }
}

public sealed record RegisterFirResponse(Guid Id);
