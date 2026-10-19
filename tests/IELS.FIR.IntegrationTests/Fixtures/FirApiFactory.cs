
using System.Collections.Concurrent;
using IELS.FIR.Application.Abstractions;
using IELS.FIR.Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace IELS.FIR.IntegrationTests.Fixtures;

public sealed class FirApiFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(
        Microsoft.AspNetCore.Hosting.IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<IFirRepository>();
            services.AddSingleton<IFirRepository>(new TestFirRepository());
        });
    }

    private sealed class TestFirRepository : IFirRepository
    {
        private readonly ConcurrentDictionary<(string, string), Fir> _firs = new();

        public Task AddAsync(Fir fir, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            _firs[(fir.PoliceStationId, fir.FirNumber.Value)] = fir;
            return Task.CompletedTask;
        }

        public Task<Fir?> GetByNumberAsync(
            string policeStationId,
            string firNumber,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            _firs.TryGetValue((policeStationId, firNumber), out var fir);
            return Task.FromResult(fir);
        }
    }
}
