
using IELS.FIR.Domain.Exceptions;

namespace IELS.FIR.Domain.ValueObjects;

public sealed record IncidentLocation
{
    public string AddressLine1 { get; }
    public string? AddressLine2 { get; }
    public string VillageOrCity { get; }
    public string District { get; }
    public string State { get; }
    public string? PinCode { get; }
    public decimal? Latitude { get; }
    public decimal? Longitude { get; }

    private IncidentLocation(
        string addressLine1,
        string? addressLine2,
        string villageOrCity,
        string district,
        string state,
        string? pinCode,
        decimal? latitude,
        decimal? longitude)
    {
        AddressLine1 = addressLine1;
        AddressLine2 = addressLine2;
        VillageOrCity = villageOrCity;
        District = district;
        State = state;
        PinCode = pinCode;
        Latitude = latitude;
        Longitude = longitude;
    }

    public static IncidentLocation Create(
        string addressLine1,
        string? addressLine2,
        string villageOrCity,
        string district,
        string state,
        string? pinCode = null,
        decimal? latitude = null,
        decimal? longitude = null)
    {
        if (string.IsNullOrWhiteSpace(addressLine1))
            throw new DomainException("Incident address is required.");

        if (string.IsNullOrWhiteSpace(villageOrCity))
            throw new DomainException("Village or city is required.");

        if (string.IsNullOrWhiteSpace(district))
            throw new DomainException("Incident district is required.");

        if (string.IsNullOrWhiteSpace(state))
            throw new DomainException("Incident state is required.");

        if (latitude is < -90 or > 90)
            throw new DomainException("Invalid latitude.");

        if (longitude is < -180 or > 180)
            throw new DomainException("Invalid longitude.");

        return new IncidentLocation(
            addressLine1.Trim(),
            addressLine2?.Trim(),
            villageOrCity.Trim(),
            district.Trim(),
            state.Trim(),
            pinCode?.Trim(),
            latitude,
            longitude);
    }
}
