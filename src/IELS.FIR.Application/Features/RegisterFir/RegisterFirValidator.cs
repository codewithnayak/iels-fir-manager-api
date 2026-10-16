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
    }
}