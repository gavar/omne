using FastEndpoints.Swagger;
using OMNE.Data.Services;
using OMNE.Postgres;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    ContentRootPath = AppContext.BaseDirectory,
});

// Health
builder.Services
    .AddHealthChecks()
    .AddDbContextCheck<PostgresContext>();

// TODO: should migrate only via CI/CD
builder.Services.AddHostedService<DbInitializerService<PostgresContext>>();
builder.SetupPostgres();

// Error Handling
builder.Services.AddProblemDetails();

// Endpoints
builder.Services
    .AddFastEndpoints()
    .SwaggerDocument();

var app = builder.Build();

// Middleware
app.UseHealthChecks("/health");

// Endpoints
app.UseFastEndpoints().UseSwaggerGen();

app.Run();
