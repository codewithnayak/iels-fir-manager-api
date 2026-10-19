
using IELS.FIR.Application.Abstractions;
using IELS.FIR.Domain.Entities;
using IELS.FIR.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace IELS.FIR.Infrastructure.Persistence.Repositories;

public sealed class SqlFirRepository(FirDbContext dbContext)
    : IFirRepository
{
    public async Task AddAsync(
        Fir fir,
        CancellationToken cancellationToken = default)
    {
        await dbContext.Firs.AddAsync(
            fir,
            cancellationToken);

        await dbContext.SaveChangesAsync(
            cancellationToken);
    }
    
    public Task<Fir?> GetByNumberAsync(
        string policeStationId,
        string firNumber,
        CancellationToken cancellationToken = default)
    {
        var normalizedStationId = policeStationId.Trim();
        var normalizedFirNumber = FirNumber.Create(firNumber);

        return dbContext.Firs
            .AsNoTracking()
            .SingleOrDefaultAsync(
                fir =>
                    fir.PoliceStationId == normalizedStationId &&
                    fir.FirNumber == normalizedFirNumber,
                cancellationToken);
    }
}
