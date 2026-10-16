
using System.Collections.Concurrent;
using IELS.FIR.Application.Abstractions;
using IELS.FIR.Domain.Entities;

namespace IELS.FIR.Infrastructure.Persistence;

public sealed class InMemoryFirRepository : IFirRepository
{
    private readonly ConcurrentDictionary<Guid, Fir> _firs = new();

    public Task AddAsync(
        Fir fir,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        ArgumentNullException.ThrowIfNull(fir);

        if (!_firs.TryAdd(fir.Id, fir))
        {
            throw new InvalidOperationException(
                $"FIR with ID {fir.Id} already exists.");
        }

        return Task.CompletedTask;
    }
}
