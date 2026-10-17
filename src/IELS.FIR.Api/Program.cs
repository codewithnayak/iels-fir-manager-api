
using IELS.FIR.Api.ExceptionHandling;
using IELS.FIR.Application;
using IELS.FIR.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddControllers();
builder.Services.AddApplication();
builder.Services.AddInfrastructure();

var app = builder.Build();
app.UseExceptionHandler();
app.MapControllers();

await app.RunAsync();


