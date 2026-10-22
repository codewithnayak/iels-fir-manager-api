using System.Diagnostics;
using IELS.FIR.Api.Contracts;
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
    private string TraceId =>
        Activity.Current?.Id ?? HttpContext.TraceIdentifier;

    [HttpPost]
    [ProducesResponseType(
        typeof(ApiResponse<RegisterFirResponse>),
        StatusCodes.Status201Created)]
    [ProducesResponseType(
        typeof(ApiResponse<object>),
        StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<RegisterFirResponse>>> Register(
        [FromBody] RegisterFirCommand command,
        CancellationToken cancellationToken)
    {
        var id = await registerHandler.HandleAsync(
            command,
            cancellationToken);

        var response = ApiResponse<RegisterFirResponse>.Ok(
            new RegisterFirResponse(id),
            TraceId);

        return StatusCode(
            StatusCodes.Status201Created,
            response);
    }

    [HttpGet("/api/police-stations/{policeStationId}/firs/{firNumber}")]
    [ProducesResponseType(
        typeof(ApiResponse<FirResponse>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        typeof(ApiResponse<object>),
        StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<FirResponse>>> GetByNumber(
        [FromRoute] string policeStationId,
        [FromRoute] string firNumber,
        CancellationToken cancellationToken)
    {
        var result = await getHandler.HandleAsync(
            policeStationId,
            firNumber,
            cancellationToken);

        if (result is not null)
            return Ok(ApiResponse<FirResponse>.Ok(
                result,
                TraceId));
        
        var error = ApiResponse<FirResponse>.Fail(
            [
                new ApiError(
                    "FIR_NOT_FOUND",
                    "The requested FIR could not be found.")
            ],
            TraceId);

        return NotFound(error);

    }
}

public sealed record RegisterFirResponse(Guid Id);