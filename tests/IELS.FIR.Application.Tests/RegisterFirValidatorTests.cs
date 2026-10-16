
using FluentValidation.TestHelper;
using IELS.FIR.Application.Features.RegisterFir;

namespace IELS.FIR.Application.Tests;

public sealed class RegisterFirValidatorTests
{
    private readonly RegisterFirValidator _validator = new();

    private static RegisterFirCommand ValidCommand() => new(
        "FIR-2026-001",
        "PS-001",
        "DIST-001",
        DateTimeOffset.UtcNow.AddDays(-1),
        "A sample incident description containing enough details.",
        "Test Complainant");

    [Fact]
    public void ValidCommand_ShouldNotHaveErrors()
    {
        var result = _validator.TestValidate(ValidCommand());

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData("")]
    [InlineData("AB")]
    [InlineData("FIR/2026/001")]
    public void InvalidFirNumber_ShouldHaveValidationError(
        string firNumber)
    {
        var command = ValidCommand() with
        {
            FirNumber = firNumber
        };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.FirNumber);
    }

    [Fact]
    public void EmptyPoliceStation_ShouldHaveValidationError()
    {
        var command = ValidCommand() with
        {
            PoliceStationId = ""
        };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.PoliceStationId);
    }

    [Fact]
    public void EmptyDistrict_ShouldHaveValidationError()
    {
        var command = ValidCommand() with
        {
            DistrictId = ""
        };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.DistrictId);
    }

    [Theory]
    [InlineData("")]
    [InlineData("Too short")]
    public void InvalidDescription_ShouldHaveValidationError(
        string description)
    {
        var command = ValidCommand() with
        {
            Description = description
        };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void FutureIncidentDate_ShouldHaveValidationError()
    {
        var command = ValidCommand() with
        {
            IncidentDate = DateTimeOffset.UtcNow.AddDays(1)
        };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.IncidentDate);
    }

    [Fact]
    public void EmptyComplainantName_ShouldHaveValidationError()
    {
        var command = ValidCommand() with
        {
            ComplainantName = ""
        };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.ComplainantName);
    }
}
