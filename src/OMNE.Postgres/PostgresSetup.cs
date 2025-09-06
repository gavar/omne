using MicroOrm.Dapper.Repositories;
using MicroOrm.Dapper.Repositories.SqlGenerator;
using Microsoft.Extensions.Configuration;
using Npgsql;
using OMNE.EFCore;

namespace OMNE.Postgres;

public static class PostgresSetup
{
    public static void SetupPostgres(this IHostApplicationBuilder host)
    {
        // EF
        host.Services.AddPooledDbContext<SqlContext, PostgresContext>(ConfigureContext);

        // Dapper
        host.Services.AddSingleton(typeof(ISqlGenerator<>), typeof(PostgresSqlGenerator<>));
        host.Services.AddScoped(typeof(IDapperRepository<>), typeof(NpgsqlDapperRepository<>));
        host.Services.AddScoped(typeof(IReadOnlyDapperRepository<>), typeof(NpgsqlReadOnlyDapperRepository<>));
    }

    private static void ConfigureContext(IServiceProvider provider, DbContextOptionsBuilder builder)
    {
        var connection = provider
            .GetRequiredService<IConfiguration>()
            .GetConnectionString("postgres") ?? throw new NpgsqlException("Unable to resolve connection string");

        builder
            .UseNpgsql(connection, ConfigurePostgres)
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution)
            .UsePostgreSqlTriggers();

        EnableDebugLogs(builder);
    }

    private static void ConfigurePostgres(NpgsqlDbContextOptionsBuilder builder)
    {
        builder
            .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
            .EnableRetryOnFailure();
    }

    [Conditional("DEBUG")]
    [SuppressMessage("Reliability", "CA2000", Justification = "Disposed by Service Provider")]
    private static void EnableDebugLogs(DbContextOptionsBuilder builder)
    {
        builder
            .UseLoggerFactory(LoggerFactory.Create(ConfigureDebugLogger))
            .EnableSensitiveDataLogging();
    }

    private static void ConfigureDebugLogger(ILoggingBuilder builder)
    {
        builder
            .AddDebug()
            .AddConsole();
    }
}
