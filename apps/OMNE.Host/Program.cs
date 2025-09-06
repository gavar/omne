using Projects;

var builder = DistributedApplication.CreateBuilder(new DistributedApplicationOptions
{
    Args = args,
    EnableResourceLogging = true,
});

// Postgres
var postgres = builder
    .AddPostgres("postgres")
    .WithImage("postgres:17.6")
    .WithPassword(builder.AddParameter("psql-password", "root"))
    .WithContainerName("omne-psql")
    .WithDataVolume("omne-psql-data");

// DB
var db = postgres.AddDatabase("omne-dev");

// API
var api = builder
    .AddProject<OMNE_Api>("api")
    .WithReference(db, connectionName: "postgres")
    .WithHttpEndpoint(5001)
    .WaitFor(db);

// WASM
var web = builder
    .AddStandaloneBlazorWebAssemblyProject<OMNE_Web>("web")
    .WithReference(api);

api.WithReference(web);

builder.Build().Run();
