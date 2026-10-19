using IELS.FIR.Domain.Entities;

namespace IELS.FIR.Application.Abstractions;

public interface IFirRepository
{
    Task AddAsync(
        Fir fir,
        CancellationToken cancellationToken = default);
    
    Task<Fir?> GetByNumberAsync(
        string policeStationId,
        string firNumber,
        CancellationToken cancellationToken = default);
}