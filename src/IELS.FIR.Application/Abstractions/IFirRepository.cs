using IELS.FIR.Domain.Entities;

namespace IELS.FIR.Application.Abstractions;

public interface IFirRepository
{
    Task AddAsync(
        Fir fir,
        CancellationToken cancellationToken = default);
}