namespace IELS.FIR.Application.Features.GetFirByNumber;

public sealed record FirResponse(
    Guid Id,
    string FirNumber,
    string PoliceStationId,
    string DistrictId,
    DateTimeOffset IncidentDate,
    string Description,
    string ComplainantName,
    string Status,
    DateTimeOffset RegisteredAtUtc,
    FirLocationResponse IncidentLocation
);

public sealed record FirLocationResponse(
    string AddressLine1,
    string? AddressLine2,
    string VillageOrCity,
    string District,
    string State,
    string? PinCode,
    decimal? Latitude,
    decimal? Longitude
);