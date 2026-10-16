using FluentValidation;
using IELS.FIR.Application.Abstractions;
using IELS.FIR.Domain.Entities;

namespace IELS.FIR.Application.Features.RegisterFir;

public sealed class RegisterFirHandler(
    IFirRepository repository,
    IValidator<RegisterFirCommand> validator,
    TimeProvider timeProvider)
{
    public async Task<Guid> HandleAsync(
        RegisterFirCommand command,
        CancellationToken cancellationToken = default)
    {
        await validator.ValidateAndThrowAsync(
            command,
            cancellationToken);

        var fir = Fir.Register(
            command.FirNumber,
            command.PoliceStationId,
            command.DistrictId,
            command.IncidentDate,
            command.Description,
            command.ComplainantName,
            timeProvider.GetUtcNow());

        await repository.AddAsync(fir, cancellationToken);

        return fir.Id;
    }
}