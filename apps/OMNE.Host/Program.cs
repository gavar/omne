var builder = DistributedApplication.CreateBuilder(new DistributedApplicationOptions
{
    Args = args,
    EnableResourceLogging = true,
});

// Postgres
var postgres = builder
    .AddPostgres("postgres")
    .WithImage("postgres:17.6")
    .WithContainerName("omne-psql")
    .WithDataVolume("omne-psql-data");

// DB
var db = postgres.AddDatabase("omne-dev");

// API
builder
    .AddProject<Projects.OMNE_Api>("api")
    .WithReference(db, connectionName: "postgres")
    .WaitFor(db);

builder.Build().Run();
