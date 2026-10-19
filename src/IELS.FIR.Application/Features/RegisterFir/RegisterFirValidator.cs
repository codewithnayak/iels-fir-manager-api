
using FluentValidation;

namespace IELS.FIR.Application.Features.RegisterFir;

public sealed class RegisterFirValidator
    : AbstractValidator<RegisterFirCommand>
{
    public RegisterFirValidator()
    {
        RuleFor(x => x.FirNumber)
            .NotEmpty()
            .Matches(@"^[A-Za-z0-9-]{3,30}$")
            .WithMessage("Invalid FIR number format.");

        RuleFor(x => x.PoliceStationId)
            .NotEmpty()
            .WithMessage("Police station is required.");

        RuleFor(x => x.DistrictId)
            .NotEmpty()
            .WithMessage("District is required.");

        RuleFor(x => x.IncidentDate)
            .LessThanOrEqualTo(_ => DateTimeOffset.UtcNow)
            .WithMessage("Incident date cannot be in the future.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .MinimumLength(20)
            .MaximumLength(5000);

        RuleFor(x => x.ComplainantName)
            .NotEmpty()
            .WithMessage("Complainant name is required.");

        RuleFor(x => x.IncidentLocation)
            .NotNull()
            .WithMessage("Incident location is required.");

        When(x => x.IncidentLocation is not null, () =>
        {
            RuleFor(x => x.IncidentLocation!)
                .SetValidator(new IncidentLocationValidator());
        });
    }
}

public sealed class IncidentLocationValidator
    : AbstractValidator<IncidentLocationDto>
{
    public IncidentLocationValidator()
    {
        RuleFor(x => x.AddressLine1)
            .NotEmpty()
            .MaximumLength(250);

        RuleFor(x => x.AddressLine2)
            .MaximumLength(250);

        RuleFor(x => x.VillageOrCity)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.District)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.State)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.PinCode)
            .Matches(@"^[1-9][0-9]{5}$")
            .When(x => !string.IsNullOrWhiteSpace(x.PinCode))
            .WithMessage("PIN code must contain 6 digits.");

        RuleFor(x => x.Latitude)
            .InclusiveBetween(-90m, 90m)
            .When(x => x.Latitude.HasValue);

        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180m, 180m)
            .When(x => x.Longitude.HasValue);

        RuleFor(x => x)
            .Must(x => x.Latitude.HasValue == x.Longitude.HasValue)
            .WithMessage(
                "Latitude and longitude must be provided together.");
    }
}
