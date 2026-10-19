using IELS.FIR.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using IELS.FIR.Infrastructure.Persistence.Outbox;
namespace IELS.FIR.Infrastructure.Persistence;

public sealed class FirDbContext(
    DbContextOptions<FirDbContext> options)
    : DbContext(options)
{
    public DbSet<Fir> Firs => Set<Fir>();
    public DbSet<OutboxMessage> OutboxMessages
    => Set<OutboxMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(FirDbContext).Assembly);
    }
}