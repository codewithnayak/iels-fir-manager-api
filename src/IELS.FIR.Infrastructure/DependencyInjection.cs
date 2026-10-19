
using IELS.FIR.Application.Abstractions;
using IELS.FIR.Infrastructure.Persistence;
using IELS.FIR.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace IELS.FIR.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services)
    {
        services.AddScoped<
            IFirRepository,
            SqlFirRepository>();
            

        return services;
    }
}
