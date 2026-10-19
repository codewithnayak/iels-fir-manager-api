
using FluentValidation;
using IELS.FIR.Application.Features.GetFirByNumber;
using IELS.FIR.Application.Features.RegisterFir;
using Microsoft.Extensions.DependencyInjection;

namespace IELS.FIR.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddSingleton(TimeProvider.System);

        services.AddScoped<
            IValidator<RegisterFirCommand>,
            RegisterFirValidator>();

        services.AddScoped<RegisterFirHandler>();
        services.AddScoped<GetFirByNumberHandler>();
        return services;
    }
}
