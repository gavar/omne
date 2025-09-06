namespace OMNE.Postgres;

public static class PostgresSetup
{
    private const string Connection = "postgres";

    public static void SetupPostgres(this IHostApplicationBuilder host, string connection = Connection)
    {
        host.AddNpgsqlDbContext<PostgresContext>(connection, default, ConfigureContext);
        host.Services.AddScoped<SqlContext>(static provider => provider.GetRequiredService<PostgresContext>());
    }

    private static void ConfigureContext(this DbContextOptionsBuilder builder)
    {
        builder
            .UseNpgsql(ConfigurePostgres)
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution)
            .UsePostgreSqlTriggers();

        EnableDebugLogs(builder);
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

    private static void ConfigurePostgres(NpgsqlDbContextOptionsBuilder builder)
    {
        builder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
    }
}
