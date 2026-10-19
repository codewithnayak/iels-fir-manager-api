using FluentValidation;
using IELS.FIR.Application.Abstractions;
using IELS.FIR.Domain.Entities;
using IELS.FIR.Domain.ValueObjects;

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

        var location = command.IncidentLocation;

        var incidentLocation = IncidentLocation.Create(
            location.AddressLine1,
            location.AddressLine2,
            location.VillageOrCity,
            location.District,
            location.State,
            location.PinCode,
            location.Latitude,
            location.Longitude);

        var fir = Fir.Register(
            command.FirNumber,
            command.PoliceStationId,
            command.DistrictId,
            command.IncidentDate,
            command.Description,
            command.ComplainantName,
            timeProvider.GetUtcNow(),
            incidentLocation);

        await repository.AddAsync(
            fir,
            cancellationToken);

        return fir.Id;
    }
}