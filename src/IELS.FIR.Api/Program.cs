
using Azure.Monitor.OpenTelemetry.AspNetCore;
using IELS.FIR.Api.ExceptionHandling;
using IELS.FIR.Application;
using IELS.FIR.Application.Features.GetFirByNumber;
using IELS.FIR.Infrastructure;
using IELS.FIR.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddControllers();
builder.Services.AddOpenApi("v1");
builder.Services.AddApplication();
builder.Services.AddInfrastructure();

builder.Services.AddDbContext<FirDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("FirDatabase")));


var appInsightsConnectionString =
    builder.Configuration["ApplicationInsights:ConnectionString"];


if (!string.IsNullOrEmpty(appInsightsConnectionString))
{
    builder.Services.AddOpenTelemetry()
        .UseAzureMonitor(options =>
        {
            options.ConnectionString =  appInsightsConnectionString;
        });
}

var app = builder.Build();
app.UseExceptionHandler();

if (app.Environment.IsDevelopment() ||
    app.Environment.IsEnvironment("Dev") ||
    app.Environment.IsEnvironment("QA"))
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint(
            "/openapi/v1.json",
            "IELS FIR API v1");

        options.RoutePrefix = "swagger";
    });
}


app.MapControllers();

await app.RunAsync();


