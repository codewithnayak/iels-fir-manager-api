
using IELS.FIR.Domain.Entities;
using IELS.FIR.Domain.Enums;
using IELS.FIR.Domain.Exceptions;
using IELS.FIR.Domain.ValueObjects;

namespace IELS.FIR.Domain.Tests;

public class FirTests
{
    private static readonly DateTimeOffset RegistrationTime =
        new(2026, 10, 16, 10, 0, 0, TimeSpan.Zero);

    [Fact]
    public void Register_WithValidData_CreatesRegisteredFir()
    {
        var fir = CreateValidFir();

        Assert.NotEqual(Guid.Empty, fir.Id);
        Assert.Equal(FirStatus.Registered, fir.Status);
        Assert.Equal("FIR-2026-001", fir.FirNumber.Value);
        Assert.Equal(RegistrationTime, fir.RegisteredAtUtc);
    }

    [Fact]
    public void Register_WithFutureIncidentDate_ThrowsDomainException()
    {
        var exception = Assert.Throws<DomainException>(() =>
            CreateValidFir(RegistrationTime.AddDays(1)));

        Assert.Equal(
            "Incident date cannot be in the future.",
            exception.Message);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("Short")]
    public void Register_WithInvalidDescription_ThrowsDomainException(
        string description)
    {
        Assert.Throws<DomainException>(() =>
            Fir.Register(
                "FIR-2026-001",
                "PS-001",
                "DIST-001",
                RegistrationTime.AddDays(-1),
                description,
                "Test Complainant",
                RegistrationTime,
                IncidentLocation.Create("asdsa",
                    "asdasd" , "Yes","dsd","sdasd","7655241")));
    }

    [Theory]
    [InlineData("fir-2026-001", "FIR-2026-001")]
    [InlineData(" FIR-2026-001 ", "FIR-2026-001")]
    public void FirNumber_NormalizesValidValues(
        string input,
        string expected)
    {
        var number = FirNumber.Create(input);

        Assert.Equal(expected, number.Value);
    }

    [Theory]
    [InlineData("")]
    [InlineData("AB")]
    [InlineData("FIR/2026/001")]
    public void FirNumber_RejectsInvalidValues(string input)
    {
        Assert.Throws<DomainException>(() =>
            FirNumber.Create(input));
    }

    private static Fir CreateValidFir(
        DateTimeOffset? incidentDate = null)
    {
        return Fir.Register(
            "FIR-2026-001",
            "PS-001",
            "DIST-001",
            incidentDate ?? RegistrationTime.AddDays(-1),
            "A sample incident description containing enough details.",
            "Test Complainant",
            RegistrationTime,
            IncidentLocation.Create("asdsa",
                "asdasd" , "Yes","dsd","sdasd","7655241"));
    }
}
