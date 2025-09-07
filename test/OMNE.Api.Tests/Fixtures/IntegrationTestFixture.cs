using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using OMNE.Api.Fixtures;
using OMNE.Postgres;
using Testcontainers.PostgreSql;

[assembly: AssemblyFixture(typeof(IntegrationTestFixture))]

namespace OMNE.Api.Fixtures;

public class IntegrationTestFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer postgres = new PostgreSqlBuilder()
        .WithImage("postgres:17.6")
        .WithDatabase("omne-test")
        .WithCleanUp(true)
        .WithReuse(false)
        .Build();

    private WebApplicationFactory<Program> factory = default!;

    /// <summary> HTTP client to make requests. </summary>
    public HttpClient Http { get; private set; } = default!;

    /// <summary> Delete existing data and create a new one from scratch. </summary>
    public async Task Truncate()
    {
        using var scope = factory.Services.CreateScope();
        await using var context = scope.ServiceProvider.GetRequiredService<PostgresContext>();
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();
    }

    /// <inheritdoc />
    [SuppressMessage("Reliability", "CA2000", Justification = "Builder")]
    public async ValueTask InitializeAsync()
    {
        await postgres.StartAsync();
        factory = new WebApplicationFactory<Program>().WithWebHostBuilder(ConfigureWebHost);
        Http = factory.CreateClient();
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        await postgres.DisposeAsync();
        await factory.DisposeAsync();
    }

    private void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("ConnectionStrings:postgres", postgres.GetConnectionString());
    }
}
