
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace IELS.FIR.IntegrationTests.Fixtures;

public sealed class FirApiFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(
        Microsoft.AspNetCore.Hosting.IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
    }
}
