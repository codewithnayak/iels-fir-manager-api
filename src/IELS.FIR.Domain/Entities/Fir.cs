
using IELS.FIR.Domain.Enums;
using IELS.FIR.Domain.Exceptions;
using IELS.FIR.Domain.ValueObjects;

namespace IELS.FIR.Domain.Entities;

public sealed class Fir
{
    public Guid Id { get; private set; }
    public FirNumber FirNumber { get; private set; } = null!;
    public string PoliceStationId { get; private set; } = string.Empty;
    public string DistrictId { get; private set; } = string.Empty;
    public DateTimeOffset IncidentDate { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public string ComplainantName { get; private set; } = string.Empty;
    public FirStatus Status { get; private set; }
    public DateTimeOffset RegisteredAtUtc { get; private set; }

    private Fir() { }

    public static Fir Register(
        string firNumber,
        string policeStationId,
        string districtId,
        DateTimeOffset incidentDate,
        string description,
        string complainantName,
        DateTimeOffset registeredAtUtc)
    {
        if (string.IsNullOrWhiteSpace(policeStationId))
            throw new DomainException("Police station is required.");

        if (string.IsNullOrWhiteSpace(districtId))
            throw new DomainException("District is required.");

        if (string.IsNullOrWhiteSpace(description) ||
            description.Trim().Length is < 20 or > 5000)
            throw new DomainException(
                "Description must contain between 20 and 5000 characters.");

        if (string.IsNullOrWhiteSpace(complainantName))
            throw new DomainException("Complainant name is required.");

        if (incidentDate > registeredAtUtc)
            throw new DomainException("Incident date cannot be in the future.");

        return new Fir
        {
            Id = Guid.NewGuid(),
            FirNumber = FirNumber.Create(firNumber),
            PoliceStationId = policeStationId.Trim(),
            DistrictId = districtId.Trim(),
            IncidentDate = incidentDate,
            Description = description.Trim(),
            ComplainantName = complainantName.Trim(),
            Status = FirStatus.Registered,
            RegisteredAtUtc = registeredAtUtc.ToUniversalTime()
        };
    }
}
