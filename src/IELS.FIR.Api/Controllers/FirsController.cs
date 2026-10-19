using IELS.FIR.Application.Features.RegisterFir;
using IELS.FIR.Application.Features.GetFirByNumber;
using Microsoft.AspNetCore.Mvc;

namespace IELS.FIR.Api.Controllers;

[ApiController]
[Route("api/firs")]
public sealed class FirsController(
    RegisterFirHandler registerHandler,
    GetFirByNumberHandler getHandler) : ControllerBase
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
        var id = await registerHandler.HandleAsync(
            command,
            cancellationToken);

        return StatusCode(
            StatusCodes.Status201Created,
            new RegisterFirResponse(id));
    }

    [HttpGet("/api/police-stations/{policeStationId}/firs/{firNumber}")]
    [ProducesResponseType(
        typeof(FirResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FirResponse>> GetByNumber(
        [FromRoute] string policeStationId,
        [FromRoute] string firNumber,
        CancellationToken cancellationToken)
    {
        var result = await getHandler.HandleAsync(
            policeStationId,
            firNumber,
            cancellationToken);

        if (result is null)
            return NotFound();

        return Ok(result);
    }
}

public sealed record RegisterFirResponse(Guid Id);