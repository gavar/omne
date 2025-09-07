using FastEndpoints.Swagger;
using OMNE.Data.Services;
using OMNE.Postgres;
using OMNE.ServiceDefaults;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    ContentRootPath = AppContext.BaseDirectory,
});

// Aspire
builder.AddServiceDefaults();

// Health
builder.Services
    .AddHealthChecks()
    .AddDbContextCheck<PostgresContext>();

// Persistence
// TODO: should migrate only via CI/CD
builder.Services.AddHostedService<DbInitializerService<PostgresContext>>();
builder.SetupPostgres();

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyHeader();
        policy.WithMethods("OPTIONS", "GET", "PUT", "POST", "DELETE");
        policy.WithOrigins(builder.Configuration.GetServiceEndpoints("web"));
    });
});

// Error Handling
builder.Services.AddProblemDetails();

// Endpoints
builder.Services
    .AddFastEndpoints()
    .SwaggerDocument();

var app = builder.Build();

// Middleware
app.UseHttpsRedirection();
app.UseHealthChecks("/health");
app.UseCors();

// Endpoints
app.UseFastEndpoints(x => x.Validation.EnableDataAnnotationsSupport = true)
    .UseSwaggerGen();

app.Run();
