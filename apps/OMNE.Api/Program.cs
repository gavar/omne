using OMNE.Data;
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
    .AddDbContextCheck<SqlContext>();

// Persistence
if (!builder.Environment.IsEnvironment("test"))
{
    // TODO: should migrate only via CI/CD
    builder.Services.AddHostedService<DbInitializerService<PostgresContext>>();
    builder.SetupPostgres();
}

// Endpoints
builder.Services.AddProblemDetails();

var app = builder.Build();

// Middleware
app.UseHealthChecks("/health");

app.Run();
