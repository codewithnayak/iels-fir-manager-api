using IELS.FIR.Application.Abstractions;

namespace IELS.FIR.Application.Features.GetFirByNumber;

public sealed class GetFirByNumberHandler(
    IFirRepository repository)
{
    public async Task<FirResponse?> HandleAsync(
        string policeStationId,
        string firNumber,
        CancellationToken cancellationToken = default)
    {
        var fir = await repository.GetByNumberAsync(
            policeStationId,
            firNumber,
            cancellationToken);

        if (fir is null)
            return null;

        var location = fir.IncidentLocation;

        return new FirResponse(
            fir.Id,
            fir.FirNumber.Value,
            fir.PoliceStationId,
            fir.DistrictId,
            fir.IncidentDate,
            fir.Description,
            fir.ComplainantName,
            fir.Status.ToString(),
            fir.RegisteredAtUtc,
            new FirLocationResponse(
                location.AddressLine1,
                location.AddressLine2,
                location.VillageOrCity,
                location.District,
                location.State,
                location.PinCode,
                location.Latitude,
                location.Longitude));
    }
}