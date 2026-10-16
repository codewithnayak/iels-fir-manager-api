
using IELS.FIR.Application;
using IELS.FIR.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddApplication();
builder.Services.AddInfrastructure();

var app = builder.Build();

app.MapControllers();

await app.RunAsync();

public partial class Program { }
