using System.Net;
using System.Net.Http.Json;
using IELS.FIR.Application.Features.RegisterFir;
using IELS.FIR.IntegrationTests.Fixtures;

namespace IELS.FIR.IntegrationTests;

public sealed class FirsEndpointTests
    : IClassFixture<FirApiFactory>
{
    private readonly HttpClient _client;

    public FirsEndpointTests(FirApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Register_WithValidRequest_ReturnsCreated()
    {
        var request = CreateValidRequest();

        var response = await _client.PostAsJsonAsync(
            "/api/firs", request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var result = await response.Content
            .ReadFromJsonAsync<RegisterFirResponse>();

        Assert.NotNull(result);
        Assert.NotEqual(Guid.Empty, result.Id);
    }

    [Theory]
    [InlineData("")]
    [InlineData("Short")]
    public async Task Register_WithInvalidDescription_ReturnsBadRequest(
        string description)
    {
        var request = CreateValidRequest() with
        {
            Description = description
        };

        var response = await _client.PostAsJsonAsync(
            "/api/firs", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Register_WithFutureIncidentDate_ReturnsBadRequest()
    {
        var request = CreateValidRequest() with
        {
            IncidentDate = DateTimeOffset.UtcNow.AddDays(2)
        };

        var response = await _client.PostAsJsonAsync(
            "/api/firs", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Register_WithMissingFirNumber_ReturnsBadRequest()
    {
        var request = CreateValidRequest() with
        {
            FirNumber = ""
        };

        var response = await _client.PostAsJsonAsync(
            "/api/firs", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    private static RegisterFirRequest CreateValidRequest()
    {
        return new RegisterFirRequest(
            "FIR-2026-001",
            "PS-001",
            "DIST-001",
            DateTimeOffset.UtcNow.AddDays(-1),
            "A reported incident containing sufficient information.",
            "Test Complainant",
            new IncidentLocationDto(
                "Main Road, Near Central Market",
                "Opposite Bus Stand",
                "Bhubaneswar",
                "Khordha",
                "Odisha",
                "751001",
                20.2961m,
                85.8245m));
    }

    private sealed record RegisterFirRequest(
        string FirNumber,
        string PoliceStationId,
        string DistrictId,
        DateTimeOffset IncidentDate,
        string Description,
        string ComplainantName,
        IncidentLocationDto IncidentLocation);

    public sealed record RegisterFirResponse(Guid Id);
}
