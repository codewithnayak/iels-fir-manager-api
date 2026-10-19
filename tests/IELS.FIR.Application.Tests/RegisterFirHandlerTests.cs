
using FluentValidation;
using IELS.FIR.Application.Abstractions;
using IELS.FIR.Application.Features.RegisterFir;
using IELS.FIR.Domain.Entities;
using Moq;

namespace IELS.FIR.Application.Tests;

public sealed class RegisterFirHandlerTests
{
    private static readonly DateTimeOffset FixedTime =
        new(2026, 10, 16, 10, 0, 0, TimeSpan.Zero);

    
    private static RegisterFirCommand ValidCommand() => new(
        "FIR-2026-001",
        "PS-001",
        "DIST-001",
        FixedTime.AddDays(-1),
        "A sample incident description containing enough details.",
        "Test Complainant",
        new IncidentLocationDto(
            AddressLine1: "Main Road, Near Central Market",
            AddressLine2: "Opposite Bus Stand",
            VillageOrCity: "Bhubaneswar",
            District: "Khordha",
            State: "Odisha",
            PinCode: "751001",
            Latitude: 20.2961m,
            Longitude: 85.8245m
        )
    );


    [Fact]
    public async Task HandleAsync_ValidCommand_SavesFirAndReturnsId()
    {
        var repository = new Mock<IFirRepository>();

        Fir? savedFir = null;

        repository
            .Setup(x => x.AddAsync(
                It.IsAny<Fir>(),
                It.IsAny<CancellationToken>()))
            .Callback<Fir, CancellationToken>(
                (fir, _) => savedFir = fir)
            .Returns(Task.CompletedTask);

        var handler = CreateHandler(repository.Object);

        var id = await handler.HandleAsync(ValidCommand());

        Assert.NotEqual(Guid.Empty, id);
        Assert.NotNull(savedFir);
        Assert.Equal(id, savedFir.Id);
        Assert.Equal("FIR-2026-001", savedFir.FirNumber.Value);
        Assert.Equal(FixedTime, savedFir.RegisteredAtUtc);

        repository.Verify(x => x.AddAsync(
            It.IsAny<Fir>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_InvalidCommand_DoesNotSaveFir()
    {
        var repository = new Mock<IFirRepository>();

        var handler = CreateHandler(repository.Object);

        var command = ValidCommand() with
        {
            Description = "Short"
        };

        await Assert.ThrowsAsync<ValidationException>(
            () => handler.HandleAsync(command));

        repository.Verify(x => x.AddAsync(
            It.IsAny<Fir>(),
            It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task HandleAsync_RepositoryFails_PropagatesException()
    {
        var repository = new Mock<IFirRepository>();

        repository
            .Setup(x => x.AddAsync(
                It.IsAny<Fir>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException(
                "Persistence failure"));

        var handler = CreateHandler(repository.Object);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => handler.HandleAsync(ValidCommand()));
    }

    private static RegisterFirHandler CreateHandler(
        IFirRepository repository)
    {
        var validator = new RegisterFirValidator();

        var timeProvider = new Mock<TimeProvider>();

        timeProvider
            .Setup(x => x.GetUtcNow())
            .Returns(FixedTime);

        return new RegisterFirHandler(
            repository,
            validator,
            timeProvider.Object);
    }
}
