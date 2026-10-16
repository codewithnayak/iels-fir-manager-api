namespace IELS.FIR.Application.Features.RegisterFir;

public sealed record RegisterFirCommand(
    string FirNumber,
    string PoliceStationId,
    string DistrictId,
    DateTimeOffset IncidentDate,
    string Description,
    string ComplainantName
);

