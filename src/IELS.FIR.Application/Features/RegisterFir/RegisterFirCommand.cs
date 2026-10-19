namespace IELS.FIR.Application.Features.RegisterFir;

public sealed record RegisterFirCommand(
    string FirNumber,
    string PoliceStationId,
    string DistrictId,
    DateTimeOffset IncidentDate,
    string Description,
    string ComplainantName,
    IncidentLocationDto IncidentLocation
);

public sealed record IncidentLocationDto(
    string AddressLine1,
    string? AddressLine2,
    string VillageOrCity,
    string District,
    string State,
    string? PinCode,
    decimal? Latitude,
    decimal? Longitude
);